CREATE DEFAULT [dbo].[PubReportInstance_email_status_msg_DF]
    AS 'unsent';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[PubReportInstance_email_status_msg_DF]', @objname = N'[dbo].[PubReportInstance].[email_status_msg]';

