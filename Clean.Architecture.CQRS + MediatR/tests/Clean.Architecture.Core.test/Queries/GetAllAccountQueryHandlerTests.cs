using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Clean.Architecture.Core.Accounts.Queries.GetAll;
using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Interfaces;

public class GetAllAccountQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidQuery_ReturnsAccountResponses()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new Account { AccountNumber = 00000, AccountType = "Savings", CustomerId = 1, BranchAddress = "Branch 1" },
            new Account { AccountNumber = 11111, AccountType = "Checking", CustomerId = 2, BranchAddress = "Branch 2" }
        };
        var expectedResponses = accounts.Select(AccountMapper.MapToAccountResponse).ToList();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts);
        var query = new GetAllAccountQuery();
        var handler = new GetAllAccountQueryHandler(unitOfWorkMock.Object);

        // Act
        var actualResponses = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponses.Count, actualResponses.Count());
        foreach (var expectedResponse in expectedResponses)
        {
            Assert.Contains(actualResponses, actualResponse => 
                actualResponse.CustomerId == expectedResponse.CustomerId &&
                actualResponse.AccountNumber == expectedResponse.AccountNumber &&
                actualResponse.AccountType == expectedResponse.AccountType &&
                actualResponse.BranchAddress == expectedResponse.BranchAddress);
        }
    }

    [Fact]
    public async Task Handle_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var accounts = new List<Account>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts);
        var query = new GetAllAccountQuery();
        var handler = new GetAllAccountQueryHandler(unitOfWorkMock.Object);

        // Act
        var actualResponses = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(actualResponses);
    }

    [Fact]
    public async Task Handle_NullUnitOfWork_ThrowsArgumentNullException()
    {
        // Arrange
        var query = new GetAllAccountQuery();
        var handler = new GetAllAccountQueryHandler(null);

        // Act
        //await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, CancellationToken.None));
        NullReferenceException exception = await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(query, default));

        // Assert
        Assert.Contains("Object reference not set to an instance of an object", exception.Message);
    }
}
