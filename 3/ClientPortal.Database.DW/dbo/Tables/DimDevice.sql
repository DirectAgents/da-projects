CREATE TABLE [dbo].[DimDevice] (
    [DeviceKey]  INT            NOT NULL,
    [DeviceName] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_DimDevice] PRIMARY KEY CLUSTERED ([DeviceKey] ASC)
);

