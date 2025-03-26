# Azure Url Shortener (AzUrlShortener)

![GitHub Release](https://img.shields.io/github/v/release/microsoft/AzUrlShortener)  ![.NET](https://img.shields.io/badge/9.0-512BD4?logo=dotnet&logoColor=fff) [![Build](https://github.com/microsoft/AzUrlShortener/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/microsoft/AzUrlShortener/actions/workflows/build.yml) ![GitHub License](https://img.shields.io/github/license/microsoft/AzUrlShortener)

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-23-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->


![UrlShortener][UrlShortener]

A simple and easy to use and to deploy budget-friendly Url Shortener for everyone. It runs in your Azure (Microsoft cloud) subscription.  

> If you don't own an Azure subscription already, you can create your **free** account today. It comes with 200$ credit, so you can experience almost everything without spending a dime. [Create your free Azure account today](https://azure.microsoft.com/free?WT.mc_id=dotnet-0000-frbouche)

Features:

- Redirect different destination base on schedules.
- Keep Statistics of your clicks.
- Budget-friendly and 100% open-source.
- Extensible for more enterprise-friendly configurations
- Simple step by step deployment. 
  

## How To Deploy

👉 **[Step by Step Deployment](doc/how-to-deploy.md)** 👈 documentation is available here.

If you want to **Update** or **Upgrade**, please refer to [the faq page](doc/faq.md). 

## How To Use It

Once deployed, use the admin webApp (aka TinyBlazorAdmin) to create new short URLs. 

![Tiny Blazor Admin looks](images/tinyblazyadmin-tour.gif)


### Alternative Admin Tool

By default, all the required resources are deployed into Azure. However you can decide to run the [API](src/Cloud5mins.ShortenerTools.Api/) locally, in a container or somewhere else. You can than use an API client like [Postman](https://www.postman.com/) or a plugin to VSCode like [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client), to manage your URLs. We've included simple API calls via a postman collection and environment [here](./src/tools/).

You can also directly update the tables in storage using [Azure Storage Explorer](doc/how-to-use-azure-storage-explorer.md). 

---

## Videos

There is also a videos that explains a bit how things works and does a quick tour of the project.

| Cloud 5 Mins | Azure Friday |
| ---          | --- |
| [![Tiny Blazor Admin looks](images/AzUrlShortener_preview.gif)](https://youtu.be/fzXy2D77WMM) | [![Azure Friday](/images/AzureFriday_preview.gif)](https://learn.microsoft.com/en-us/shows/azure-friday/azurlshortener-an-open-source-budget-friendly-url-shortener)  |


---


## What's Next?

We are always trying to make it better. See the [AzUrlShortener project](https://github.com/users/FBoucher/projects/6/views/4) page and [issues](https://github.com/microsoft/AzUrlShortener/issues) to see the current progress. 

You are invited to go into the [Discussion](https://github.com/microsoft/AzUrlShortener/discussions) tab to share your feedback, ask question, and suggest new feature! Or have look at our [faq](doc/faq.md) page for more information.

Current Backlog contains:
- More Statistics
- QR Code
- More tracking information (like Country)
- etc.


---


## Contributing

If you find a bug or would like to add a feature, check out those resources:

Check out our [Code of Conduct](CODE_OF_CONDUCT.md) and [Contributing](CONTRIBUTING.md) docs. This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification.  Contributions of any kind welcome!

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/surlydev"><img src="https://avatars1.githubusercontent.com/u/880671?v=4?s=100" width="100px;" alt="SurlyDev"/><br /><sub><b>SurlyDev</b></sub></a><br /><a href="#ideas-surlydev" title="Ideas, Planning, & Feedback">🤔</a></td>
      <td align="center" valign="top" width="14.28%"><a href="http://cloud5mins.com"><img src="https://avatars3.githubusercontent.com/u/2404846?v=4?s=100" width="100px;" alt="Frank Boucher"/><br /><sub><b>Frank Boucher</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=FBoucher" title="Code">💻</a> <a href="#video-FBoucher" title="Videos">📹</a> <a href="https://github.com/microsoft/AzUrlShortener/issues?q=author%3AFBoucher" title="Bug reports">🐛</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/AK0785"><img src="https://avatars1.githubusercontent.com/u/40241010?v=4?s=100" width="100px;" alt="AKER"/><br /><sub><b>AKER</b></sub></a><br /><a href="#ideas-AK0785" title="Ideas, Planning, & Feedback">🤔</a></td>
      <td align="center" valign="top" width="14.28%"><a href="http://baaijte.net"><img src="https://avatars3.githubusercontent.com/u/1761079?v=4?s=100" width="100px;" alt="Vincent Baaij"/><br /><sub><b>Vincent Baaij</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=vnbaaij" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/kmm7"><img src="https://avatars3.githubusercontent.com/u/13196402?v=4?s=100" width="100px;" alt="kmm7"/><br /><sub><b>kmm7</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=kmm7" title="Code">💻</a> <a href="#ideas-kmm7" title="Ideas, Planning, & Feedback">🤔</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/fs366e2spm"><img src="https://avatars2.githubusercontent.com/u/52791126?v=4?s=100" width="100px;" alt="fs366e2spm"/><br /><sub><b>fs366e2spm</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/issues?q=author%3Afs366e2spm" title="Bug reports">🐛</a> <a href="#ideas-fs366e2spm" title="Ideas, Planning, & Feedback">🤔</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/Hedlund01"><img src="https://avatars1.githubusercontent.com/u/48281171?v=4?s=100" width="100px;" alt="Hugo Hedlund"/><br /><sub><b>Hugo Hedlund</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=Hedlund01" title="Code">💻</a></td>
    </tr>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/thefisk"><img src="https://avatars2.githubusercontent.com/u/39799908?v=4?s=100" width="100px;" alt="Nathan Fisk"/><br /><sub><b>Nathan Fisk</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=thefisk" title="Documentation">📖</a></td>
      <td align="center" valign="top" width="14.28%"><a href="http://www.lexplore.com"><img src="https://avatars0.githubusercontent.com/u/3719489?v=4?s=100" width="100px;" alt="Erik Alsmyr"/><br /><sub><b>Erik Alsmyr</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/issues?q=author%3Aalsmyr" title="Bug reports">🐛</a> <a href="https://github.com/microsoft/AzUrlShortener/commits?author=alsmyr" title="Documentation">📖</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://jawn.net"><img src="https://avatars3.githubusercontent.com/u/1705112?v=4?s=100" width="100px;" alt="Bernard Vander Beken"/><br /><sub><b>Bernard Vander Beken</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=jawn" title="Documentation">📖</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/IronManion"><img src="https://avatars0.githubusercontent.com/u/36028632?v=4?s=100" width="100px;" alt="IronManion"/><br /><sub><b>IronManion</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=IronManion" title="Documentation">📖</a></td>
      <td align="center" valign="top" width="14.28%"><a href="http://www.jasonhand.com"><img src="https://avatars0.githubusercontent.com/u/1173344?v=4?s=100" width="100px;" alt="Jason Hand"/><br /><sub><b>Jason Hand</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=jasonhand" title="Documentation">📖</a> <a href="#infra-jasonhand" title="Infrastructure (Hosting, Build-Tools, etc)">🚇</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://Microsoft.com"><img src="https://avatars.githubusercontent.com/u/617586?v=4?s=100" width="100px;" alt="Scott Cate"/><br /><sub><b>Scott Cate</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=scottcate" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/arglgruml"><img src="https://avatars.githubusercontent.com/u/3940298?v=4?s=100" width="100px;" alt="arglgruml"/><br /><sub><b>arglgruml</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/issues?q=author%3Aarglgruml" title="Bug reports">🐛</a></td>
    </tr>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/DavidTCarpenters"><img src="https://avatars.githubusercontent.com/u/50587918?v=4?s=100" width="100px;" alt="DavidTCarpenters"/><br /><sub><b>DavidTCarpenters</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=DavidTCarpenters" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/solvaholic"><img src="https://avatars.githubusercontent.com/u/14636658?v=4?s=100" width="100px;" alt="Roger D. Winans"/><br /><sub><b>Roger D. Winans</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=solvaholic" title="Documentation">📖</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/fatpacket"><img src="https://avatars.githubusercontent.com/u/5621063?v=4?s=100" width="100px;" alt="fatpacket"/><br /><sub><b>fatpacket</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=fatpacket" title="Documentation">📖</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/ch-rob"><img src="https://avatars.githubusercontent.com/u/14352153?v=4?s=100" width="100px;" alt="Chad Voelker"/><br /><sub><b>Chad Voelker</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=ch-rob" title="Code">💻</a> <a href="https://github.com/microsoft/AzUrlShortener/commits?author=ch-rob" title="Documentation">📖</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/davidmginn"><img src="https://avatars.githubusercontent.com/u/831166?v=4?s=100" width="100px;" alt="David Ginn"/><br /><sub><b>David Ginn</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=davidmginn" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="http://www.c-sharpcorner.com/members/catcher-wong"><img src="https://avatars.githubusercontent.com/u/8394988?v=4?s=100" width="100px;" alt="Catcher Wong"/><br /><sub><b>Catcher Wong</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=catcherwong" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/stulzq"><img src="https://avatars.githubusercontent.com/u/13200155?v=4?s=100" width="100px;" alt="Zhiqiang Li"/><br /><sub><b>Zhiqiang Li</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=stulzq" title="Code">💻</a></td>
    </tr>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/ddematheu2"><img src="https://avatars.githubusercontent.com/u/43075365?v=4?s=100" width="100px;" alt="ddematheu2"/><br /><sub><b>ddematheu2</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/issues?q=author%3Addematheu2" title="Bug reports">🐛</a> <a href="https://github.com/microsoft/AzUrlShortener/commits?author=ddematheu2" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://davidop.code.blog/"><img src="https://avatars.githubusercontent.com/u/7433346?v=4?s=100" width="100px;" alt="David Oliva Paredes"/><br /><sub><b>David Oliva Paredes</b></sub></a><br /><a href="https://github.com/microsoft/AzUrlShortener/commits?author=davidop" title="Code">💻</a></td>
    </tr>
  </tbody>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!



> This project was inspired by a project created by [Jeremy Likness](https://github.com/JeremyLikness) that you can find here [jlik.me](https://github.com/JeremyLikness/jlik.me).


[UrlShortener]: images/UrlShortener_600.png
