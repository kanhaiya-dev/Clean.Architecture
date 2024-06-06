using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure.Data.Queries;
using Clean.Architecture.Infrastructure.Wrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AccountRepository : IAccountRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly IDbConnectionWrapper _connectionWrapper;
    private readonly ILogger<AccountRepository> _logger;

    public AccountRepository(IConfiguration configuration, IDbConnectionWrapper connectionWrapper, ILogger<AccountRepository> logger)
    {
        _configuration = configuration;
        _connectionWrapper = connectionWrapper;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Account?>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Getting all accounts...");
            var result = await _connectionWrapper.QueryAsync(AccountQueries.AllAccount);
            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all accounts.");
            return null;
        }
    }

    public async Task<Account?> GetByIdAsync(long id)
    {
        try
        {
            _logger.LogInformation("Getting account by ID: {AccountId}", id);
            var result = await _connectionWrapper.QuerySingleOrDefaultAsync(AccountQueries.AccountById, new { AccountNumber = id });
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting account by ID: {AccountId}", id);
            return null;
        }
    }

    public async Task AddAsync(Account entity)
    {
        try
        {
            _logger.LogInformation("Adding new account...");
            entity.CreatedAt = DateTime.Now;
            await _connectionWrapper.ExecuteAsync(AccountQueries.AddAccount, entity);
            _logger.LogInformation("Account added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new account.");
            return;
        }
    }

    public async Task<bool> UpdateAsync(Account entity)
    {
        try
        {
            _logger.LogInformation("Updating account...");
            entity.UpdatedAt = DateTime.Now;
            var result = await _connectionWrapper.ExecuteAsync(AccountQueries.UpdateAccount, entity);
            _logger.LogInformation("Account updated successfully.");
            return result == 1 ? true : false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating an account.");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            _logger.LogInformation("Deleting account with ID: {AccountId}", id);
            var result = await _connectionWrapper.ExecuteAsync(AccountQueries.DeleteAccount, new { AccountNumber = id });
            _logger.LogInformation("Account deleted successfully.");
            return result == 1 ? true : false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting an account with ID: {AccountId}", id);
            return false;
        }
    }
}
