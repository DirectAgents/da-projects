da-projects
===========
This repository contains 5 main solutions:
* EOM Tool desktop application - [/1/Solution/EomApp1.sln](https://github.com/DirectAgents/da-projects/tree/master/1/Solution)
* EOM Tool web application - [/1/Solution/EomToolWeb.sln](https://github.com/DirectAgents/da-projects/tree/master/1/Solution)
* Application to manage roles\permissions for EOM tool users - [/1/EomTool/EomToolSecurityWeb/EomToolSecurity.sln](https://github.com/DirectAgents/da-projects/tree/master/1/EomTool/EomToolSecurityWeb)
* Application to extract stats from different advertising platforms - [/3/DirectAgents.Web/DirectAgents.Web/DirectAgents.Web.sln](https://github.com/DirectAgents/da-projects/tree/master/3/DirectAgents.Web/DirectAgents.Web)
* Admin Portal to manage advertisers, profiles, accounts and etc. - [/3/DirectAgents.Web/DirectAgents.Web/DirectAgents.Web.sln](https://github.com/DirectAgents/da-projects/tree/master/3/DirectAgents.Web/DirectAgents.Web)

For EOM projects all required dependencies are located at [\1\Solution\packages](https://github.com/DirectAgents/da-projects/tree/master/1/Solution/packages)

For the extractor and admin portal dependencies are managed via Nuget package manager.

Additional helpful projects/folders:
* Project DirectAgents.Domain ([/1/DirectAgents.Domain/DirectAgents.Domain.csproj](https://github.com/DirectAgents/da-projects/tree/master/1/DirectAgents.Domain)) - Project provides funtionaity to work with database using Entity Framework and CodeFirst approach.
* Folder API ([/3/API/](https://github.com/DirectAgents/da-projects/tree/master/3/API)) - Contains multiple projects which provide API integrations with different platforms.
* Folder EomAutomation ([/1/EomAutomation/](https://github.com/DirectAgents/da-projects/tree/master/1/EomAutomation)) - Contains PowerShell scripts to create monthly databases for EOM Tool.
* Folder SQL ([/3/CakeExtracter.ConsoleApplication/SQL/](https://github.com/DirectAgents/da-projects/tree/master/3/CakeExtracter.ConsoleApplication/SQL)) - Contains SQL scripts for automating purposes(Synch/Create/Select different tables).