using Xunit;
using Moq;
using Clean.Architecture.Core.Accounts.Commands.Create;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Response;
using System;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Entities.Buisness;

public class CreateAccountCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_AddsAccountAndGeneratesToken()
    {
        // Arrange
        AccountRequest accountRequest = new AccountRequest { AccountNumber = 1111, AccountType = "Savings", BranchAddress = "Address", CustomerId = 1 };
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var account = new Account();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        unitOfWorkMock.Setup(u => u.AccountRepository.AddAsync(account));
        var handler = new CreateAccountCommandHandler(unitOfWorkMock.Object, jwtTokenGeneratorMock.Object);
        var command = new CreateAccountCommand { AccountRequest = accountRequest };

        // Act
        var response = handler.Handle(command, CancellationToken.None);

        // Assert
        unitOfWorkMock.Verify(uow => uow.AccountRepository.AddAsync(It.IsAny<Account>()), Times.Once);
        jwtTokenGeneratorMock.Verify(jwt => jwt.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        Assert.NotNull(response);
        Assert.True(response.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task Handle_NullCommand_ThrowsArgumentNullException()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        var handler = new CreateAccountCommandHandler(unitOfWorkMock.Object, jwtTokenGeneratorMock.Object);

        // Act & Assert
        NullReferenceException exception = await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, CancellationToken.None));

        // Assert
        Assert.Contains("Object reference not set to an instance of an object", exception.Message);
    }

}
