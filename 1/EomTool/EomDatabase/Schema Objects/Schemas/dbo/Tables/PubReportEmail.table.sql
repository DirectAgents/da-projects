CREATE TABLE [dbo].[PubReportEmail] (
    [id]                     INT            IDENTITY (1, 1) NOT NULL,
    [to_email_address]       VARCHAR (255)  NULL,
    [sent_date]              DATETIME       NOT NULL,
    [subject]                VARCHAR (255)  NOT NULL,
    [body]                   NVARCHAR (MAX) NOT NULL,
    [pub_report_instance_id] INT            NOT NULL
);

