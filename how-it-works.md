How It Works
============

The backend is using Azure Functions and Azure Table Storable this page will explains how they work together in this tool.

Azure Functions
===============

Azure Function were the perfect match for this project because when you use a dynamic plan you are charged only when the function is running. In our case it's only a few seconds at the time. To know more read [Azure Function Pricing](https://azure.microsoft.com/en-us/pricing/details/functions/?WT.mc_id=azurlshortener-github-frbouch)


1- Function: UrlClickStats
--------------------------

This function return the statistic for a specific URL.

### Expected Input 

```json
{
    ""
}
```

### Output

```json

```



2- Function: UrlList
-------------------------


### Expected Input 

No input required

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
        },
        ...
}
```

3- Function: UrlRedirect
-------------------------

(soon)

4- Function: UrlShortener
-------------------------

(soon)


---

Azure Table Storage
===================

(soon explain why table for this project)

1- Table: ClickStats
-------------------------

(soon)

1- Table: UrlDetails
-------------------------

(soon)