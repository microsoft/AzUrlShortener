# Azure Url Shortener (AzUrlShortener)

[![Deploy to Azure](https://img.shields.io/badge/Deploy%20To-Azure-blue?logo=microsoft-azure)](https://portal.azure.com/?WT.mc_id=dotnet-0000-frbouche#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FFBoucher%2FAzUrlShortener%2Fmain%2Fdeployment%2FazureDeploy.json)
[![GLO Board](https://img.shields.io/badge/with-GLO_Board-orange/?color=05887F&logoWidth=15&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFgAAABMCAMAAADeDICLAAAABlBMVEX///8A/+Bm7Mx0AAAAAXRSTlMAQObYZgAAAXlJREFUeNrV2NGOwjAMRNH4/396X5Cm26twww5IrF9CXfvUSlskWPuYOUh4kJ25HzN3RsOBi+SrMolLINu7hFx2t5dROgicLVyRC1fkwgWssru9zJZzuXFd7l2/gT3MXndL+ZEw12VO+lg6OA5gcV3OkitNBxMiDNdk7oXsRLkXzcDswfJJeHVwkIgfhOfL4ZDXzzV8nRIwZHUdNtldhym7SzgZkdWNhokpu8uJvxRep7DLPvC/hte7YfnahOzu3Zr4m0KYXs+FdZCTRD3EbeGzn2MGr/fBkXCdAia034t5GY6HBGXePLgcUEfehw8cWGRxoWDXSQtLxEZ2WAxkHTaX6UEHHzeBJR94s5z0z/aMw3jZKGd1WFzZZofpAoasMN3kVXZ4bgObPGfwYJ69nE8Os8VlUlw2rsvzBJ6h63JoiDjn7v3GOcwel22rWaTy78ItjHIPlBKu/kCe2cKs+xNN2FmnBzCft5fpBN+MsB0dGGxt47AOwsvjB19NCt1bbfikAAAAAElFTkSuQmCC)](https://app.gitkraken.com/glo/board/XnI94exk8AARj-ph)
[![Serverless Library](https://img.shields.io/badge/Serverless%20Library-%E2%9C%94%EF%B8%8Fyes-blue?style=flat&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABkAAAAZCAMAAADzN3VRAAACx1BMVEUAAAAAAAAAAP8Af38AVaoAf78AVaoAbbYAXLkAc7kAZqoAWqUAY7gAZrIAZa4AYKwAY7QAZ7YAZ7MAZ7UAZLMAY7MAZLUAZrIAZLcAY7QAZrIAZLMAZLYAZ7YAZLYAZrcAZbgAZLMAZ7YAZbIAZLQAYrMAY7UAZLUAZLYAZbQAZrcAZbMAZbgAZbQAZrQAZrgAZrcAZbgAaLkAZbUAZbYAZ7YAZbUAZrUAZ7YAZ7cAZbcAZrcAZ7cAZrcAZ7cAZrcAZrcAZbcAZ7YAZ7cAZrcAZrYAZrcAZbYAZrcAZ7cAZrYAZrcAZrcAZ7cAZ7gAZrcAZrcAZ7cAZ7gAZ7gBaLcCaLcDabgEaLYFabUGarQGa7oHarMIbLoKbbsMbrsOb7wPcLwQcb0Rcb0SbawTcr0Xdb4Ydb8Zdr8dc6UeecEfdKQgcqMhe8Eic6IkdqEkfcIndJ4of8MqeJ0rgcQsdpsseZwxhMY2eZQ3iMc7fZI7gJJBjstCfYxGkcxHkcxMh4hNlc5TmM9VioFVmtBYnNFZnNFbndFbntFcntFcntJdn9Jfh3lhodNiodNnpNVopdVsi3Bsp9Ztj3BulXFuqdZuqddwqtd0mG11mG12j2l7mml9kWWGn2KMlluUpVmWpliaqFWpoEeusEivokOz0uq31Ou51eu61uy7pju81+2/2O3BqDfB2u7C2u7C2+7DqTbD2+7EuTrF3O/F3e/G3e/H3e/H3u/ItTbKqzLKuzbL4PDOsjDOvTTW5vTb6vXdwyrgsyPg7ffitCLk7/jm8PjqyCLq8/ns9PrttxrwuBjxuBfyzB3zvBfzzBzz+Pz0uRb1uRX1+fz2+v33+v34+/35uxL5/P36/P77wxP70Bf7/f78yhT8yxT8zBX8zRX8zhX8zxb80Bb8/f79vA/9vQ/9vhD9vxD9wBH9whH9xBL9xhP9xxP9yBOZ2sNJAAAAU3RSTlMAAQECAwQGBwsLDxESFCMlKSovOz1AQkNHSFBRUVFUVVpbW11gYmRlZmdoaWlqbXByc3V5e36AgYOIubrBysvR09XX2drd3ub2+vv7/Pz8/f7+/mxm4TkAAAHCSURBVHgBXcz1U1VBHEDxr90d2N2KLYbYAbZwVAzB7g6xOxS7OzCwu8UQFVtEBUMxWFeeoX+Ee5e5bxg+v56ZI155itesX80nl6STpYJ/EI5uzUplShsKtwVGP4+LexEff7hlQfHy7YKxU6kkrZ+NI7CsG2pj3VLql9YbgZ5lxCraHRjJDKW+an2qzwgg0A6zdQSWvhm/V6nf+tXkWS/nAi0ymFILWJEY0fuuSv6hd8H21wuA0qa0hrXvVzJHqb/6egiwKWERNBfJBxtM4IBSKfpJTEzM8b7rExbTOadUZNunCOj3UH3+6fF49NuZsC5xOT7SaOv3VcBCZSXpPRir3y2rIk1vx4YBS/ZHRh5SynMuBCP0wbE64jfl8clQrC3q39MJGGFno4dXlcqEPzqRmi4kp6yx4XT0UIpJAZgWe3QgMPbjn4M2nLk6hB65RdrDpHtRJu34dn8YMOj85cHgLyJ1gTF3jvTnxod5Trh0xQTKmZIjABh1MXzil30Y86Oc0CazGCWDcOy+OQBXp0JiNcZxbTpe1SVVRr9gmLoZV1df8SoRwOxebuhQRNLIXq8dVnCrGlklnfyVGjZpUD6vuP4DQn3cxeG842QAAAAASUVORK5CYII=)](https://serverlesslibrary.net/sample/1c809aa2-2d4f-4fee-bc27-0c2c36844ac8)
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-13-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->


![UrlShortener][UrlShortener]

A simple and easy to use and to deploy budget-friendly Url Shortener for everyone. It runs in Azure (Microsoft cloud) in your subscription.  

> If you don't own an Azure subscription already, you can create your **free** account today. It comes with 200$ credit, so you can experience almost everything without spending a dime. [Create your free Azure account today](https://azure.microsoft.com/free?WT.mc_id=dotnet-0000-frbouche)

Features:

- Create Edit and Delete short URL.
- Redirect different destination base on schedules
- Keep Statistics of your clicks.
- Budget-friendly and 100% open-source.
- Secure by Azure Active Directory AAD (Microsoft Identity)
- Simple step by step deployment 

![Tiny Blazor Admin looks](/medias/TinyBlazorAdmin.gif)

## How To Deploy

We suggest to deploy it paired with [Tiny Blazor Admin](https://github.com/FBoucher/TinyBlazorAdmin); it's a static website. However also possible do deploy it headless (just the Azure Function), if you want.  

👉 **[Step by Step Deployment](https://github.com/FBoucher/TinyBlazorAdmin/blob/main/deployment.md)** 👈 documentation is available here. This will bring you on the TinyBlazorAdmin repository where you will deploying both projects.

To have all alternative and previous version deployments, refer to the [Azure Function Deployment Details](https://github.com/FBoucher/AzUrlShortener/blob/main/doc/azFunctions-deployment.md) page.

If you want to **Update** or **Upgrade**, please refer to [this page](https://github.com/FBoucher/AzUrlShortener/blob/main/doc/Update-upgrade.md). 

## How To Use It

There are many different ways to manage your Short Urls, from a direct HTTP call to a fancy website. 
[See the complete list of admin frontends here](src/adminTools/README.md), with the instructions to deploy and use them. 

There is also a video that does a quick tour of the project.

[![YouTube thumbnail of the Az URL Shortener quick tour video](https://img.youtube.com/vi/fzXy2D77WMM/hqdefault.jpg)](https://youtu.be/fzXy2D77WMM)

Youtube: [https://youtu.be/fzXy2D77WMM](https://youtu.be/fzXy2D77WMM)


---


## How It Works

If you are interested to learn more about what's under the hood, and get more details on each Azure Function, read the [How it works](doc/how-it-works.md) page.


---


## What's Next?

We are always trying to make it better. See  the [GitKraken boards](https://app.gitkraken.com/glo/board/XnI94exk8AARj-ph) and [issues](https://github.com/FBoucher/AzUrlShortener/issues) to see the current progress. 

You are invited to go into the [Discussion](https://github.com/FBoucher/AzUrlShortener/discussions) tab to share your feedback, ask question, and suggest new feature!

Current Backlog contains:
- More Statistics
- QR Code
- More tracking information (like Country)
- etc.


---


## Contributing

If you find a bug or would like to add a feature, check out those resources:

- To see the current work in progress: [GLO boards 'kanban board'](https://app.gitkraken.com/glo/board/XnI94exk8AARj-ph) </br></br> [![AzUrlShortener Glo Board 'kanban board'][glo]](https://app.gitkraken.com/glo/board/XnI94exk8AARj-ph)

Check out our [Code of Conduct](CODE_OF_CONDUCT.md) and [Contributing](CONTRIBUTING.md) docs. This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification.  Contributions of any kind welcome!

There is also instructions or [guidance](src/adminTools/README.md#how-to-add-a-new-frontend) if you would like to create a new one and collaborate to this project.


## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/surlydev"><img src="https://avatars1.githubusercontent.com/u/880671?v=4?s=100" width="100px;" alt=""/><br /><sub><b>SurlyDev</b></sub></a><br /><a href="#ideas-surlydev" title="Ideas, Planning, & Feedback">🤔</a></td>
    <td align="center"><a href="http://cloud5mins.com"><img src="https://avatars3.githubusercontent.com/u/2404846?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Frank Boucher</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=FBoucher" title="Code">💻</a> <a href="#video-FBoucher" title="Videos">📹</a> <a href="https://github.com/FBoucher/AzUrlShortener/issues?q=author%3AFBoucher" title="Bug reports">🐛</a></td>
    <td align="center"><a href="https://github.com/AK0785"><img src="https://avatars1.githubusercontent.com/u/40241010?v=4?s=100" width="100px;" alt=""/><br /><sub><b>AKER</b></sub></a><br /><a href="#ideas-AK0785" title="Ideas, Planning, & Feedback">🤔</a></td>
    <td align="center"><a href="http://baaijte.net"><img src="https://avatars3.githubusercontent.com/u/1761079?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Vincent Baaij</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=vnbaaij" title="Code">💻</a></td>
    <td align="center"><a href="https://github.com/kmm7"><img src="https://avatars3.githubusercontent.com/u/13196402?v=4?s=100" width="100px;" alt=""/><br /><sub><b>kmm7</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=kmm7" title="Code">💻</a> <a href="#ideas-kmm7" title="Ideas, Planning, & Feedback">🤔</a></td>
    <td align="center"><a href="https://github.com/fs366e2spm"><img src="https://avatars2.githubusercontent.com/u/52791126?v=4?s=100" width="100px;" alt=""/><br /><sub><b>fs366e2spm</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/issues?q=author%3Afs366e2spm" title="Bug reports">🐛</a> <a href="#ideas-fs366e2spm" title="Ideas, Planning, & Feedback">🤔</a></td>
    <td align="center"><a href="https://github.com/Hedlund01"><img src="https://avatars1.githubusercontent.com/u/48281171?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Hugo Hedlund</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=Hedlund01" title="Code">💻</a></td>
  </tr>
  <tr>
    <td align="center"><a href="https://github.com/thefisk"><img src="https://avatars2.githubusercontent.com/u/39799908?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Nathan Fisk</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=thefisk" title="Documentation">📖</a></td>
    <td align="center"><a href="http://www.lexplore.com"><img src="https://avatars0.githubusercontent.com/u/3719489?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Erik Alsmyr</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/issues?q=author%3Aalsmyr" title="Bug reports">🐛</a> <a href="https://github.com/FBoucher/AzUrlShortener/commits?author=alsmyr" title="Documentation">📖</a></td>
    <td align="center"><a href="https://jawn.net"><img src="https://avatars3.githubusercontent.com/u/1705112?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Bernard Vander Beken</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=jawn" title="Documentation">📖</a></td>
    <td align="center"><a href="https://github.com/IronManion"><img src="https://avatars0.githubusercontent.com/u/36028632?v=4?s=100" width="100px;" alt=""/><br /><sub><b>IronManion</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=IronManion" title="Documentation">📖</a></td>
    <td align="center"><a href="http://www.jasonhand.com"><img src="https://avatars0.githubusercontent.com/u/1173344?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Jason Hand</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=jasonhand" title="Documentation">📖</a> <a href="#infra-jasonhand" title="Infrastructure (Hosting, Build-Tools, etc)">🚇</a></td>
    <td align="center"><a href="https://Microsoft.com"><img src="https://avatars.githubusercontent.com/u/617586?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Scott Cate</b></sub></a><br /><a href="https://github.com/FBoucher/AzUrlShortener/commits?author=scottcate" title="Code">💻</a></td>
  </tr>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!



> This project was inspire by a project created by [Jeremy Likness](https://github.com/JeremyLikness) that you can find here [jlik.me](https://github.com/JeremyLikness/jlik.me).


[UrlShortener]: https://github.com/FBoucher/AzUrlShortener/raw/main/medias/UrlShortener_600.png
[ThumbnailYTAzUrlShortener_EN]: https://github.com/FBoucher/AzUrlShortener/raw/main/medias/ThumbnailYTAzUrlShortener_EN.png
[glo]: https://github.com/FBoucher/AzUrlShortener/raw/main/medias/glo-board_screenshot.png
[AzFunctionGitSync]: https://github.com/FBoucher/AzUrlShortener/raw/main/medias/AzFunctionGitSync.png


