# How to use Azure Storage Explorer as admin tool for AzUrlShortener

Azure Storage Explorer is a free tool to manage your Azure cloud storage resources (aka your short URLs) from your desktop. It's cross platform and can be download [here](https://azure.microsoft.com/en-us/products/storage/storage-explorer/). 

Once installed, you need to configure it to access your Azure account. You can do this by clicking on the Open connection dialog the **DC plug** icon in the top left corner, then select the Azure Subscription option.

![Azure Storage Explorer, Open connection dialog](/medias/ase-connect-to-azure.png)

And follow the steps to login to your Azure account.

## Find the URLs Storage table

To find the table where the URLs data is saved, expend the your subscription (with the key icon) in the tree-view. Expand the Storage accounts, your account (probably starting by 'urldata'), Tables and finally look for `UrlsDetails`

![](/medias/ase-find-urls-table.png)