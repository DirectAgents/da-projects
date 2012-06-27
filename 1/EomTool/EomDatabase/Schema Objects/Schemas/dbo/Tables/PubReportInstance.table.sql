CREATE TABLE [dbo].[PubReportInstance] (
    [id]                   INT            IDENTITY (1, 1) NOT NULL,
    [created_by_user_name] VARCHAR (255)  NOT NULL,
    [report_content]       NVARCHAR (MAX) NOT NULL,
    [path_to_hard_copy]    VARCHAR (255)  NOT NULL,
    [vendor_id]            INT            NOT NULL,
    [saved]                DATETIME       NOT NULL,
    [email_status_msg]     VARCHAR (255)  NOT NULL
);

