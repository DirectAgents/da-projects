CREATE TABLE [dbo].[DailySummary] (
    [offer_id]        INT             NOT NULL,
    [date]            DATETIME        NOT NULL,
    [views]           INT             NOT NULL,
    [clicks]          INT             NOT NULL,
    [click_thru]      DECIMAL (18, 2) NOT NULL,
    [conversions]     INT             NOT NULL,
    [paid]            INT             NOT NULL,
    [sellable]        INT             NOT NULL,
    [conversion_rate] DECIMAL (18, 2) NOT NULL,
    [cpl]             DECIMAL (18, 2) NOT NULL,
    [cost]            DECIMAL (18, 2) NOT NULL,
    [rpt]             DECIMAL (18, 2) NOT NULL,
    [revenue]         DECIMAL (18, 2) NOT NULL,
    [margin]          DECIMAL (18, 2) NOT NULL,
    [profit]          DECIMAL (18, 2) NOT NULL,
    [epc]             DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_dbo.DailySummary] PRIMARY KEY CLUSTERED ([offer_id] ASC, [date] ASC)
);

