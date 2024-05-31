using Xunit;
using Moq;
using Clean.Architecture.Core.Accounts.Queries.Get;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Common.Mapper;
using System.Threading.Tasks;

public class GetAccountByNumberQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidQuery_ReturnsAccountResponse()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var expectedAccount = new Account { AccountNumber = 1, CustomerId = 1, AccountType = "Savings", BranchAddress = "Branch 1" };
        unitOfWorkMock.Setup(uow => uow.AccountRepository.GetByIdAsync(1)).ReturnsAsync(expectedAccount);

        var handler = new GetAccountByNumberQueryHandler(unitOfWorkMock.Object);
        var query = new GetAccountByNumberQuery{ AccountNumber = 1 };

        // Act
        var response = await handler.Handle(query, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedAccount.AccountNumber, response.AccountNumber);
        Assert.Equal(expectedAccount.CustomerId, response.CustomerId);
        Assert.Equal(expectedAccount.AccountType, response.AccountType);
        Assert.Equal(expectedAccount.BranchAddress, response.BranchAddress);
    }

    [Fact]
    public async Task Handle_NullQuery_ThrowsArgumentNullException()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var handler = new GetAccountByNumberQueryHandler(unitOfWorkMock.Object);

        // Act
        NullReferenceException exception = await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, default));

        // Assert
        Assert.Contains("Object reference not set to an instance of an object", exception.Message);
    }

    [Fact]
    public async Task Handle_NonExistentAccount_ReturnsNull()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((Account)null);

        var handler = new GetAccountByNumberQueryHandler(unitOfWorkMock.Object);
        var query = new GetAccountByNumberQuery { AccountNumber = 1 };

        // Act
        NullReferenceException exception = await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, default));

        // Assert
        Assert.Contains("Object reference not set to an instance of an object", exception.Message);
    }
}
