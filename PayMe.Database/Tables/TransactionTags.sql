CREATE TABLE [dbo].[TransactionTags] (
	[Id]          NVARCHAR (250)     NOT NULL,
    [TagId]         NVARCHAR (250) NOT NULL,
    [TransactionId] NVARCHAR (250) NOT NULL,
	[CreatedAt]   DATETIMEOFFSET (7) CONSTRAINT [DF_TransactionTags_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]   DATETIMEOFFSET (7) NULL,
    [Version]     ROWVERSION         NOT NULL,
    [Deleted]     BIT                NOT NULL,
	CONSTRAINT [PK_TransactionTags] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TransactionTags_Tags] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id]),
    CONSTRAINT [FK_TransactionTags_Transactions] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transactions] ([Id])
);

