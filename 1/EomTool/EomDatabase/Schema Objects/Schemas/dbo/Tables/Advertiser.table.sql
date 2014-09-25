CREATE TABLE [dbo].[Advertiser] (
    [id]   INT          IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (50) NOT NULL, 
    [status] VARCHAR(50) NULL, 
    [payment_terms] VARCHAR(50) NULL, 
    [invoicing_status] VARCHAR(50) NULL, 
    [comments] VARCHAR(255) NULL, 
    [prev_open_balance] VARCHAR(100) NULL, 
    [qb_name] VARCHAR(100) NULL
);

