#Calling a REST API from PowerShell
#Idea from: https://4bes.nl/2020/08/23/calling-a-rest-api-from-powershell/amp/

#GET all URls
$Parameters = @{
    Method = "GET" 
    Uri =  "https://shortenertoolsn....azurewebsites.net/api/UrlList?code=..."
    ContentType = "application/json"
}

$allUrls = Invoke-RestMethod @Parameters | ConvertTo-Json
$allUrls


###############################################################


#CREATE new ShortURL
$Body = @{
  title = "Microsoft"
  url = "https://www.microsoft.com"
  vanity = "msft"
}

$Parameters = @{
    Method = "POST" 
    Uri =  "https://shortenertools....azurewebsites.net/api/UrlShortener?code=..."
    Body = ($Body | ConvertTo-Json)
    ContentType = "application/json"
}

$newUrl = Invoke-RestMethod @Parameters | ConvertTo-Json
$newUrl


###############################################################


#UPDATE ShortURL
$Body = @{
  title = "Microsoft Homepage"
  url = "https://www.microsoft.com"
  RowKey = "msft"
  PartitionKey = "m"
  vanity = "msft"
}

$Parameters = @{
    Method = "POST" 
    Uri =  "https://shortenertools....azurewebsites.net/api/UrlUpdate?code=..."
    Body = ($Body | ConvertTo-Json)
    ContentType = "application/json"
}

$updUrl = Invoke-RestMethod @Parameters | ConvertTo-Json
$updUrl


###############################################################


#ARCHIVE ShortURL
$Body = @{
  RowKey = "msft"
  PartitionKey = "m"
  vanity = "msft"
}

#Currently Method is DELETE to Archive a ShortURL, with newest code version change it to POST
$Parameters = @{
    Method = "DELETE" 
    Uri =  "https://shortenertools....azurewebsites.net/api/UrlArchive?code="
    Body = ($Body | ConvertTo-Json)
    ContentType = "application/json"
}

$archUrl = Invoke-RestMethod @Parameters | ConvertTo-Json
$archUrl