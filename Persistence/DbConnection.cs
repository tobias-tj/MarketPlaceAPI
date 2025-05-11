using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Persistence
{
    public class DbConnection
    {
        private readonly IConfiguration _configuration;

        public DbConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreatePostgresConnection()
        {
            string environment = _configuration.GetValue<string>("AppSettings:Environment") ?? "Production";
            string connectionName = environment == "Development" ? "ConexionPostgresDev" : "ConexionPostgress";

            string postgresConnectionString = BuildPostgresConnectionString(_configuration, connectionName);

            return new NpgsqlConnection(postgresConnectionString);

        }

        private string BuildPostgresConnectionString(IConfiguration configuration, string connectionName)
        {
            var connectionSettings = configuration.GetSection($"ConnectionStrings:{connectionName}");

            if (connectionSettings == null || !connectionSettings.Exists())
            {
                throw new Exception(($"Detalles de conexión para '{connectionName}' no encontrados."));
            }

            var host = connectionSettings["Host"];
            var port = connectionSettings["Port"];
            var database = connectionSettings["Database"];
            var username = connectionSettings["Username"];
            var password = connectionSettings["Password"];
            var sslMode = connectionSettings["SslMode"] ?? "Require";
            var trustServerCertificate = connectionSettings["TrustServerCertificate"] ?? "true";

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(database) ||
          string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Uno o más parámetros requeridos de la cadena de conexión son nulos o están vacíos.");
            }


            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = int.Parse(port),
                Database = database,
                Username = username,
                Password = password,
                SslMode = Enum.TryParse<SslMode>(sslMode, true, out var parsedSslMode) ? parsedSslMode : SslMode.Require,
            };

            return builder.ConnectionString;

        }
    }
}
