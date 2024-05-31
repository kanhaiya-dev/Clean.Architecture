using Clean.Architecture.Core.Entities.Buisness;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Clean.Architecture.Infrastructure.Wrapper
{
    public class DapperDbConnectionWrapper : IDbConnectionWrapper
    {
        private readonly IDbConnection _connection;

        public DapperDbConnectionWrapper(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task<IEnumerable<Account>> QueryAsync(string sql, object param = null)
        {
            return _connection.QueryAsync<Account>(sql, param);
        }

        public Task<Account> QuerySingleOrDefaultAsync(string sql, object param = null)
        {
            return _connection.QuerySingleOrDefaultAsync<Account>(sql, param);
        }

        public Task<int> ExecuteAsync(string sql, object param = null)
        {
            return _connection.ExecuteAsync(sql, param);
        }
    }
}
