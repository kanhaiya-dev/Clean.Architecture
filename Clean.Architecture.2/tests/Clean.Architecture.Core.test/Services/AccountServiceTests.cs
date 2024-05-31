using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Services.Implementation;
using Clean.Architecture.Core.Services.Interfaces;
using Moq;
using System.Security.Principal;

namespace Clean.Architecture.Core.test.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            _accountService = new AccountService(_mockUnitOfWork.Object, _mockJwtTokenGenerator.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsAccountResponses()
        {
            // Arrange
            var accounts = new List<Account>
        {
            new Account {AccountNumber = 111, CustomerId = 1, AccountType = "Type", BranchAddress = "Address"},
            new Account {AccountNumber = 111, CustomerId = 1, AccountType = "Type", BranchAddress = "Address" }
        };

            _mockUnitOfWork.Setup(x => x.AccountRepository.GetAllAsync())
                .ReturnsAsync(accounts);

            // Act
            var result = await _accountService.GetAllAccountsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<AccountResponse>>(result);
            Assert.Equal(accounts.Count, result.Count());
        }

        [Fact]
        public async Task GetByAccountNumberAsync_ReturnsAccountResponse_WhenAccountExists()
        {
            // Arrange
            var expectedAccount = new Account { AccountNumber = 1, CustomerId = 1, AccountType = "Savings", BranchAddress = "Branch 1" };
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetByIdAsync(1)).ReturnsAsync(expectedAccount);

            // Act
            var result = await _accountService.GetByAccountNumberAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AccountResponse>(result);
            Assert.Equal(expectedAccount.AccountNumber, result.AccountNumber);
            Assert.Equal(expectedAccount.CustomerId, result.CustomerId);
            Assert.Equal(expectedAccount.AccountType, result.AccountType);
            Assert.Equal(expectedAccount.BranchAddress, result.BranchAddress);
        }

        [Fact]
        public async Task GetByAccountNumberAsync_ReturnsNull_WhenAccountDoesNotExists()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetByIdAsync(1)).ReturnsAsync((Account)null);

            // Act
            var result = await _accountService.GetByAccountNumberAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAccountAsync_CreatesAccount_WhenInputObjectIsCorrect()
        {
            // Arrange
            var account = new Account();
            AccountRequest accountRequest = new AccountRequest { AccountNumber = 1111, AccountType = "Savings", BranchAddress = "Address", CustomerId = 1 };
            _mockUnitOfWork.Setup(u => u.AccountRepository.AddAsync(account));
            _mockJwtTokenGenerator.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("token");

            // Act
            await _accountService.CreateAccountAsync(accountRequest);

            // Assert
            _mockUnitOfWork.Verify(x => x.AccountRepository.AddAsync(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountAsync_ReturnTrue_WhenUpdatedSuccessfully()
        {
            // Arrange
            var request = new AccountRequest();
            var account = new Account();
            _mockUnitOfWork.Setup(u => u.AccountRepository.UpdateAsync(account)).ReturnsAsync(true);

            // Act
            var result = await _accountService.UpdateAccountAsync(request);

            // Assert
            _mockUnitOfWork.Verify(x => x.AccountRepository.UpdateAsync(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountAsync_ReturnsTrue_WhenDeletedSuccessfully()
        {
            // Arrange
            long accountNumber = 1;
            _mockUnitOfWork.Setup(u => u.AccountRepository.DeleteAsync(accountNumber)).ReturnsAsync(true);

            // Act
            var result = await _accountService.DeleteAccountAsync(accountNumber);

            // Assert
            _mockUnitOfWork.Verify(x => x.AccountRepository.DeleteAsync(accountNumber), Times.Once);
            Assert.True(result);
        }
    }
}
