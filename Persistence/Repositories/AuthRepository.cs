using Application.Interfaces.IRepository;
using Dapper;
using Domain.DTOs;
using WebAPI.Helpers;

namespace Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DbConnection _dbConnection;
        private readonly JwtHelper _jwtHelper;


        public AuthRepository(DbConnection dbConnection, JwtHelper jwtHelper)
        {
            _dbConnection = dbConnection;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO dto)
        {
            var sql = @"SELECT id, email, password_hash FROM users WHERE email = @Email";

            using var conn = _dbConnection.CreatePostgresConnection();
            var user = await conn.QuerySingleOrDefaultAsync<(int id, string email, string password_hash)>(sql, new { dto.Email });

            if (user.id == 0 || !BCrypt.Net.BCrypt.Verify(dto.Password, user.password_hash))
                throw new Exception("Credenciales inválidas");

            var token = _jwtHelper.GenerateToken(user.id, user.email);

            return new AuthResponseDTO { Token = token };
        }

        public async Task<AuthResponseDTO> Register(RegisterDTO dto)
        {
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var sql = @"
            INSERT INTO users (username, email, password_hash, phone, location)
            VALUES (@Username, @Email, @PasswordHash, @Phone, @Location)
            RETURNING id, email;";

            using var conn = _dbConnection.CreatePostgresConnection();
            var user = await conn.QuerySingleOrDefaultAsync<(int id, string email)>(sql, new
            {
                dto.Username,
                dto.Email,
                PasswordHash = hashPassword,
                dto.Phone,
                dto.Location
            });

            if (user.id == 0) throw new Exception("No se pudo registrar el usuario");

            var token = _jwtHelper.GenerateToken(user.id, user.email);

            return new AuthResponseDTO { Token = token };
        }
    }
}
