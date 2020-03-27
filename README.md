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

The is many different way to manage your short Urls, from a direct HTTP call to a fancy website. 
[Here](src/adminTools/README.md), you will find the list of all available interfaces with the instructions to deploy and use them.

[See the complete list of admin interfaces here](src/adminTools/README.md)

### How to get the Azure Function URLs?

To find the URLs of you two functions go to the Azure Portal (portal.azure.com), and open the Azure Function inside the resource created previously.

![getURL][getURL]

Expend the Functions section from the left menu, and click on the Function **UrlRedirect** (1). Then click on the **</> Get function URL** (2) button. And finally, click the **Copy** button to get the URL of your function with the security token. Repeat for the function **UrlShortener**.


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


---


## Contributing

If you find a bug or would like to add a feature, check out those resources:

- To see the current work in progress: [GLO boards](https://app.gitkraken.com/glo/board/XnI94exk8AARj-ph)

Check out our [Code of Conduct](CODE_OF_CONDUCT.md) and [Contributing](CONTRIBUTING.md) docs. This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification.  Contributions of any kind welcome!


## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/surlydev"><img src="https://avatars1.githubusercontent.com/u/880671?v=4" width="100px;" alt=""/><br /><sub><b>SurlyDev</b></sub></a><br /><a href="#ideas-surlydev" title="Ideas, Planning, & Feedback">🤔</a></td>
    <td align="center"><a href="http://cloud5mins.com"><img src="https://avatars3.githubusercontent.com/u/2404846?v=4" width="100px;" alt=""/><br /><sub><b>Frank Boucher</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=FBoucher" title="Code">💻</a> <a href="#video-FBoucher" title="Videos">📹</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!


[createARM]: medias/createARM.png
[ArmResult]: medias/ArmResult.png
[getURL]: medias/getURL.png
