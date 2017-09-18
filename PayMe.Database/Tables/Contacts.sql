CREATE TABLE [dbo].[Contacts] (
    [Id]                    NVARCHAR (128)     NOT NULL,
    [Name]                  NVARCHAR (50)      NOT NULL,
    [DeviceUniqueContactId] NVARCHAR (250)     NULL,
    [Notes]                 NVARCHAR (2000)    NOT NULL,
    [UserId]                NVARCHAR (128)     NOT NULL,
    [CreatedAt]             DATETIMEOFFSET (7) CONSTRAINT [DF_Contacts_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]             DATETIMEOFFSET (7) NULL,
    [Version]               ROWVERSION         NOT NULL,
    [Deleted]               BIT                NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

