# Deployment Details

## Deployment with the `Deploy to Azure` Button

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
- Application Insights: To get some metric/ usage of our function. (In the future we could Frontend that information in a friendly web UI)
- 2 Storage account: 1 for the Azure Function, the second use as Data Storage leveraging the [Azure Table storage](https://azure.microsoft.com/en-us/services/storage/tables/?WT.mc_id=azurlshortener-github-frbouche).

![ArmResult][ArmResult]


## Deploy using Azure CLI

An alternative method to deploy is to use the Azure CLI command that you can find in the [debug.azcli](deployment/debug.azcli) file.


[createARM]: medias/createARM.png
[ArmResult]: medias/ArmResult.png