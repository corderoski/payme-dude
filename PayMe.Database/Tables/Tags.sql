CREATE TABLE [dbo].[Tags] (
    [Id]        NVARCHAR (250)     NOT NULL,
    [Name]      NVARCHAR (200)     NOT NULL,
    [UserId]    NVARCHAR (128)     NOT NULL,
    [CreatedAt] DATETIMEOFFSET (7) CONSTRAINT [DF_Tags_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt] DATETIMEOFFSET (7) NULL,
    [Version]   ROWVERSION         NOT NULL,
    [Deleted]   BIT                NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC)
);

