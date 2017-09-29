CREATE TABLE [dbo].[Transactions] (
    [Id]          NVARCHAR (250)     NOT NULL,
    [Type]        INT                NOT NULL,
    [Amount]      DECIMAL (18)       NOT NULL,
    [Description] NVARCHAR (2000)    NOT NULL,
	[RegisterDate] DATETIMEOFFSET(7) NOT NULL DEFAULT (sysutcdatetime()),
    [ContactId]   NVARCHAR (128)     NOT NULL,
    [UserId]      NVARCHAR (128)     NOT NULL,
    [CreatedAt]   DATETIMEOFFSET (7) CONSTRAINT [DF_Transactions_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]   DATETIMEOFFSET (7) NULL,
    [Version]     ROWVERSION         NOT NULL,
    [Deleted]     BIT                NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transactions_Contacts] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id])
);

