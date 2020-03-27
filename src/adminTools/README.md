# Admin interfaces for Azure Shortener Urls

You just deployed the AzShortenerUrl backend (the Azure Functions), and now you would like to be able to create Urls! There is many different ways to calls those Azure Functions from a direct HTTP call to a fancy website. 

Here, you will find the list of all available interfaces with the instructions to deploy and use them.

---

## List of available Admin interfaces


### [Postman](postman/README.md)

Simple use of the API testing tool Postman to call the Azure Functions.

![Postman Create ShotUrl](medias/postman_createShotUrl.png)

More details [here](postman/README.md).




---

## How to add a new interface

You don't find the interface you would like? You can create one in your favorite language and add it to the list!

To do that simply create a pull request for your new interface. Here some guidance on the format so everybody have the best experience possible.

### Things to include in you Pull Request

1- Add a section to **[List of available Admin interfaces](#list-of-available-admin-interfaces)**. Make sure you add a short description, language used and a screen shot of your interface.

2- Create a new folder in the adminTools subfolder with the following structure.

```
├──adminTools
   └───newInterface
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

If you have question or encounter any problem using any admin interface with AzShortenerUrl please feel free to ask help in the [issues section](https://github.com/FBoucher/AzUrlShortener/issues).