CREATE TABLE [dbo].[Payout] (
    [id]               INT          IDENTITY (1, 1) NOT NULL,
    [currency_id]      INT          NOT NULL,
    [payout_id]        VARCHAR (50) NOT NULL,
    [payout_type]      VARCHAR (20) NOT NULL,
    [pid]              INT          NOT NULL,
    [affid]            INT          NOT NULL,
    [impression]       MONEY        NULL,
    [click]            MONEY        NOT NULL,
    [lead]             MONEY        NOT NULL,
    [percent_sale]     MONEY        NOT NULL,
    [flat_sale]        MONEY        NOT NULL,
    [percent_sub_sale] MONEY        NOT NULL,
    [flat_sub_sale]    MONEY        NOT NULL,
    [effective_date]   DATE         NOT NULL,
    [modify_date]      DATE         NOT NULL,
    [product_id]       INT          NOT NULL
);

