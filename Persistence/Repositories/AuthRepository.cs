using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Dapper;
using Domain.DTOs;
using Domain.Request;
using Microsoft.Extensions.Logging;
using WebAPI.Helpers;

namespace Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DbConnection _dbConnection;
        private readonly JwtHelper _jwtHelper;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthRepository> _logger;


        public AuthRepository(DbConnection dbConnection, JwtHelper jwtHelper, IEmailService emailService, ILogger<AuthRepository> logger)
        {
            _dbConnection = dbConnection;
            _jwtHelper = jwtHelper;
            _emailService = emailService;
            _logger = logger;
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


        public async Task GenerateAndSendResetPin(SendResetPassword sendResetPassword)
        {
            var pin = GenerateRandomPin();
            var expiration = DateTime.UtcNow.AddMinutes(15);// Luego de 15 minutos expira

            var sql = @"UPDATE users
                        SET reset_pin = @Pin,
                        reset_pin_expiration = @Expiration
                        WHERE LOWER(email) = LOWER(@Email)";

            using var conn = _dbConnection.CreatePostgresConnection();
            var rows = await conn.ExecuteAsync(sql, new { Pin = pin, Expiration = expiration, Email = sendResetPassword.Email });

            if (rows == 0)
                throw new Exception("No se encontró el correo proporcionado.");

            await _emailService.SendResetPinEmail(sendResetPassword.Email, pin);
        }



        private string GenerateRandomPin()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString(); // PIN de 4 dígitos
        }

        public async Task ResetPassword(ResetPasswordDTO dto)
        {
            var sqlSelect = @"SELECT id, reset_pin, reset_pin_expiration
                      FROM users
                      WHERE LOWER(email) = LOWER(@Email)";

            using var conn = _dbConnection.CreatePostgresConnection();
            var user = await conn.QueryFirstOrDefaultAsync<(int Id, string? Pin, DateTime? Expiration)>(sqlSelect, new { dto.Email });

            if (user.Id == 0 || user.Pin == null || user.Expiration == null)
                throw new Exception("El correo no tiene un PIN asignado o no existe.");

            if (user.Pin != dto.Pin)
                throw new Exception("El PIN ingresado es incorrecto.");

            if (user.Expiration < DateTime.UtcNow)
                throw new Exception("El PIN ha expirado.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            var sqlUpdate = @"UPDATE users
                      SET password_hash = @Password,
                          reset_pin = NULL,
                          reset_pin_expiration = NULL
                      WHERE id = @UserId";

            await conn.ExecuteAsync(sqlUpdate, new
            {
                Password = hashedPassword,
                UserId = user.Id
            });
        }

        public async Task<bool> ValidPin(ValidPin request)
        {
            _logger.LogInformation("Inicio de validación de PIN para el correo: {Email}", request.Email);

            const string sql = @"SELECT COUNT(1)
                         FROM users
                         WHERE LOWER(email) = LOWER(@Email)
                         AND reset_pin = @Pin
                         AND reset_pin_expiration > NOW()";

            try
            {
                using var connection = _dbConnection.CreatePostgresConnection();

                var parametros = new DynamicParameters();
                parametros.Add("@Email", request.Email);
                parametros.Add("@Pin", request.Pin);

                var existe = await connection.ExecuteScalarAsync<int>(sql, parametros);

                _logger.LogInformation("Fin de validación de PIN: {Resultado}", existe > 0);

                return existe > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar el PIN");
                throw;
            }
        }
    }
}
