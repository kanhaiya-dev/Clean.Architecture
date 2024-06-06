using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, ILogger<AccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task CreateAccountAsync(AccountRequest request)
        {
            try
            {
                _logger.LogInformation("Creating account...");
                Account account = AccountMapper.MapToAccount(request);
                var token = _jwtTokenGenerator.GenerateToken(Guid.NewGuid(), "John", "Doe");
                await _unitOfWork.AccountRepository.AddAsync(account);
                _logger.LogInformation("Account created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an account.");
                return;
            }
        }

        public async Task<bool> DeleteAccountAsync(long accountNumber)
        {
            try
            {
                _logger.LogInformation("Deleting account with account number: {AccountNumber}", accountNumber);
                return await _unitOfWork.AccountRepository.DeleteAsync(accountNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an account.");
                return false;
            }
        }

        public async Task<IEnumerable<AccountResponse?>?> GetAllAccountsAsync()
        {
            try
            {
                _logger.LogInformation("Getting all accounts...");
                var result = await _unitOfWork.AccountRepository.GetAllAsync();
                var response = result.Select(AccountMapper.MapToAccountResponse).ToList();
                _logger.LogInformation("Retrieved {Count} accounts.", response.Count);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all accounts.");
                return null;
            }
        }

        public async Task<AccountResponse?> GetByAccountNumberAsync(long accountNumber)
        {
            try
            {
                _logger.LogInformation("Getting account by account number: {AccountNumber}", accountNumber);
                var response = await _unitOfWork.AccountRepository.GetByIdAsync(accountNumber);
                if (response == null)
                {
                    _logger.LogInformation("No account found with account number: {AccountNumber}", accountNumber);
                }
                return response != null ? AccountMapper.MapToAccountResponse(response) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting account by account number.");
                return null;
            }
        }

        public async Task<bool> UpdateAccountAsync(AccountRequest request)
        {
            try
            {
                _logger.LogInformation("Updating account...");
                Account account = AccountMapper.MapToAccount(request);
                return await _unitOfWork.AccountRepository.UpdateAsync(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating an account.");
                return false;
            }
        }
    }
}
