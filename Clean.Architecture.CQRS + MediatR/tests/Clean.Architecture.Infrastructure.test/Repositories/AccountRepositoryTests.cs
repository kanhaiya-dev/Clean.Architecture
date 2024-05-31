using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Infrastructure.Wrapper;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Clean.Architecture.Tests.Repositories
{
    public class AccountRepositoryTests
    {
        private readonly Mock<IDbConnectionWrapper> _dbConnectionWrapperMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AccountRepository _accountRepository;

        public AccountRepositoryTests()
        {
            _dbConnectionWrapperMock = new Mock<IDbConnectionWrapper>();
            _configurationMock = new Mock<IConfiguration>();

            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("YourConnectionStringHere");
            _configurationMock.Setup(x => x.GetSection(It.Is<string>(s => s == "ConnectionStrings:defaultConnection")))
                .Returns(mockSection.Object);

            _accountRepository = new AccountRepository(_configurationMock.Object, _dbConnectionWrapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_Returns_AllAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { CustomerId = 1, AccountNumber = 123, AccountType = "Savings", BranchAddress = "Branch A" },
                new Account { CustomerId = 2, AccountNumber = 456, AccountType = "Checking", BranchAddress = "Branch B" }
            };

            _dbConnectionWrapperMock
                .Setup(db => db.QueryAsync(It.IsAny<string>(), null))
                .ReturnsAsync(accounts);

            // Act
            var result = await _accountRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Savings", result[0].AccountType);
            Assert.Equal("Branch A", result[0].BranchAddress);
            Assert.Equal("Checking", result[1].AccountType);
            Assert.Equal("Branch B", result[1].BranchAddress);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_CorrectAccount()
        {
            // Arrange
            var account = new Account
            {
                CustomerId = 1,
                AccountNumber = 123,
                AccountType = "Savings",
                BranchAddress = "Branch A"
            };

            _dbConnectionWrapperMock
                .Setup(db => db.QuerySingleOrDefaultAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(account);

            // Act
            var result = await _accountRepository.GetByIdAsync(123);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Savings", result.AccountType);
            Assert.Equal("Branch A", result.BranchAddress);
        }

        [Fact]
        public async Task AddAsync_Adds_Account()
        {
            // Arrange
            var account = new Account
            {
                CustomerId = 1,
                AccountNumber = 123,
                AccountType = "Savings",
                BranchAddress = "Branch A"
            };

            _dbConnectionWrapperMock
                .Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(1);

            // Act
            await _accountRepository.AddAsync(account);

            // Assert
            _dbConnectionWrapperMock.Verify(db => db.ExecuteAsync(It.IsAny<string>(), account), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Updates_Account()
        {
            // Arrange
            var account = new Account
            {
                CustomerId = 1,
                AccountNumber = 123,
                AccountType = "Savings",
                BranchAddress = "Branch A",
                UpdatedAt = DateTime.Now
            };

            _dbConnectionWrapperMock
                .Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(1);

            // Act
            var result = await _accountRepository.UpdateAsync(account);

            // Assert
            _dbConnectionWrapperMock.Verify(db => db.ExecuteAsync(It.IsAny<string>(), account), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_Deletes_Account()
        {
            // Arrange
            _dbConnectionWrapperMock
                .Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(1);

            // Act
            var result = await _accountRepository.DeleteAsync(123);

            // Assert
            _dbConnectionWrapperMock.Verify(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            Assert.True(result);
        }
    }
}
