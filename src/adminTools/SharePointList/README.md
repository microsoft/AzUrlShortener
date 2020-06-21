# M365 SharePoint List as Admin UI
> **Note**: Some SharePoint knowledge is required! At least you should know whats a site and a list.
> The PowerAutomate Flows require a premium license (e.g. a per user license) to use the premium connectors.

## Deployment

To deploy YOUR version of **Azure Url Shortener** to Azure and make sure you deploy the Azure Url Shortener __without__ a frontend.
You just need to click on the "Deploy to Azure" button.

[![Deploy to Azure](https://img.shields.io/badge/Deploy%20To-Azure-blue?logo=microsoft-azure)](https://portal.azure.com/?WT.mc_id=urlshortener-github-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmaster%2Fdeployment%2FazureDeploy.json)

> **Note**: make sure you deploy the Azure Url Shortener __without__ a frontend.
![Deploy without frontend](medias/Deploy_AzureUrlShortener_without_Frontend.jpg)


---


## How to use it

### What you need on SharePoint side
First you should have some knowledge what a Site and a list is. You also need to have permission to create a site or a list in an existing site.

Required things:
- Create a SharePoint site (or use an existing one)
- Create a UrlShortener list
    - add columns to list
- Flows to call Azure Functions


Goto https://office.com, login with your M365 account and open a SharePoint site you have permission to create a list or open directly the URL to an existing SharePoint site (e.g. https://yourtenant.sharepoint.com/sites/UrlShortener)

**Manual way**
On a modern page, create a list via **New => List**  
![Create new List](medias/CreateNewList.jpg)

or go to **Site contents**: https://yourtenant.sharepoint.com/sites/UrlShortener/_layouts/15/viewlsts.aspx?view=14 and create it from there via **New => List**
Give the list a name.

Our new list is created and contains already a first "Title" column. We can create/add our required columns (in this order, type and values):
| Column Name   | Column Type | Comments |
| ------------- | ------------- | -------------  |
| Url           | Hyperlink or Picture  | set is as required   |
| Vanity        | Single line of text  |   |
| ShortUrl      | Hyperlink or Picture  |   |
| IsArchived    | Choice  | add choices: true, and false as default value  |
| Archive       | Choice  | add choice YES, no default value, set the "Display choices using" to Radio Buttons (=> This is a "help" column to trigger the flow to delete/archive the ShortUrl) |

![Create list columns](medias/Create_ListColumns.jpg)



**Automated way**
You can import a flow, which creates a site and the list with the required columns automatically for you.

Goto: https://flow.microsoft.com
Then import the Flow: [Download and import Flow](src/ProvisionSharePointsite(UrlShortener)withlistorlistonly_20200621202053.zip)
![Import Flow to create site and list](medias/Import_PowerAutomate_Flow.jpg)

Run the new imported Flow:  
![Run Flow](medias/Run_flow.jpg) 





---


## Question, problem?

If you have question or encounter any problem using this admin Frontend with AzShortenerUrl please feel free to ask help in the [issues section](https://github.com/FBoucher/AzUrlShortener/issues).


[adminBlazorWebsite_Url_list]: medias/adminBlazorWebsite_Url_list.png
[adminBlazorWebsite_Add_Url]: medias/adminBlazorWebsite_Add_Url.png
[portalConfig]: medias/portalConfig.png
