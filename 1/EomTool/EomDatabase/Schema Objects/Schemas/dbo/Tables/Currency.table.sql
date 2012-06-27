CREATE TABLE [dbo].[Currency] (
    [id]                INT             IDENTITY (1, 1) NOT NULL,
    [name]              VARCHAR (50)    NOT NULL,
    [to_usd_multiplier] DECIMAL (14, 4) NOT NULL
);
