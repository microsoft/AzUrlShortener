How It Works
============

The backend is using Azure Functions and Azure Table Storable this page will explains how they work together in this tool.

![Global Diagram][globalDiagram]

Azure Functions
===============

Azure Function were the perfect match for this project because when you use a dynamic plan you are charged only when the function is running. In our case, it's only a few seconds at the time. To know more read [Azure Function Pricing](https://azure.microsoft.com/en-us/pricing/details/functions/?WT.mc_id=azurlshortener-github-frbouch)


1- Function: UrlArchive
--------------------------

This function set the property `IsArchived` to true. It's a soft delete. When `IsArchived` equal true it won't be return in the list and should works anymore as a short URL. You can call it directly doing an HTTP request of type POST with a header "Content-Type" equal to "application/json". Passing the vanity of that specific URL in the body as showed below.

### Expected Input 

```json
{
    // [Required]
    "PartitionKey": "d",

    // [Required]
    "RowKey": "doc",

    // [Optional] all other properties
}
```

### Output

```json
{
    "Url": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio",
    "Title": "My Title",
    "ShortUrl": null,
    "Clicks": 0,
    "IsArchived": true,
    "PartitionKey": "a",
    "RowKey": "azFunc2",
    "Timestamp": "2020-07-23T06:22:33.852218-04:00",
    "ETag": "W/\"datetime'2020-07-23T10%3A24%3A51.3440526Z'\""
}
```



2- Function: UrlClickStats
--------------------------

This function return the statistic for a specific URL. You can call it directly doing an HTTP request of type POST with a header "Content-Type" equal to "application/json". Passing the vanity of that specific URL in the body as showed below.

### Expected Input 

```json
{
    "vanity": "docs"
}
```

### Output

```json
{
    "ClickStatsList": [
        {
            "Datetime": "2020-04-08 10:42",
            "PartitionKey": "docs",
            "RowKey": "eafa12d4-fae1-xxxx-aaaa-af0e4cfede5e",
            "Timestamp": "2020-04-08T10:42:30.6545923-04:00",
            "ETag": "W/\"datetime'2020-04-08T14%3A42%3A30.6545923Z'\""
        }
    ]
}
```



3- Function: UrlList
-------------------------

This function return a list of all URLs created previously including the clicks count. You can call it directly doing an HTTP request of type GET with a header "Content-Type" equal to "application/json". 

### Expected Input 

No input required.

### Output

```json
{
    "UrlList": [
        {
            "Url": "http://www....",
            "Clicks": 0,
            "PartitionKey": "2",
            "RowKey": "2w",
            "Timestamp": "2020-04-01T16:24:12.5007407-04:00",
            "ETag": "W/\"datetime'2020-04-01T20%3A24%3A12.5007407Z'\""
        }
    ]
}
```

4- Function: UrlRedirect
-------------------------

This function return a HTTP Redirect to the URL. You can call it directly doing an HTTP request of type POST or GET passing the vanity at the end of the URL. The Azure Function Proxy will call Function passing the parameter.

For example if the domain is *c5m.ca* and the vanity is "2w", the request `c5m.ca/2w` will call "UrlRedirect/{shortUrl}" where `shortUrl` is equal to "2w". end the result will be a redirect to the long URL save in the storage.

Every times the Azure Function is called it will increment the click count and save the timestamp when this call appends.


5- Function: UrlShortener
-------------------------

This function creates the short version of our URL and return the info it. You can call it directly doing an HTTP request of type POST with a header "Content-Type" equal to "application/json". Passing the URL and an optional vanity in the body as showed below. If no vanity is specified one will be automatically generated for you!

### Expected Input 

Example of of a body **without** vanity.

```json
{
    "url": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio",
    "title": "MS Docs - Create first function",
    "vanity": ""
}
```

Example of of a body **with** vanity.

```json
{
    "url": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio",
    "title": "MS Docs - Create first function",
    "vanity": "azFunc"
}
```

### Output

Here the result when the vanity wasn't specified and the alternative when the vanity was part of the call.

```json
{
    "ShortUrl": "http://c5m.ca/20",
    "LongUrl": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio"
}
```

```json
{
    "ShortUrl": "http://c5m.ca/azFunc",
    "LongUrl": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio"
}
```

---


6- Function: UrlUpdate
--------------------------

This function will update the properties: `Url` and `Title` to the new value. You can call it directly doing an HTTP request of type POST with a header "Content-Type" equal to "application/json". Passing the vanity of that specific URL in the body as showed below.

### Expected Input 

```json
{
    // [Required]
    "PartitionKey": "d",

    // [Required]
    "RowKey": "doc",

    // [Optional] New Title for this URL, or text description of your choice.
    "title": "Quickstart: Create your first function in Azure using Visual Studio"

    // [Optional] New long Url where the the user will be redirect
    "Url": "https://SOME_URL"
}
```

### Output

```json
{
    "Url": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio",
    "Title": "My Title",
    "ShortUrl": null,
    "Clicks": 0,
    "IsArchived": true,
    "PartitionKey": "a",
    "RowKey": "azFunc2",
    "Timestamp": "2020-07-23T06:22:33.852218-04:00",
    "ETag": "W/\"datetime'2020-07-23T10%3A24%3A51.3440526Z'\""
}
```

---



Azure Table Storage
===================

The [Azure table storage](https://docs.microsoft.com/en-us/azure/storage/tables/?WT.mc_id=azurlShortener-github-frbouche) are the data store in this project. They are a very convenient service to keep structured NoSQL data in the cloud. They are also typically lower in cost than traditional SQL for similar volumes of data.

You can explore the Azure Table storage from Azure portal or using the [Azure Storage Explorer](https://docs.microsoft.com/en-us/azure/vs-azure-tools-storage-manage-with-storage-explorer?tabs=windows#overview?WT.mc_id=azurlShortener-github-frbouche) it's a nice free tool that is available on all platforms (MacOS, Linux, Windows).

There are two tables that will be automatically created at the first call.

1- Table: ClickStats
-------------------------

The ClickStats table get a new entry at every call of the Azure Function **UrlRedirect** with the Datetime value.


1- Table: UrlDetails
-------------------------

The UrlDetails table has the information about all the URLs created. The Vanity, URL , and number of clicks.

[globalDiagram]: medias/globalDiagram.png
