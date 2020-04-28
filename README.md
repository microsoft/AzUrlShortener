# Azure Url Shortener (AzUrlShortener)

[![Deploy to Azure](https://img.shields.io/badge/Deploy%20To-Azure-blue?logo=microsoft-azure)](https://portal.azure.com/?WT.mc_id=urlshortener-github-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmaster%2Fdeployment%2FazureDeploy.json)
[![GLO Board](https://img.shields.io/badge/with-GLO_Board-orange/?color=05887F&logoWidth=15&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFgAAABMCAMAAADeDICLAAAABlBMVEX///8A/+Bm7Mx0AAAAAXRSTlMAQObYZgAAAXlJREFUeNrV2NGOwjAMRNH4/396X5Cm26twww5IrF9CXfvUSlskWPuYOUh4kJ25HzN3RsOBi+SrMolLINu7hFx2t5dROgicLVyRC1fkwgWssru9zJZzuXFd7l2/gT3MXndL+ZEw12VO+lg6OA5gcV3OkitNBxMiDNdk7oXsRLkXzcDswfJJeHVwkIgfhOfL4ZDXzzV8nRIwZHUdNtldhym7SzgZkdWNhokpu8uJvxRep7DLPvC/hte7YfnahOzu3Zr4m0KYXs+FdZCTRD3EbeGzn2MGr/fBkXCdAia034t5GY6HBGXePLgcUEfehw8cWGRxoWDXSQtLxEZ2WAxkHTaX6UEHHzeBJR94s5z0z/aMw3jZKGd1WFzZZofpAoasMN3kVXZ4bgObPGfwYJ69nE8Os8VlUlw2rsvzBJ6h63JoiDjn7v3GOcwel22rWaTy78ItjHIPlBKu/kCe2cKs+xNN2FmnBzCft5fpBN+MsB0dGGxt47AOwsvjB19NCt1bbfikAAAAAElFTkSuQmCC)](https://app.gitkraken.com/glo/board/XnI94exk8AARj-ph)
[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors)

![UrlShortener][UrlShortener]

A simple and easy to use and to deploy budget-friendly Url Shortener for everyone. It runs in Azure (Microsoft cloud) in your subscription.  

> If you don't own an Azure subscription already, you can create your **free** account today. It comes with 200$ credit, so you can experience almost everything without spending a dime. [Create your free Azure account today](https://azure.microsoft.com/en-us/free?WT.mc_id=azurlshortener-github-frbouche)


## How To Deploy

To deploy YOUR version of **Azure Url Shortener** you could fork this repo, but if you are looking for the easy way just click on the "Deploy to Azure".

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/?WT.mc_id=urlshortener-github-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmaster%2Fdeployment%2FazureDeploy.json)

To have all details and alternative deployment refer to the [Deployment Details](azFunctions-deployment.md) page.

#### Post Deployment Configuration

A good Url Shortener wouldn't be completed without a custom domain name. To know how to add it and other useful post-deployment configurations refer to the [post-deployment-configuration](post-deployment-configuration.md) page.


---


## How To Use It

There is many different way to manage your Url Shortener, from a direct HTTP call to a fancy website. 
[Here](src/adminTools/README.md), you will find the list of all available frontend with the instructions to deploy and use them.

[See the complete list of admin frontends here](src/adminTools/README.md)


---


## How It Works

If you are interested to learn more about what's under the hood, and get more details on each Azure Function, read the [How it works](how-it-works.md) page.


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



> This project was inspire by a project created by [Jeremy Likness](https://github.com/JeremyLikness) that you can find here [jlik.me](https://github.com/JeremyLikness/jlik.me).


[UrlShortener]: medias/UrlShortener_600.png