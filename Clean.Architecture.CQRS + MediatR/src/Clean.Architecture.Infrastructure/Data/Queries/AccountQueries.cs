
namespace Clean.Architecture.Infrastructure.Data.Queries
{
    public static class AccountQueries
    {
        public static string AllAccount => "SELECT * FROM [Accounts]";

        public static string AccountById => "SELECT * FROM [Accounts] WHERE [AccountNumber] = @AccountNumber";

        public static string AddAccount =>
            @"INSERT INTO [Accounts] ([CustomerId], [AccountType], [BranchAddress], [CreatedAt]) 
            VALUES (@CustomerId, @AccountType, @BranchAddress, @CreatedAt)";

        public static string UpdateAccount =>
            @"UPDATE [Accounts] 
        SET [CustomerId] = @CustomerId, 
            [AccountType] = @AccountType, 
            [BranchAddress] = @BranchAddress, 
            [UpdatedAt] = @UpdatedAt
        WHERE [AccountNumber] = @AccountNumber";

        public static string DeleteAccount => "DELETE FROM [Accounts] WHERE [AccountNumber] = @AccountNumber";
    }
}
