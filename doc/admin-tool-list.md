# Admin frontend for Azure Shortener Urls

You just deployed the AzShortenerUrl backend (the Azure Functions), and now you would like to be able to create Urls! There is many different ways to calls those Azure Functions from a direct HTTP call to a fancy website. 

Here, you will find the list of all available frontend with the instructions to deploy and use them.

---

## List of available Admin frontends

###  [Azure Storage Explorer](https://azure.microsoft.com/en-us/products/storage/storage-explorer/)

Free tool to conveniently manage your Azure cloud storage resources (aka your short URLs) from your desktop. It's cross platform and can be download [here](https://azure.microsoft.com/en-us/products/storage/storage-explorer/).

[Learn about how to use it to manage your URls here](/doc/howto-use-azure-storage-explorer.md).

![Azure Storage Explorer interface](/medias/azure-storage-explorer.png)



###  [TinyBlazorAdmin Website](https://github.com/FBoucher/TinyBlazorAdmin)
Admin tools for Azure Url Shortener using Blazor Single Page Application (webassembly).

More details [here](https://github.com/FBoucher/TinyBlazorAdmin).

![Tiny Blazor Admin looks](../medias/TinyBlazorAdmin.gif)


---

## How to add a new frontend

You don't find the frontend you would like? You can create one in your favorite language and add it to the list!

To do that simply create a pull request for your new frontend. Here some guidance on the format so everybody have the best experience possible.

### Things to include in you Pull Request

1- Add a section to **[List of available Admin frontends](#list-of-available-admin-frontends)**. Make sure you add a short description, language used and a screen shot of your frontend.

2- Create a new folder in the adminTools subfolder with the following structure.

```
├──adminTools
   └───newFrontend
       ├───medias
       |   └───images, screenshots
       ├───src
       |   └───the code
       └───README.md
```

Make sure your readme contains:
- Detailed deployment steps, and how to configure it.
- Utilization steps/ guide
- Use screenshot when possible


## Question, problem?

If you have question or encounter any problem using any admin frontend with AzShortenerUrl please feel free to ask help in the [issues section](https://github.com/FBoucher/AzUrlShortener/issues).

