Param (
    [string]$ResultPath = "D:\share\eom"
)

$MonthShortNames = @(
    "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"
)

$TemplatesPath = ".\templates" # directory where source templates are stored

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

$TargetDbName = GetDbName -isPrev $false

$Placeholders = @{
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

function PrepareDataSourceFile() {
    $model = $Placeholders["biz_AccountingDetailsWithPercentMargin"]
    $resultFileName = $model["ResultFileName"]
    $content = ReplacePlaceholders -fileName $model["FileName"] -placeholders $model["Placeholders"]
	New-Item -Path "$ResultPath\$resultFileName" -ItemType File -Force
    Add-Content -Path "$ResultPath\$resultFileName" -Value $content
}

Function Run() {
    PrepareDataSourceFile
}

Run