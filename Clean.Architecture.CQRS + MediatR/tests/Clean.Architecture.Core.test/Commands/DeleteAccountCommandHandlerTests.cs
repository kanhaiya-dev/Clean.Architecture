using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Clean.Architecture.Core.Accounts.Commands.Delete;
using Clean.Architecture.Core.Interfaces;

public class DeleteAccountCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_DeletesAccount()
    {
        // Arrange
        long accountNumber = 123;
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.AccountRepository.DeleteAsync(It.IsAny<long>())).Returns(Task.FromResult(true));
        var command = new DeleteAccountCommand { AccountNumber = accountNumber };
        var handler = new DeleteAccountCommandHandler(unitOfWorkMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        unitOfWorkMock.Verify(u => u.AccountRepository.DeleteAsync(accountNumber), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ThrowsException()
    {
        // Arrange
        long accountNumber = 123;
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.AccountRepository.DeleteAsync(It.IsAny<long>())).ThrowsAsync(new Exception("Account not found."));
        var command = new DeleteAccountCommand { AccountNumber = accountNumber };
        var handler = new DeleteAccountCommandHandler(unitOfWorkMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}
