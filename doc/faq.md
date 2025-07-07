# Frequently asked questions (f.a.q.)

- [How to Add a Custom Domain](./how-to-add-custom-domain.md)
- [Add authentication to the admin website](./how-to-deploy.md#add-authentication-to-the-admin-website)
- [Add a custom domain to the admin website](./how-to-add-custom-domain.md#add-a-custom-domain-to-the-admin-website)
- [How to migrate your data](./how-to-migrate-data.md)
- [How to deploy your AzUrlShortener](./how-to-deploy.md)
- [How to run AzUrlShortener locally](#how-to-run-azurlshortener-locally)
- [How to update/ redeploy AzUrlShortener](#update-redeploy-azurlshortener)
- [How does it work?](./how-it-works.md)
- [Security Considerations](./security-considerations.md)
- [How to make the api public](./how-to-set-api-public.md)


## How to run AzUrlShortener locally

You will need .NET 9, Docker or Podman installed on your machine. From the `scr` directory, run the following command `dotnet run --project AppHost`. You can also open the solution in Visual Studio or Visual studio Code and use F5, make sure the `Cloud5mins.ShortenerTools.AppHost` project is set as starting project.


## Update/ redeploy AzUrlShortener

In a terminal, navigate to the `src` directory of your project.

```bash
cd src
```

To avoid affecting custom domains when deploying Azure Container Apps use the following command. 

```bash
azd config set alpha.aca.persistDomains on
```

If you haven't already, log in to your Azure account with azd auth login. You can re-deploy the application with the following command:

```bash
azd up
```