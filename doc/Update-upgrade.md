# How to Update / Upgrade

You deployed the Azure Url Shortener and it's now running in your Azure Subscription, but you would like to have the new feature(s). Updating your current version is in fact really simple.  

>Note: Currently there is breaking changes between **v1** and **v2**. It is possible to migrate without losing anything, it's just that the documentation is not done yet. See [Issue #196](https://github.com/FBoucher/AzUrlShortener/issues/196) for more details on the progress.

---

## Update the Azure Functions

Navigate to the Azure portal (azure.portal.com) and select the Azure Function instance, for this project.

From the left panel, click on the **Deployment Center** (1), then the **Sync** button(2). This will start a synchronization between GitHub and the App Service (aka Azure Function) local Git. 

![Steps to update the Azure Function doing a Git Sync][AzFunctionGitSync]

IF you are using the Admin Blazor Website, repeat the same operation but selecting the App Service with the name starting by "adm".

[AzFunctionGitSync]: https://github.com/FBoucher/AzUrlShortener/raw/main/medias/AzFunctionGitSync.png