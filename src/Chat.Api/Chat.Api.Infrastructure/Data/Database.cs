using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Chat.Api.Core.Configs;
using Chat.Api.Core.Interfaces;
using Dapper;

namespace Chat.Api.Infrastructure.Data
{
    public class Database : IDatabase
    {
        private readonly DatabaseConfig _config;
        public Database(DatabaseConfig config)
        {
            _config = config;
        }
        public async Task OperationalAsync(string sql)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql);
                await connection.CloseAsync();
            }
        }
        public async Task<int> ExecuteAsync(string sql)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync(sql);
                await connection.CloseAsync();
                return result;
            }
        }
        public async Task<int> ExecuteAsync(string sql, object param)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync(sql, param);
                await connection.CloseAsync();
                return result;
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<T>(sql);
                await connection.CloseAsync();
                return result;
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<T>(sql, param);
                await connection.CloseAsync();
                return result;
            }
        }
    }
}
