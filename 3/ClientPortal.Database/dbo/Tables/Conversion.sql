CREATE TABLE [dbo].[Conversion] (
    [conversion_id]        NVARCHAR (128)  NOT NULL,
    [conversion_date]      DATETIME        NOT NULL,
    [affiliate_id]         INT             NOT NULL,
    [advertiser_id]        INT             NOT NULL,
    [offer_id]             INT             NOT NULL,
    [received_currency_id] TINYINT         NOT NULL,
    [received_amount]      DECIMAL (18, 2) NOT NULL,
    [transaction_id]       NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Conversion] PRIMARY KEY CLUSTERED ([conversion_id] ASC)
);



