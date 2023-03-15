using Dapper;
using SupperMarket.Domain.Commons;
using System.Data;

namespace SupperMarket.Data.DapperDb
{
    public interface IDapper<TEntity> where TEntity : Auditable
    {
        Task InsertAsync(string query, 
                        DynamicParameters @params = null, 
                        CommandType commandType = CommandType.Text);
        Task UpdateAsync(string query,
                        DynamicParameters @params = null,
                        CommandType commandType = CommandType.Text);
        Task DeleteAsync(string query,
                        DynamicParameters @params = null,
                        CommandType commandType = CommandType.Text);
        Task<TEntity> SelectAsync(string query,
                        DynamicParameters @params = null,
                        CommandType commandType = CommandType.Text);
        Task<List<TEntity>> SelectAllAsync(string query,
                        DynamicParameters @params = null,
                        CommandType commandType = CommandType.Text);
        void Check();
    }
}
