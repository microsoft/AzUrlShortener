# Admin Blazor Website Frontend

## Deployment

To deploy YOUR version of **Azure Url Shortener Admin Blazor WebSite** to Azure, you just need to click on the "Deploy to Azure" button.

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/?WT.mc_id=urlshortener-github-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmain%2Fsrc%2FadminTools%2FadminBlazorWebsite%2Fdeployment%2FadminBlazorWebsite-deployAzure.json)

> **Note**: this is now done automatically during the Azure deployment.
>
> Once it's deployed, you will need to add the URLs of the Azure Function in the configuration of the website. You can use this page to know [How to get the Azure Function URLs](https://github.com/FBoucher/AzUrlShortener/blob/main/post-deployment-configuration.md#how-to-get-the-azure-function-urls). 
> 
> To update the website configuration, go to the Azure Portal [portal.azure.com](portal.azure.com). Find the website you just deployed, and click on it. From the left menu, select *Configuration*. Update the value of the config named: `AzureFunctionUrlListUrl` and `AzureFunctionUrlShortenerUrl`.
> 
> Don't forget to click the **Save** button! This will prompt you to confirm because it will restart the App service. Click Ok.

> ![Update App Service Config][portalConfig]

### Alternative deployment

You can host the website locally or in any regular webserver.  You will need to Build, and Publish yourself.  

To test it a simple F5 in VSCode or Visual Studio should work.


---


## How to use it

### Create a short Url

Once you are in the website, first go into the **Manage Urls** section by clicking the option in the left menu. Than click the **Add New Url** button.

The *Vanity* is the end of  the URL and is optional.

![How To Add a Url][adminBlazorWebsite_Add_Url]


### List all Urls

Once you are in the website, just click on the **Manage Urls** on the left menu.

![See Url list][adminBlazorWebsite_Url_list]


---


## Question, problem?

If you have question or encounter any problem using this admin Frontend with AzShortenerUrl please feel free to ask help in the [issues section](https://github.com/FBoucher/AzUrlShortener/issues).


[adminBlazorWebsite_Url_list]: medias/adminBlazorWebsite_Url_list.png
[adminBlazorWebsite_Add_Url]: medias/adminBlazorWebsite_Add_Url.png
[portalConfig]: medias/portalConfig.png
