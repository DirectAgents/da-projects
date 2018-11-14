Param (
    [bool]$CreateDatabase = $true,
    [bool]$CopyData = $true,
    [string]$SqlScriptPath = ".\sql_templates",
    [string]$AuditScriptPath = "G:\GitHub\da-projects2\1\SchemaChanges\AuditScripts", #Full path required
    [string]$ResultFile = ".\bin\eom.create_db.result.sql",
    [string]$DataSource = "biz\sqlexpress",
    [string]$CommonDbName = "DAMain1"
)

$MonthShortNames = @(
    "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"
)

Function GetDbNameByDate($month, $year) {
    return "DADatabase$month$year"
}

Function GetDbName($isPrev) {
    $date = Get-Date
    if ($isPrev) {
        $date = $date.AddMonths(-1)
    }
    $month = $MonthShortNames[$date.Month - 1]
    $year = Get-Date $date -UFormat %Y
    return GetDbNameByDate -month $month -year $year
}

Function GetTimeName() {
    $month = $MonthShortNames[(Get-Date).Month - 1]
    $year = Get-Date -UFormat %Y
    return "$month $year"
}

Function GetViewAction() {
    $month = (Get-Date).Month
    if ($month -eq 1) {
        return "CREATE"
    }
    return "ALTER"
}

Function GenerateSelectQueryForThisYearDbs($viewName, $month, $year) {
    $monthString = $month.ToString("00")
    $datePeriod = "$year-$monthString"
    $nameOfMonth = $MonthShortNames[$month - 1]
    $dbName = GetDbNameByDate -month $nameOfMonth -year $year
    return "SELECT * FROM (select '$datePeriod' AS Period, $dbName.dbo.$viewName.* from $dbName.dbo.$viewName) $dbName UNION ALL`r`n"
}

Function GenerateQueryForThisYearDbs($viewName) {
    $currentMonth = (Get-Date).Month
    $currentYear = (Get-Date).Year
    $queryAll = ""
    $query = ""
    for ($i = $currentMonth; $i -ge 0; $i--) {
        $queryAll = "$queryAll$query"
        $query = GenerateSelectQueryForThisYearDbs -viewName $viewName -month $i -year $currentYear
    }
    return $queryAll.Remove($queryAll.LastIndexOf("UNION ALL"))
}

Function GetMainViewName($viewName) {
    $currYear = Get-Date -Format "yy"
    return $viewName+"Rollup"+$MonthShortNames[0]+$currYear+"To"+$MonthShortNames[11]+$currYear
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
            "COMMON_DATABASE_NAME" = $CommonDbName
            "OLD_DATABASE_NAME" = $SourceDbName
            "NEW_DATABASE_NAME" = $TargetDbName
            "AUDIT_PATH" = $AuditScriptPath
        }
    };
    AddRowToDADatabase = @{
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
            "ACTION" = GetViewAction
            "VIEW_NAME" = GetMainViewName -viewName "CommissionView"
            "SELECT_QUERIES" = GenerateQueryForThisYearDbs -viewName "CommissionView"
        }
    };
    AccountView = @{
        FileName = "UpdateMonthViewRollup.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "ACTION" = GetViewAction
            "VIEW_NAME" = GetMainViewName -viewName "AccountView2"
            "SELECT_QUERIES" = GenerateQueryForThisYearDbs -viewName "AccountingView2"
        }
    };
    AdvertiserPaymentStatus = @{
        FileName = "UpdateMonthViewRollup.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "ACTION" = GetViewAction
            "VIEW_NAME" = GetMainViewName -viewName "AdvertiserPaymentStatus"
            "SELECT_QUERIES" = GenerateQueryForThisYearDbs -viewName "AdvertiserPaymentStatus"
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
    if ($CreateDatabase) {
        AddPreparedScriptToResultFile "CreateDb"
    }
    if ($CopyData) {
        AddPreparedScriptToResultFile "CopyPrevMonth"
        AddPreparedScriptToResultFile "AddRowToDADatabase"
        AddPreparedScriptToResultFile "CommissionView"
        AddPreparedScriptToResultFile "AccountView"
        AddPreparedScriptToResultFile "AdvertiserPaymentStatus"
    }
}

Function Run() {
    New-Item -Path $ResultFile -ItemType File -Force
    PrepareSqlScript
}

Run