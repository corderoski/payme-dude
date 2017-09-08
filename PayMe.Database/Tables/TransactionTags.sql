CREATE TABLE [dbo].[TransactionTags] (
    [TagId]         NVARCHAR (250) NOT NULL,
    [TransactionId] NVARCHAR (250) NOT NULL,
    CONSTRAINT [FK_TransactionTags_Tags] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id]),
    CONSTRAINT [FK_TransactionTags_Transactions] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transactions] ([Id])
);

