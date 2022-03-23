using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Api.Core.Interfaces
{
    public interface IDatabase
    {
        Task OperationalAsync(string sql);
        Task<int> ExecuteAsync(string sql);
        Task<int> ExecuteAsync(string sql, object param);
        Task<IEnumerable<T>> QueryAsync<T>(string sql);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param);
    }
}