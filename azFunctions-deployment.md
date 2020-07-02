# Deployment Details

This page describes step by step how to deploy the Azure URL Shortener. To learn more  about how the deployment was built see the section [below](#how-the-deployment-works)

## Deployment with the `Deploy to Azure` Button

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/?WT.mc_id=urlshortener-github-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmain%2Fdeployment%2FazureDeploy.json)

This will open the Azure Portal (portal.azure.com) in your subscription and create the required resources.

![createARM][createARM]

- **Resource group**: Logical group where your resources will be created.
- **Location**: Select a location (usually closer of your users)
- **Base Name**: This is how you would like to named your resources. Because some names need to be globally unique, the deployment will generate a suffix and append it to the end of your Base name.
- **Frontend**: Select the frontend that will be deploy. Select 'none', if you don't want any. Frontend available: `adminBlazorWebsite`, `none`. 
- **Default Url Redirect**: Default URL use when the key pass by the user is not found.
- **GitHub URL and Branch**: Keep the default if you when to deploy from Frank's main repo. 

#### Setting for adminBlazorWebsite (Required only if frontend = adminBlazorWebsite)

- **Frontend-Admin E Mail**: The EMail use to connect into the admin Blazor Website.
- **Frontend-Admin Password**: Password use to connect into the admin Blazor Website. It **_MOST_** have:
  - an uppercase character
  - a lowercase character
  - a digit
  - a non-alphanumeric character 
  - _must be at least six characters long_


- **Expire On and Owner Name**: Those value are for tags. They **won't affect** in any cases your deployment. I use it in another project ([AzSubscriptionCleaner](https://github.com/FBoucher/AzSubscriptionCleaner)) to clean my subscription; without this project it's just information.

Once all the resources are created you will end-up with: 

- Azure Function: Where the code from the project [src/shortenerTools](src/shortenerTools) will be copy.
- Service Plan: Dynamic service plan (aka. [Consumption Plan](https://azure.microsoft.com/en-us/pricing/details/functions/?WT.mc_id=azurlshortener-github-frbouche)) to make sure you are only charged on a per-second granularity.
- Application Insights: To get some metric/ usage of our function. (In the future we could Frontend that information in a friendly web UI)
- 2 Storage account: 1 for the Azure Function, the second use as Data Storage leveraging the [Azure Table storage](https://azure.microsoft.com/en-us/services/storage/tables/?WT.mc_id=azurlshortener-github-frbouche).

![ArmResult][ArmResult]


---


## Deploy using Azure CLI

An alternative method to deploy is to use the Azure CLI command that you can find in the [debug.azcli](deployment/debug.azcli) file.


---


## How the Deployment works

The deployment uses Azure Resource Manager (ARM) template, one for the Azure Function, and one for the frontend. 

You can deploy each part individually by using the respective ARM template in the deployment folder or all together. To get that possible, the templates were nested. 

You can learn more about it on [Using linked and nested templates when deploying Azure resources](https://c5m.ca/nestedARM) in the Microsoft documentation, watching this video where I explains how I built this deployment.

[![Thumbnail of the YouTube video about the Nested ARM template deployment][Episode60_EN]](https://youtu.be/IePDTQk6Bz8)

There also a **blog post**: [Simplify your deployment with nested Azure Resource Manager (ARM) templates](http://www.frankysnotes.com/2020/05/simplify-your-deployment-with-nested.html)

If you are interested to see the ARM templates before-after you can go in the [Tutorial Section](tutorials/optional-arm/Howto.md)



[createARM]: medias/createARM.png
[ArmResult]: medias/ArmResult.png
[Episode60_EN]: medias/Episode60_EN.png
