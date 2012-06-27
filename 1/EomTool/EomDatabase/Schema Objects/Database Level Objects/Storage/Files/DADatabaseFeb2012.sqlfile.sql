ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [DADatabaseFeb2012], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

