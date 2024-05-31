using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;

namespace Clean.Architecture.Core.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();
        Task<AccountResponse> GetByAccountNumberAsync(long accountNumber);
        Task CreateAccountAsync(AccountRequest request);
        Task<bool> UpdateAccountAsync(AccountRequest request);
        Task<bool> DeleteAccountAsync(long accountNumber);

    }
}
