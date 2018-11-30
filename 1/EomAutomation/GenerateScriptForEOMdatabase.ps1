Param (
    [bool]$CreateDatabase = $true,
    [bool]$CopyData = $true,
    [string]$ResultPath = ".\results",
    [string]$ResultFile = "eom.create_db.result.sql",
    [string]$CommonDbName = "DAMain1"
)

$CreateDatabaseAnswer = Read-Host 'Do you want to run the part of the script that creates the database? (y/n)'
$CopyDataAnswer = Read-Host 'Do you want to run the part of the script that copies the data? (y/n)'

if ($CreateDatabaseAnswer -eq 'n') {
  $CreateDatabase = $false
} else {
  $CreateDatabase = $true
}
if ($CopyDataAnswer -eq 'n') {
  $CopyData = $false
} else {
  $CopyData = $true
}


$MonthShortNames = @(
    "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"
)

$AuditScriptPath = "G:\GitHub\da-projects2\1\SchemaChanges\AuditScripts" # full path required
$TemplatesPath = ".\templates" # directory where source templates are stored
$StopYear = Get-Date -Year 2017 -Format "yy" # for generation of multi year view

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
    if (IsCurrentMonthToBeginYear) {
        return "CREATE"
    }
    return "ALTER"
}

function IsCurrentMonthToBeginYear() {
    $month = (Get-Date).Month
    if ($month -eq 1) {
        return $true
    }
    else {
        return $false
    }
}

Function GetViewName {
    param (
        [string] $viewName,
        [string] $year = (Get-Date -Format "yy")
      )
    return $viewName+"Rollup"+$MonthShortNames[0]+$year+"To"+$MonthShortNames[11]+$year
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
    for ($month = $currentMonth; $month -ge 1; $month--) {
        $queryAll = "$queryAll$query"
        $query = GenerateSelectQueryForThisYearDbs -viewName $viewName -month $month -year $currentYear
    }
    $queryAll = "$queryAll$query"
    return $queryAll.Remove($queryAll.LastIndexOf("UNION ALL"))
}

Function GenerateQueryForMultiYear($paramViewName) {
    $currentYear = Get-Date -Format "yy"
    $currentYearNum = [int]::Parse($currentYear)
    $stopYearNum = [int]::Parse($StopYear)
    for ($year = $currentYearNum; $year -ge $stopYearNum; $year--) {
        $viewName = GetViewName -viewName $paramViewName -year $year
        $queryAll = "$queryAll$query"
        $query = "SELECT * from [$CommonDbName].[dbo].[$viewName] UNION ALL`r`n"
    }
    $queryAll = "$queryAll$query"
    return $queryAll.Remove($queryAll.LastIndexOf("`r`n"))
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
            "CONNECTION_STRING" = "Data Source=biz\sqlexpress;Initial Catalog=$TargetDbName;Integrated Security=True"
            "EFFECTIVE_DATE" = Get-Date -UFormat "%Y-%m-01"
        }
    };
    CommissionView = @{
        FileName = "UpdateMonthViewRollup.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "ACTION" = GetViewAction
            "VIEW_NAME" = GetViewName -viewName "CommissionView"
            "SELECT_QUERIES" = GenerateQueryForThisYearDbs -viewName "CommissionView"
        }
    };
    AccountView = @{
        FileName = "UpdateMonthViewRollup.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "ACTION" = GetViewAction
            "VIEW_NAME" = GetViewName -viewName "AccountView2"
            "SELECT_QUERIES" = GenerateQueryForThisYearDbs -viewName "AccountingView2"
        }
    };
    AdvertiserPaymentStatus = @{
        FileName = "UpdateMonthViewRollup.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "ACTION" = GetViewAction
            "VIEW_NAME" = GetViewName -viewName "AdvertiserPaymentStatus"
            "SELECT_QUERIES" = GenerateQueryForThisYearDbs -viewName "AdvertiserPaymentStatus"
        }
    };
    AccountView2RollupMultiYear = @{
        FileName = "UpdateViewRollupMultiYear.sql"
        Placeholders = @{
            "COMMON_DATABASE_NAME" = $CommonDbName
            "SELECT_QUERIES" = GenerateQueryForMultiYear -paramViewName "AccountView2"
        }
    };
    biz_AccountingDetailsWithPercentMargin = @{
        FileName = 'biz_sqlexpress_AccountingDetailsWithPercentMargin.odc'
        ResultFileName = "biz_sqlexpress $TargetDbName AccountingDetailsWithPercentMargin.odc"
        Placeholders = @{
            "NEW_DATABASE_NAME" = $TargetDbName
        }
    };
}

Function ReplacePlaceholders($fileName, $placeholders) {
    $filePath = "$TemplatesPath\$($fileName)"
    $content = Get-Content($filePath)
    foreach($placeholderKey in $placeholders.Keys){
        $content = $content.Replace("{%$($placeholderKey)%}", $placeholders[$placeholderKey])
    }
    return $content
}

Function AddPreparedScriptToResultFile($blockName) {
    $model = $Placeholders[$blockName]
    $content = ReplacePlaceholders -fileName $model["FileName"] -placeholders $model["Placeholders"]
    Add-Content -Path "$ResultPath\$ResultFile" -Value $content
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
        if (IsCurrentMonthToBeginYear) {
            AddPreparedScriptToResultFile "AccountView2RollupMultiYear"
        }
    }
}

function PrepareDataSourceFile() {
    $model = $Placeholders["biz_AccountingDetailsWithPercentMargin"]
    $resultFileName = $model["ResultFileName"]
    $content = ReplacePlaceholders -fileName $model["FileName"] -placeholders $model["Placeholders"]
    Add-Content -Path "$ResultPath\$resultFileName" -Value $content
}

Function Run() {
    New-Item -Path "$ResultPath\$ResultFile" -ItemType File -Force
    PrepareSqlScript
    PrepareDataSourceFile
}

Run