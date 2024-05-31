using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Entities.Buisness;

public class AccountMapperTests
{
    [Fact]
    public void MapToAccount_ShouldMapCorrectly()
    {
        // Arrange
        var request = new AccountRequest
        {
            CustomerId = 1,
            AccountType = "Savings",
            AccountNumber = 123456789,
            BranchAddress = "123 Main St"
        };

        // Act
        var result = AccountMapper.MapToAccount(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.CustomerId, result.CustomerId);
        Assert.Equal(request.AccountType, result.AccountType);
        Assert.Equal(request.AccountNumber, result.AccountNumber);
        Assert.Equal(request.BranchAddress, result.BranchAddress);
    }

    [Fact]
    public void MapToAccountResponse_ShouldMapCorrectly()
    {
        // Arrange
        var account = new Account
        {
            CustomerId = 1,
            AccountType = "Savings",
            AccountNumber = 123456789,
            BranchAddress = "123 Main St"
        };

        // Act
        var result = AccountMapper.MapToAccountResponse(account);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(account.CustomerId, result.CustomerId);
        Assert.Equal(account.AccountType, result.AccountType);
        Assert.Equal(account.AccountNumber, result.AccountNumber);
        Assert.Equal(account.BranchAddress, result.BranchAddress);
    }
}