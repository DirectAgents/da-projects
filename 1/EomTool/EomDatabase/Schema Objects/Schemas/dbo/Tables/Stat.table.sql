CREATE TABLE [dbo].[Stat] (
    [id]                              INT             IDENTITY (1, 1) NOT NULL,
    [pid]                             INT             NOT NULL,
    [affid]                           INT             NOT NULL,
    [revenue_payout_id]               INT             NOT NULL,
    [cost_payout_id]                  INT             NOT NULL,
    [apiurl]                          VARCHAR (255)   NOT NULL,
    [clicks]                          INT             NOT NULL,
    [leads]                           INT             NOT NULL,
    [num_sales]                       INT             NOT NULL,
    [num_post_impression_sales]       INT             NOT NULL,
    [sale_amount]                     DECIMAL (12, 2) NOT NULL,
    [post_impression_sale_amount]     DECIMAL (12, 2) NOT NULL,
    [num_sub_sales]                   INT             NOT NULL,
    [num_post_impression_sub_sales]   INT             NOT NULL,
    [sub_sale_amount]                 DECIMAL (12, 2) NOT NULL,
    [post_impression_sub_sale_amount] DECIMAL (12, 2) NOT NULL,
    [stat_date]                       DATE            NOT NULL
);

