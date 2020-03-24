# Azure Url Shortener (AzUrlShortener)

An simple and easy budget friendly Url Shortener for anyone. It runs in Azure (Microsoft cloud) in your subscription.  

> If you don't own an Azure subscription already, you can create your **free** account today. It comes with 200$ credit, so you can experience almost everything without spending a dime. [Create your free Azure account today](https://azure.microsoft.com/en-us/free?WT.mc_id=azurlshortener-github-frbouche)

> This project was inspire by a project created by [Jeremy Likness](https://github.com/JeremyLikness) that you can find here [jlik.me](https://github.com/JeremyLikness/jlik.me).


## To deploy

To deploy YOUR version of **Azure Url Shortener** you could fork this repo, but if you are looking for the easy way just click on the "Deploy to Azure".

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/?WT.mc_id=urlshortener-github-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmaster%2Fdeployment%2FazureDeploy.json)

This will open the Azure Portal (portal.azure.com) in your subscription and create the required resources.

![createARM][createARM]

- **Resource group**: Logical group where your resources will be created.
- **Location**: Select a location (usually closer of your users)
- **Base Name**: This is how you would like to named your resources. Because some names need to be globally unique, the deployment will generate a suffix and append it to the end of your Base name.
- **GitHub URL and Branch**: Keep the default if you when to deploy from Frank's master repo. 
- **Expire On and Owner Name**: Those value are for tags. They **won't affect** in any cases your deployment. I use it in another project ([AzSubscriptionCleaner](https://github.com/FBoucher/AzSubscriptionCleaner)) to clean my subscription; without this project it's just information.

Once all the resources are created you will end-up with: 

- Azure Function: Where the code from the project [src/shortenerTools](src/shortenerTools) will be copy.
- Service Plan: Dynamic service plan (aka. [Consumption Plan](https://azure.microsoft.com/en-us/pricing/details/functions/?WT.mc_id=azurlshortener-github-frbouche)) to make sure you are only charged on a per-second granularity.
- Application Insights: To get some metric/ usage of our function. (In the future we could interface that information in a friendly web UI)
- 2 Storage account: 1 for the Azure Function, the second use as Data Storage leveraging the [Azure Table storage](https://azure.microsoft.com/en-us/services/storage/tables/?WT.mc_id=azurlshortener-github-frbouche).

![ArmResult][ArmResult]


### Deploy using Azure CLI

An alternative method to deploy is to use the Azure CLI command that you can find in the [debug.azcli](deployment/debug.azcli) file.


### Post deployment configuration

Add a custom domain to your Azure Function. (coming soon)
(or check [Microsoft docs](https://docs.microsoft.com/en-ca/azure/app-service/app-service-web-tutorial-custom-domain?WT.mc_id=azurlshortener-github-frbouche))


---


## How to use it

Until there is a *Admin* web page you can call the Azure Function doing simple HTTP calls (ex: using Postman).
To find the URLs of you two functions go to the Azure Portal (portal.azure.com), and open the Azure Function inside the resource created previously.

![getURL][getURL]

Expend the Functions section from the left menu, and click on the Function **UrlRedirect** (1). Then click on the **</> Get function URL** (2) button. And finally, click the **Copy** button to get the URL of your function with the security token. Repeat for the function **UrlShortener**.

### Generate a Shot URL

In your favorite API testing tool (ex: Postman), use the URL from the **UrlShortener** Azure Function. Set the request to POST. In the body of the request, add a JSON document containing two properties. See the examples bellow: 

```json
{
    "url": "http://www.frankysnotes.com/2020/03/reading-notes-416.html",
    "vanity": ""
}
```

This will create a short generic URL.

```json
{
    "url": "http://www.frankysnotes.com/2020/03/reading-notes-416.html",
    "vanity": "rn-416"
}
```

If the passed vanity doesn't already exist, this will create a short using the vanity.

The response will be:

```json
{
    "ShortUrl": "http://localhost:7071/2r",
    "LongUrl": "http://www.frankysnotes.com/2020/03/reading-notes-416.html"
}
```

### Try the redirect

Once the domain will be attach, it will act a little bit different.  To try/ test it use the **UrlRedirect** Url following this pattern. `http://localhost/api/UrlRedirect/{shortUrl}`

```
http://localhost:7071/api/UrlRedirect/z10test
```


> Note: To run it the Azure Function locally, you will need to create a `local.settings.json` file at the root of the project. Here what the file should look like.
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


---


## Contributing

If you find a bug or would like to add a feature, check out those resources:

- To see the current work in progress: [GLO boards](https://app.gitkraken.com/glo/board/XnI94exk8AARj-ph)

Check out our [Code of Conduct](CODE_OF_CONDUCT.md) and [Contributing](CONTRIBUTING.md) docs. This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification.  Contributions of any kind welcome!

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):


[createARM]: medias/createARM.png
[ArmResult]: medias/ArmResult.png
[getURL]: medias/getURL.png