# Post Deployment Configuration

## Add a Custom Domain 

To know how to add a custom domain to your Azure Function. (coming soon)
(or check [Microsoft docs](https://docs.microsoft.com/en-ca/azure/app-service/app-service-web-tutorial-custom-domain?WT.mc_id=azurlshortener-github-frbouche))


---


## How to get the Azure Function URLs?

To find the URLs of you functions go to the Azure Portal (portal.azure.com), and open the Azure Function inside the resource created previously.

![getURL][getURL]

**For Each Function**, expand the Functions section from the left menu, and click on the Function **name** ex: UrlRedirect (1). Then click on the **</> Get function URL** (2) button.  

> **Note:** You will notice a `code` at the end of some functions, it's **IMPORTANT** to keep that code with your URL and to keep it secret. This code is your security token. If your keys get compromised, or if you want to recycle them, it's possible from the the Azure portal in the Azure Function blade click on Platform features | All settings | App Keys.

And finally, click the **Copy** button to get the URL of your function _with the security token_. Repeat for the function **UrlShortener**.


> **Note:** To run it the Azure Function locally, you will need to create a `local.settings.json` file at the root of the project. Here what the file should look like.
> ```
> {
>   "IsEncrypted": false,
>   "Values": {
>     "AzureWebJobsStorage": "CONNSTR_TO_shortenertools",
>     "FUNCTIONS_WORKER_RUNTIME": "dotnet",
>     "FUNCTIONS_V2_COMPATIBILITY_MODE":"true",
>     "UlsDataStorage":"CONNSTR_TO_urldata"
>   }
> }
> ```
> 



[getURL]: medias/getURL.png
