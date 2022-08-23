# How to use Azure Storage Explorer as admin tool for AzUrlShortener

Azure Storage Explorer is a free tool to manage your Azure cloud storage resources (aka your short URLs) from your desktop. It's cross platform and can be download [here](https://azure.microsoft.com/en-us/products/storage/storage-explorer/). 

Once installed, you need to configure it to access your Azure account. You can do this by clicking on the Open connection dialog the **DC plug** icon in the top left corner, then select the Azure Subscription option.

![Azure Storage Explorer, Open connection dialog](/medias/ase-connect-to-azure.png)

And follow the steps to login to your Azure account.

## Find the URLs Storage table

To find the table where the URLs data is saved, expend the your subscription (with the key icon) in the tree-view. Expand the Storage accounts, your account (probably starting by 'urldata'), Tables and finally look for `UrlsDetails`

![Finding the UrlsDetails table in Azure Storage Explorer](/medias/ase-find-urls-table.png)

Pro tip: You can right-click on the table and select `Pin to Quick Access` to create a shortcut to it.

## Create a new URL

To create a new URL, make sure the `UrlsDetails` table is selected (directly or using the quick access), then click on the **+ Add** button in the top menu.

![Creating a new URL in Azure Storage Explorer](/medias/ase-create-url.png)

The *Add Entity* dialog window will open, You will need to fill it in order to create the URL.

- **Partition key:** (Required) This MUST be the first character of yout short URL (aka RowKey).
- **RowKey:** (Required) The short URL.
- **Id:** Never set any value in this field, this is used by the system and URLs don't need then.
- **Clicks:** Don't set any value in this field, it will be incremented automatically when your short URL is used.
- **Url:** (Required) The original (aka long) URL.
- **Title:** (Optional) This is for at your convenience. Write something that help you understand what the URL is from.
- **Is Archived:** Since we create a new URL leave that field empty.