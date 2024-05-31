using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Services.Interfaces;

namespace Clean.Architecture.Core.Services.Implementation
{

    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AccountService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task CreateAccountAsync(AccountRequest request)
        {
            Account account = AccountMapper.MapToAccount(request);
            var token = _jwtTokenGenerator.GenerateToken(Guid.NewGuid(), "John", "Doe");
            await _unitOfWork.AccountRepository.AddAsync(account);
        }

        public async Task<bool> DeleteAccountAsync(long accountNumber)
        {
            return await _unitOfWork.AccountRepository.DeleteAsync(accountNumber);
        }

        public async Task<IEnumerable<AccountResponse>> GetAllAccountsAsync()
        {
            var result = await _unitOfWork.AccountRepository.GetAllAsync();
            var response = result.Select(AccountMapper.MapToAccountResponse).ToList();
            return response;
        }

        public async Task<AccountResponse?> GetByAccountNumberAsync(long accountNumber)
        {
            var response = await _unitOfWork.AccountRepository.GetByIdAsync(accountNumber);
            return response != null ? AccountMapper.MapToAccountResponse(response) : null;
        }


        public async Task<bool> UpdateAccountAsync(AccountRequest request)
        {
            Account account = AccountMapper.MapToAccount(request);
            return await _unitOfWork.AccountRepository.UpdateAsync(account);
        }
    }
}
