using Clean.Architecture.API.Controllers;
using Clean.Architecture.Core.Accounts.Commands.Create;
using Clean.Architecture.Core.Accounts.Commands.Delete;
using Clean.Architecture.Core.Accounts.Commands.Update;
using Clean.Architecture.Core.Accounts.Queries.Get;
using Clean.Architecture.Core.Accounts.Queries.GetAll;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Common.Utility;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class AccountControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AccountsController _controller;

    public AccountControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AccountsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAccounts_WithAccountNumber_ReturnsOk()
    {
        // Arrange
        long accountNumber = 123456789;
        var accountData = new AccountResponse { AccountNumber = accountNumber };
        _mediatorMock.Setup(m => m.Send(It.Is<GetAccountByNumberQuery>(q => q.AccountNumber == accountNumber), default)).ReturnsAsync(accountData);

        // Act
        var result = await _controller.GetAccounts(accountNumber);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(accountData, okResult.Value);
    }

    [Fact]
    public async Task GetAccounts_WithNullAccountNumber_ReturnsOk()
    {
        // Arrange
        var allAccountsData = new[] { new AccountResponse() };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllAccountQuery>(), default)).ReturnsAsync(allAccountsData);

        // Act
        var result = await _controller.GetAccounts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(allAccountsData, okResult.Value);
    }

    [Fact]
    public async Task GetAccounts_WhenAccountNumberDoesNotExist_ReturnsNoContent()
    {
        // Arrange
        long nonExistentAccountNumber = 123456789;
        _mediatorMock.Setup(m => m.Send(It.Is<GetAccountByNumberQuery>(q => q.AccountNumber == nonExistentAccountNumber), default)).ReturnsAsync((AccountResponse)null);

        // Act
        var result = await _controller.GetAccounts(nonExistentAccountNumber);

        // Assert
        Assert.IsType<StatusCodeResult>(result);
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetAccounts_ThrowsException_ReturnsStatus500InternalServerError()
    {
        // Arrange
        long accountNumber = 123456789;
        _mediatorMock.Setup(m => m.Send(It.Is<GetAccountByNumberQuery>(q => q.AccountNumber == accountNumber), default)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetAccounts(123456789);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task CreateAccount_ShouldReturnOk_WhenAccountCreatedSuccessfully()
    {
        // Arrange
        var request = new AccountRequest();
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateAccountCommand>(), default)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateAccount(request);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task CreateAccount_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        var request = new AccountRequest();
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateAccountCommand>(), default)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.CreateAccount(request);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnOk_WhenAccountUpdatedSuccessfully()
    {
        // Arrange
        var request = new AccountRequest();
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateAccountCommand>(), default)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateAccount(request);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnBadRequest_WhenAccountUpdateFails()
    {
        // Arrange
        var request = new AccountRequest();
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateAccountCommand>(), default)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateAccount(request);

        // Assert
        var statusCodeResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode);
        Assert.Equal("Update account failed.", statusCodeResult.Value);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        var request = new AccountRequest();
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateAccountCommand>(), default)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.UpdateAccount(request);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnOk_WhenAccountDeletedSuccessfully()
    {
        // Arrange
        var accountNumber = 123;
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteAccountCommand>(), default)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteAccount(accountNumber);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnNotFound_WhenAccountNotFound()
    {
        // Arrange
        var accountNumber = 123;
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteAccountCommand>(), default)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteAccount(accountNumber);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        var accountNumber = 123;
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteAccountCommand>(), default)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.DeleteAccount(accountNumber);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

    }
}
