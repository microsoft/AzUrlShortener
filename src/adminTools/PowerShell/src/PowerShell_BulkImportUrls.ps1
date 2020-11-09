#Example csv => BulkImportUrls.csv
#as vanity is not mandatory just don't add a value for the vanity column like in the following Microsoft2 example
#title,url,vanity
#"Microsoft1","https://www.microsoft.com","msft1"
#"Microsoft2","https://www.microsoft.com"

$UrlValuesFromCSV = Import-CSV .\BulkImportUrls.csv
<<<<<<< HEAD
$AzureFunctionUrlShortenerUrl = "https://shortenertools....azurewebsites.net/api/UrlShortener?code=..."
=======
#$AzureFunctionUrlShortenerUrl = "https://shortenertools....azurewebsites.net/api/UrlShortener?code=..."
>>>>>>> PowerShell_BulkImport_CSV

ForEach ($csventry in $UrlValuesFromCSV)
{
    if (!$($csventry.vanity)) { $vanity = '' } else {$vanity = $($csventry.vanity)}
    $Body = @{
      title = $($csventry.title)
      url = $($csventry.url)
      vanity = $vanity      
    }

    $Body

    $Parameters = @{
        Method = "POST" 
        Uri =  $AzureFunctionUrlShortenerUrl
        Body = ($Body | ConvertTo-Json)
        ContentType = "application/json"
    }

    $newUrl = Invoke-RestMethod @Parameters | ConvertTo-Json
    $newUrl
}

