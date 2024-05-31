using Clean.Architecture.API.Controllers;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Common.Utility;
using Clean.Architecture.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Clean.Arcitecture.API.test.Controller
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountsController _accountsController;

        public AccountsControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _accountsController = new AccountsController(_accountServiceMock.Object);
        }

        [Fact]
        public async Task GetAccounts_WithAccountNumber_ReturnsOk()
        {
            // Arrange
            long accountNumber = 123456789;
            var accountData = new AccountResponse { AccountNumber = accountNumber };
            _accountServiceMock.Setup(x => x.GetByAccountNumberAsync(accountNumber)).ReturnsAsync(accountData);

            // Act
            var result = await _accountsController.GetAccounts(accountNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(accountData, okResult.Value);
        }

        [Fact]
        public async Task GetAccounts_WithNullAccountNumber_ReturnsOk()
        {
            // Arrange
            var allAccountsData = new[] { new AccountResponse() };
            _accountServiceMock.Setup(x => x.GetAllAccountsAsync()).ReturnsAsync(allAccountsData);

            // Act
            var result = await _accountsController.GetAccounts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(allAccountsData, okResult.Value);
        }

        [Fact]
        public async Task GetAccounts_WhenAccountNumberDoesNotExist_ReturnsNoContent()
        {
            // Arrange
            long nonExistentAccountNumber = 123456789;
            _accountServiceMock.Setup(x => x.GetByAccountNumberAsync(nonExistentAccountNumber)).ReturnsAsync((AccountResponse)null);

            // Act
            var result = await _accountsController.GetAccounts(nonExistentAccountNumber);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetAccounts_ThrowsException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.GetByAccountNumberAsync(It.IsAny<long>())).ThrowsAsync(new Exception());

            // Act
            var result = await _accountsController.GetAccounts(123456789);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }


        [Fact]
        public async Task CreateAccount_ShouldReturnOk_WhenAccountCreatedSuccessfully()
        {
            // Arrange
            var request = new AccountRequest();
            _accountServiceMock.Setup(service => service.CreateAccountAsync(request)).Returns(Task.CompletedTask);

            // Act
            var result = await _accountsController.CreateAccount(request);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateAccount_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var request = new AccountRequest();
            _accountServiceMock.Setup(service => service.CreateAccountAsync(request)).ThrowsAsync(new Exception());

            // Act
            var result = await _accountsController.CreateAccount(request);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnOk_WhenAccountUpdatedSuccessfully()
        {
            // Arrange
            var request = new AccountRequest();
            _accountServiceMock.Setup(service => service.UpdateAccountAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _accountsController.UpdateAccount(request);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnBadRequest_WhenAccountUpdateFails()
        {
            // Arrange
            var request = new AccountRequest();
            _accountServiceMock.Setup(service => service.UpdateAccountAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _accountsController.UpdateAccount(request);

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
            _accountServiceMock.Setup(service => service.UpdateAccountAsync(request)).ThrowsAsync(new Exception());

            // Act
            var result = await _accountsController.UpdateAccount(request);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAccount_ShouldReturnOk_WhenAccountDeletedSuccessfully()
        {
            // Arrange
            var accountNumber = 123;
            _accountServiceMock.Setup(service => service.DeleteAccountAsync(accountNumber)).ReturnsAsync(true);

            // Act
            var result = await _accountsController.DeleteAccount(accountNumber);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAccount_ShouldReturnNotFound_WhenAccountNotFound()
        {
            // Arrange
            var accountNumber = 123;
            _accountServiceMock.Setup(service => service.DeleteAccountAsync(accountNumber)).ReturnsAsync(false);

            // Act
            var result = await _accountsController.DeleteAccount(accountNumber);

            // Assert
            Assert.IsType<BadRequestResult>(result);            
        }

        [Fact]
        public async Task DeleteAccount_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var accountNumber = 123;
            _accountServiceMock.Setup(service => service.DeleteAccountAsync(accountNumber)).ThrowsAsync(new Exception());

            // Act
            var result = await _accountsController.DeleteAccount(accountNumber);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}
