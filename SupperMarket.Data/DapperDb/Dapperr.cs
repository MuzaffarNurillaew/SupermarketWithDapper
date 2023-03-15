using Dapper;
using Npgsql;
using SupperMarket.Data.AppSettings;
using SupperMarket.Domain.Commons;
using System.Data;

namespace SupperMarket.Data.DapperDb
{
    public class Dapperr<TEntity> : IDapper<TEntity> where TEntity : Auditable
    {
        private IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(Configurations.CONNECTION_STRING);
        }
        public async Task DeleteAsync(string query, DynamicParameters @params = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = GetDbConnection())
            {
                connection.Open();

                await connection.ExecuteAsync(sql: query, param: @params, commandType: commandType);
            }
        }
        public void Check()
        {
            using (var connection = GetDbConnection())
            {
                connection.Open();
            }
        }
        public async Task InsertAsync(string query, DynamicParameters @params = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = GetDbConnection())
            {
                connection.Open();

                await connection.ExecuteAsync(sql: query, param: @params, commandType: commandType);
            }
        }

        public async Task<List<TEntity>> SelectAllAsync(string query, DynamicParameters @params = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = GetDbConnection())
            {
                connection.Open();

                return (await connection.QueryAsync<TEntity>(sql: query, param: @params, commandType: commandType)).ToList<TEntity>();
            }
        }

        public async Task<TEntity> SelectAsync(string query, DynamicParameters @params = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = GetDbConnection())
            {
                connection.Open();

                return await connection.QueryFirstOrDefaultAsync<TEntity>(sql: query, param: @params, commandType: commandType);
            }
        }

        public async Task UpdateAsync(string query, DynamicParameters @params = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = GetDbConnection())
            {
                connection.Open();

                await connection.ExecuteAsync(sql: query, param: @params, commandType: commandType);
            }
        }
    }
}
