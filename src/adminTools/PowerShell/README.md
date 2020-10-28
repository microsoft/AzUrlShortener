# PowerShell

## Deployment

PowerShell is available out of the box with  every Windows 10 installation, so there is no deployment needed.
Just type "PowerShell" via Windows Start button to start either the "Windows PowerShell" command window or the "Windows PowerShell ISE"

## How to use it
Get the Azure Function URL and run the PowerShell command (as long as the Azure Function doesn't require a login...)

You will need to create a request for each functions. Here some requirements:

- Set the request e.g. to POST.
- Add a header: Content-Type = application/json
- Use the URL **WITH** the security `Code` to call the Azure Function. ([Read here](https://github.com/FBoucher/AzUrlShortener/blob/feature/docs/post-deployment-configuration.md#how-to-get-the-azure-function-urls) to learn how to get the URLs )  


See full examples in provided PowerShell file:
PowerShell with all commands: [PowerShell_Create_List_Update_Archive_ShortURLs.ps1](src/PowerShell_Create_List_Update_Archive_ShortURLs.ps1)


### 1a- Create a short Url

In PowerShell, use the URL from the **UrlShortener** Azure Function.  In the body of the request, add a JSON document containing two properties. 

See the examples bellow to create short generic URL.

```powershell
#CREATE new ShortURL
$Body = @{
  title = "Microsoft"
  url = "https://www.microsoft.com"
  vanity = ""
}
```

If you prefer you can pass a `vanity` to have control on the end part of the Url. In this sample the vanity is `msft`.

```powershell
$Body = @{
  title = "Microsoft"
  url = "https://www.microsoft.com"
  vanity = "msft"
}
```

To execute the call run this.
```powershell
$Parameters = @{
    Method = "POST" 
    Uri =  "https://shortenertools....azurewebsites.net/api/UrlShortener?code=..."
    Body = ($Body | ConvertTo-Json)
    ContentType = "application/json"
}

$newUrl = Invoke-RestMethod @Parameters | ConvertTo-Json
$newUrl
```

### 1b- Bulk Import short Urls from CSV file
Create a csv file, e.g. name it BulkImportUrls.csv or save an excel file as csv.
```csv
title,url,vanity
"Microsoft1","https://www.microsoft.com","msft1"
"Microsoft2","https://www.microsoft.com"
```

Download BulkImportUrls.csv to start with: [BulkImportUrls.csv](src/BulkImportUrls.csv)

Now run the 
[PowerShell_BulkImportUrls.ps1](src/PowerShell_BulkImportUrls.ps1) (maybe not the best PowerShell code but it does its job... You are welcome to enhance it to a more advanced PowerShell script, e.g. using function, etc)


### 2- List all Urls

Use the URL from the **UrlList** Azure Function. Set the request to GET. No body content is required for this request. However, make sure the url contains the security token `code`.

    https://shortenertools.azurewebsites.net/api/UrlList?code=JVzE6CvlEHxDHbq.....


To execute the call:
```powershell
#GET all URls
$Parameters = @{
    Method = "GET" 
    Uri =  "https://shortenertoolsn....azurewebsites.net/api/UrlList?code=..."
    ContentType = "application/json"
}

$allUrls = Invoke-RestMethod @Parameters | ConvertTo-Json
$allUrls
```

The response will be a json document with an array:
```json
{
    "UrlList": [
        {
            "Url": "http://www.frankysnotes.com/2020/03/reading-notes-416.html",
            "PartitionKey": "2",
            "RowKey": "2r",
            "Timestamp": "2020-03-20T13:43:47.5758051+00:00",
            "ETag": "W/\"datetime'2020-03-20T13%3A43%3A47.5758051Z'\""
        },
        {
            "Url": "http://www.frankysnotes.com/2020/03/reading-notes-416.html",
            "PartitionKey": "t",
            "RowKey": "test10h24",
            "Timestamp": "2020-03-27T14:26:44.4342376+00:00",
            "ETag": "W/\"datetime'2020-03-27T14%3A26%3A44.4342376Z'\""
        },
        {
            "Url": "https://www.frankysnotes.com/2020/03/reading-notes-416.html",
            "PartitionKey": "z",
            "RowKey": "z10test",
            "Timestamp": "2020-03-20T15:27:08.8691188+00:00",
            "ETag": "W/\"datetime'2020-03-20T15%3A27%3A08.8691188Z'\""
        }
    ]
}
```

### 3- Update entry

Use the URL from the **Update** Azure Function. 

    https://shortenertools.azurewebsites.net/api/UrlList?code=JVzE6CvlEHxDHbq.....


To update an entry:
```powershell
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
```

### 4- Archive entry

Use the URL from the **Archive** Azure Function. 

    https://shortenertools.azurewebsites.net/api/UrlList?code=JVzE6CvlEHxDHbq.....


To archive an entry:
```powershell
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
```



## Question, problem?

If you have question or encounter any problem using this admin interface with AzShortenerUrl please feel free to ask help in the [issues section](https://github.com/FBoucher/AzUrlShortener/issues).