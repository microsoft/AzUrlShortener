# M365 SharePoint List as Admin UI

## Deployment

To deploy YOUR version of **Azure Url Shortener** to Azure and make sure you don't create a 
 you just need to click on the "Deploy to Azure" button.

[![Deploy to Azure](https://img.shields.io/badge/Deploy%20To-Azure-blue?logo=microsoft-azure)](https://portal.azure.com/?WT.mc_id=urlshortener-github-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmaster%2Fdeployment%2FazureDeploy.json)

> **Note**: make sure you deploy the Azure Url Shortener __without__ a frontend.
![Deploy without frontend](medias/Deploy_AzureUrlShortener_without_Frontend.jpg)


---


## How to use it

### Create a SharePoint list

Once you are in the website, first go into the **Manage Urls** section by clicking the option in the left menu. Than click the **Add New Url** button.

The *Vanity* is the end of  the URL and is optional.

![How To create the SharePoint Admin UI list][adminBlazorWebsite_Add_Url]


### List all Urls

Once you are in the website, just click on the **Manage Urls** on the left menu.

![See Url list][adminBlazorWebsite_Url_list]


---


## Question, problem?

If you have question or encounter any problem using this admin Frontend with AzShortenerUrl please feel free to ask help in the [issues section](https://github.com/FBoucher/AzUrlShortener/issues).


[adminBlazorWebsite_Url_list]: medias/adminBlazorWebsite_Url_list.png
[adminBlazorWebsite_Add_Url]: medias/adminBlazorWebsite_Add_Url.png
[portalConfig]: medias/portalConfig.png
