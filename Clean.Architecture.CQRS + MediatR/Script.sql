CREATE TABLE [dbo].[Accounts](
    AccountNumber bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId bigint NOT NULL,
    AccountType nvarchar(max) NOT NULL,
    BranchAddress nvarchar(max) NOT NULL,
    CreatedAt datetime2(7) NULL,
    UpdatedAt datetime2(7) NULL
);

INSERT INTO [dbo].[Accounts] (CustomerId, AccountType, BranchAddress, CreatedAt, UpdatedAt)
VALUES 
(1001, 'Savings', '123 Elm St, Springfield, IL', '2023-01-01 09:00:00', '2023-01-01 09:00:00'),
(1002, 'Checking', '456 Oak St, Springfield, IL', '2023-02-01 10:00:00', '2023-02-01 10:00:00'),
(1003, 'Business', '789 Pine St, Springfield, IL', '2023-03-01 11:00:00', '2023-03-01 11:00:00'),
(1004, 'Savings', '321 Maple St, Springfield, IL', '2023-04-01 12:00:00', '2023-04-01 12:00:00'),
(1005, 'Checking', '654 Birch St, Springfield, IL', '2023-05-01 13:00:00', '2023-05-01 13:00:00');




