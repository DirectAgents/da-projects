CREATE TABLE [dbo].[Affiliate] (
    [id]                INT           IDENTITY (1, 1) NOT NULL,
    [name]              VARCHAR (255) NOT NULL,
    [media_buyer_id]    INT           NOT NULL,
    [affid]             INT           NOT NULL,
    [currency_id]       INT           NOT NULL,
    [email]             VARCHAR (100) NOT NULL,
    [add_code]          VARCHAR (100) NOT NULL,
    [net_term_type_id]  INT           NULL,
    [payment_method_id] INT           NOT NULL,
    [name2]             AS            ((([name]+' (')+[add_code])+')'),
    [date_created]      DATETIME      NULL,
    [date_modified]     DATETIME      NULL
);
