CREATE TABLE [dbo].[UserProfileDevices] (
    [UserId]         NVARCHAR (128)     NOT NULL,
    [DeviceUniqueId] NVARCHAR (128)     NOT NULL,
    [Platform]       NVARCHAR (50)      NOT NULL,
    [Version]        NVARCHAR (20)      NULL,
    [Model]          NVARCHAR (50)      NULL,
    [IPLocation]     NVARCHAR (20)      NULL,
    [CreatedAt]      DATETIMEOFFSET (7) CONSTRAINT [DF_UserProfileDevices_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_UserProfileDevices] PRIMARY KEY CLUSTERED ([UserId] ASC, [DeviceUniqueId] ASC)
);

