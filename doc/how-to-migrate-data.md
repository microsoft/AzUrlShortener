# How to migrate your data

The easiest way to migrate your data between account or from an ealier version of AzUrlShortener is to use the **Azure Storage Explorer** to export the data as a CSV files and then import it into the new account or version.

Azure Storage Explorer is a free tool to manage your Azure cloud storage resources (aka your short URLs) from your desktop. It’s a cross-platform and can be downloaded [here](https://azure.microsoft.com/en-us/products/storage/storage-explorer/). 

In the resources deployed to Azure there will be 2 storage accounts. One for the redirect service (azfunc-light) and one acting as the data store. That last one is the one you want to use for the data migration. The name of that storage account should start wirh `urldata`. 

You need to expot to CSV the following tables:
- UrlsDetails: This table contains the details of the short URLs, including the original URL, the short URL, schedules, etc.
- ClickStats: This table contains the clicks informations.
