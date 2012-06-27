CREATE DEFAULT [dbo].[PubReportInstance_saved_DF]
    AS getdate();


GO
EXECUTE sp_bindefault @defname = N'[dbo].[PubReportInstance_saved_DF]', @objname = N'[dbo].[PubReportInstance].[saved]';

