using Application.Interfaces.IRepository;
using Dapper;
using Domain.DTOs;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnection _dbConnection;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DbConnection dbConnection, ILogger<UserRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }
        public async Task<UserDetailsDTO> GetDetailsUser(int userId)
        {
            _logger.LogInformation("Inicio de proceso para obtener detalles del usuario logeado");
            string query = @"SELECT
                    username,
                    email,
                    phone,
                    profile_photo_url as ProfilePhotoUrl,
                    location as Location,
                    verified as Verified,
                    buyer_rating as BuyerRating,
                    seller_rating as SellerRating,
                    items_sold as ItemsSold,
                    items_bought as ItemsBought
                    FROM users WHERE id = @UserId";

            try
            {
                using (var connection = _dbConnection.CreatePostgresConnection())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@UserId", userId);

                    var resultado = await connection.QueryFirstOrDefaultAsync<UserDetailsDTO>(query, parametros);

                    if (resultado == null)
                    {
                        throw new Exception(message: "Usuario no existe");
                    }

                    _logger.LogInformation("Fin del proceso para obtener detalles del usuario logeado");
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles del usuario");
                throw;
            }
        }

        public async Task<bool> GetEmailExist(string email)
        {
            _logger.LogInformation("Inicio de proceso para validar si email existe");
            string query = @"SELECT EXISTS (
                        SELECT 1
                        FROM users
                        WHERE LOWER(email) = LOWER(@email)
                    ) AS email_exists;";

            try
            {
                using (var connection = _dbConnection.CreatePostgresConnection())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@email", email);

                    var resultado = await connection.QueryFirstOrDefaultAsync<bool>(query, parametros);

                    _logger.LogInformation("Fin del proceso para validar email");
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar el email");
                throw;
            }
        }
    }
}
