CREATE TABLE [dbo].[Users] (
    [Id]           NVARCHAR (128)     NOT NULL,
    [UserName]     NVARCHAR (250)     NOT NULL,
    [Email]        NVARCHAR (250)     NOT NULL,
    [Password]     NVARCHAR (MAX)     NOT NULL,
    [RegisterDate] DATETIMEOFFSET (7) NOT NULL,
    [CreatedAt]    DATETIMEOFFSET (7) CONSTRAINT [DF_Users_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]    DATETIMEOFFSET (7) NULL,
    [Version]      ROWVERSION         NOT NULL,
    [Deleted]      BIT                NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

