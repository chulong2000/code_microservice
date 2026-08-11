using Microsoft.Data.SqlClient;

namespace DemoApi.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        Task<SqlConnection> CreateAsync(string name);
    }

    public sealed class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IReadOnlyDictionary<string, string> _connectionStrings;

        public SqlConnectionFactory(IReadOnlyDictionary<string, string> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public async Task<SqlConnection> CreateAsync(string name)
        {
            if (!_connectionStrings.TryGetValue(name, out var connectionString))
                throw new InvalidOperationException($"No connection string configured for '{name}'.");

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }

}
