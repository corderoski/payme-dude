CREATE TABLE [dbo].[UserProfileAuthorizations] (
    [UserId]         NVARCHAR (128)     NOT NULL,
    [Provider]       NVARCHAR (20)      NOT NULL,
    [ProviderUserId] NVARCHAR (128)     NOT NULL,
    [CreatedAt]      DATETIMEOFFSET (7) CONSTRAINT [DF_UserProfileAuthorizations_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_UserProfileAuthorizations] PRIMARY KEY CLUSTERED ([UserId] ASC, [ProviderUserId] ASC)
);

