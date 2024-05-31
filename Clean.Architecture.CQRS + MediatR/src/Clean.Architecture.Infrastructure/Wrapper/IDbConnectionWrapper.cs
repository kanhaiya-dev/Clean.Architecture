using Clean.Architecture.Core.Entities.Buisness;

namespace Clean.Architecture.Infrastructure.Wrapper
{
    public interface IDbConnectionWrapper
    {
        Task<IEnumerable<Account>> QueryAsync(string sql, object param = null);
        Task<Account> QuerySingleOrDefaultAsync(string sql, object param = null);
        Task<int> ExecuteAsync(string sql, object param = null);
    }
}
