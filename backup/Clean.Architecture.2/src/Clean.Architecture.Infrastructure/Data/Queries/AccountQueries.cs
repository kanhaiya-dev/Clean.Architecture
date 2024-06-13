
namespace Clean.Architecture.Infrastructure.Data.Queries
{
    public static class AccountQueries
    {
        public static string AllAccount => "SELECT [AccountNumber], [CustomerId], [AccountType], [BranchAddress] FROM [Accounts]";

        public static string AccountById => "SELECT [AccountNumber], [CustomerId], [AccountType], [BranchAddress] FROM [Accounts] WHERE [AccountNumber] = @AccountNumber";

        public static string AddAccount =>
            @"INSERT INTO [Accounts] ([CustomerId], [AccountType], [Branchddress], [CreatedAt]) 
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
