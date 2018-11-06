Param (
    [bool]$CopyData = $true,
    [string]$SqlScriptPath = ".\sql_templates",
    [string]$AuditScriptPath = "G:\GitHub\da-projects2\1\SchemaChanges\AuditScripts", #Full path required
    [string]$ResultFile = ".\bin\eom.create_db.result.sql",
    [string]$DataSource = "biz\sqlexpress"
    [string]$CommonDbName = "DAMain1"
)

$MonthShortNames = @(
    "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"
)

Function GetDbName($month, $year) {
    return "DADatabase$month$year"
}

Function GetDbName($isPrev) {
    $date = Get-Date
    if ($isPrev) {
        $date = $date.AddMonths(-1)
    }
    $month = $MonthShortNames[$date.Month - 1]
    $year = Get-Date $date -UFormat %Y
    return GetDbName -month $month -year $year
}

Function GetTimeName() {
    $month = $MonthShortNames[(Get-Date).Month - 1]
    $year = Get-Date -UFormat %Y
    return "$month $year"
}

Function GetComissionViewAction() {
    $month = (Get-Date).Month
    if ($month -eq 1) {
        return "CREATE"
    }
    return "ALTER"
}

Function GenerateSelectQueryForThisYearDbs($viewName, $month, $year) {
    $datePeriod = "$year-$month"
    $dbName = GetDbName -month $month -year $year
    return "SELECT * FROM (select '$datePeriod' AS Period, $dbName.dbo.$viewName.* from $dbName.dbo.$viewName) $dbName"
}

Function GenerateSelectQueryForThisYearDbs($viewName) {
    $currentMonth = (Get-Date).Month
    $currentYear = (Get-Date).Year
    # $query = ""
    # for ($i in $currentMonth - 1) {

    # }
}

$SourceDbName = GetDbName -isPrev $true
$TargetDbName = GetDbName -isPrev $false

$Placeholders = @{
    CreateDb = @{
        FileName = "EomDatabase.publish.sql"
        Placeholders = @{
            "DATABASE_NAME" = $TargetDbName
        }
    };
    CopyPrevMonth = @{
        FileName = "NewMonth_copyover.sql"
        Placeholders = @{
            "OLD_DATABASE_NAME" = $SourceDbName
            "NEW_DATABASE_NAME" = $TargetDbName
            "AUDIT_PATH" = $AuditScriptPath
        }
    };
    AddRowToDADatabse = @{
        FileName = "DADatabase_NewRow.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "NAME" = GetTimeName
            "CONNECTION_STRING" = "Data Source=$DataSource;Initial Catalog=$TargetDbName;Integrated Security=True"
            "EFFECTIVE_DATE" = Get-Date -UFormat "%Y-%m-01"
        }
    };
    CommissionView = @{
        FileName = "UpdateMonthViewRollup.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "ACTION" = GetComissionViewAction
            # "SELECT_QUERIES" = GenerateSelectQueryForThisYearDbs
        }
    }
}

Function ReplacePlaceholders($fileName, $placeholders) {
    $filePath = "$SqlScriptPath\$($fileName)"
    $content = Get-Content($filePath)
    foreach($placeholderKey in $placeholders.Keys){
        $content = $content.Replace("{%$($placeholderKey)%}", $placeholders[$placeholderKey])
    }
    return $content
}

Function AddPreparedScriptToResultFile($blockName) {
    $model = $Placeholders[$blockName]
    $content = ReplacePlaceholders -fileName $model["FileName"] -placeholders $model["Placeholders"]
    Add-Content -Path $ResultFile -Value $content
}
Function PrepareSqlScript() {
    AddPreparedScriptToResultFile "CreateDb"
    if (!$CopyData) {
        return
    }
    AddPreparedScriptToResultFile "CopyPrevMonth"
    AddPreparedScriptToResultFile "AddRowToDADatabse"
    AddPreparedScriptToResultFile "CommissionView"
}

Function Run() {
    New-Item -Path $ResultFile -ItemType File -Force
    PrepareSqlScript
}

Run