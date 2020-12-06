# Post Deployment Configuration

## Add a Custom Domain 

Of course the beauty of a URL shortener is to have short link and this is done the best by a naked domain (domain.com). To realized this you would need to by an expensive certificate with a wild card, and this doesn't fit in the goals of this project. We will achieve it nervertheless by having a secure www.domain.com and doing a dynamic forward of domain.com to www.domain.com.

[<img src="https://img.youtube.com/vi/srZv8aj3ZP8/maxresdefault.jpg" width="50%">](https://youtu.be/srZv8aj3ZP8)

[https://youtu.be/srZv8aj3ZP8](https://youtu.be/srZv8aj3ZP8)
### 1- Assign a Domain to the Azure Function

Let`s start by Adding a domain to the App Service.

![AddCustomDomain][AddCustomDomain]

From the Azure portal (portal.azure.com), open the ShortnerTools App Service (Azure Function). From the left panel select *Custom domains*. Then Click the **Add custom domain** button in the middle of the screen.

![AddCustomDomainDetails][AddCustomDomainDetails]

This will open a tab where you will need to enter your domain with the `www` sub-domain. In our case it was www.07f.ca.

You will now need to validate the domain with you proviver (in our case it's GoDaddy.com). Navigate to the domain provider and create two new DNS record (TXT + CNAME) like explained in the bottom of the panel in the Azure portal.

Here an exemple from godaddy where: 
- The CNAME was created with the URL of the Azure function.
- The TXT was created with the Custom Domain Verification ID provided on the Azure portal page.

![domainProviderDNS][domainProviderDNS]

Once all that is enter. Click the validate button on the Azure portal. If you don't have the green check beside Hostname availability and Domain ownership, be patient as it can take up to 48 hours. (You can always double check that you didn't do any typo any where ;P )

![domainValidated][domainValidated]

When you have it gree you ready for the next step.

### 2- Adding a certificate

The custom domain has been added, we now first need to create a certificate before we can add the binding

![4_AddBinding](https://user-images.githubusercontent.com/52791126/83807076-c81db100-a6b2-11ea-8cc4-1facc50d6dd4.png)

Create a **free** App Service managed certificate as described in the documentation [Secure a custom DNS name with a TLS/SSL binding in Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/configure-ssl-bindings?WT.mc_id=azurlshortener-github-frbouche) 


From the left navigation of your Azure Function (same page as before), select **TLS/SSL settings**. Then from the top of the new panel, click on **Private Key Certificates (.pfx)** and then **Create App Service Managed Certificate**.

![Create free certificate in App Service](https://docs.microsoft.com/en-us/azure/app-service/media/configure-ssl-certificate/create-free-cert.png)
 
Select the custom domain to create a free certificate for and select Create. You can create only one certificate for each supported custom domain.

When the operation completes, you see the certificate in the Private Key Certificates list.

![Create free certificate finished](https://docs.microsoft.com/en-us/azure/app-service/media/configure-ssl-certificate/create-free-cert-finished.png)
 
Go back to the **Custom domain** panel. Click the **Add binding** link beside the big red "Not Secure" label.

Select your custom domain, the created certificate thumbprint which has been created in step 5 and "SNI SSL" as the SSL Binding

![7_create_certificate_binding](https://user-images.githubusercontent.com/52791126/83808893-d5886a80-a6b5-11ea-9cf0-c3268b57d250.png)

Voila! You can now call your azURLShortener from your custom domain! ✔

> If you are using Version 1, you need to change the URL in the App Service configuration (it's in the admin "App Service") so you can copy the URLs from the Blazor UI

![8_change_azFunctionUrl](https://user-images.githubusercontent.com/52791126/83810560-9c9dc500-a6b8-11ea-9c1c-da2d99f2a79f.png)



### 3- Remove the www sub-domain

This next section is to "fake" a naked domain.

![cf_addSite][cf_addSite]

Create an account in cloudflare.com and add a site for your domain. We will need to customized the DNS and create some Page Rules. 

![cf_DNS_Rules][cf_DNS_Rules]

On the cloudflare.com note the 2 nameservers adresses. Go to the origin name provider (in my case godaddy) and replace the nameservers names with the value found on cloudflare.

![cl_dns_details][cl_dns_details]

Now let's create a rules that will redirect all the incoming traffic from the naked-domain to www.domain. On the option on the top, click the **Pages Rules** (B). Then Click the Button **Create Page Rule**

![Rule1][Rule1]

In the field for **If the URL matches:** enter the naked-domain follow by `/*`. That will match everything coming from that URL

For the **settings** select `Forwarding URL` and `301- Permanent Redirect`. Then the destination url should be `https://www.` with your domain and `/$1`.

The URL shortener is now completly configured, and your users will be pleased. There only one last step to make YOUR experience more enjoyable.


### 4- Add A Page Rule for your Admin page (TinyBlazorAdmin)

The TinyBlazorAdmin is the place to manage all your URLs. Create a new Page rule to redirect all call to your domain and the prefix of your choice. ( ex: admin.07f.ca, manager.07f.ca, new.07f.ca, etc)




### 5- Add the new Custom Domain to the Settings file

The last step is to update the `src\shortenerTools\settings.json`. This is the setting file for the Azure Function. Add (or edit if already present) `customDomain` and set it to your new domain.

```
 "customDomain":"https://c5m.ca"
```


Congradulation, you are all set!



---


## How to get the Azure Function URLs?

To find the URLs of you functions go to the Azure Portal (portal.azure.com), and open the Azure Function inside the resource created previously.

![getURL][getURL]

**For Each Function**, expand the Functions section from the left menu, and click on the Function **name** ex: UrlRedirect (1). Then click on the **</> Get function URL** (2) button.  

> **Note:** You will notice a `code` at the end of some functions, it's **IMPORTANT** to keep that code with your URL and to keep it secret. This code is your security token. If your keys get compromised, or if you want to recycle them, it's possible from the the Azure portal in the Azure Function blade click on Platform features | All settings | App Keys.

And finally, click the **Copy** button to get the URL of your function _with the security token_. Repeat for the function **UrlShortener**.


> **Note:** To run it the Azure Function locally, you will need to create a `local.settings.json` file at the root of the project. Here what the file should look like.
> ```
> {
>   "IsEncrypted": false,
>   "Values": {
>     "AzureWebJobsStorage": "CONNSTR_TO_shortenertools",
>     "FUNCTIONS_WORKER_RUNTIME": "dotnet",
>     "FUNCTIONS_V2_COMPATIBILITY_MODE":"true",
>     "UlsDataStorage":"CONNSTR_TO_urldata"
>   }
> }
> ```
> 



[getURL]: medias/getURL.png
[AddCustomDomain]: medias/AddCustomDomain.png
[AddCustomDomainDetails]: medias/AddCustomDomainDetails.png
[domainProviderDNS]: medias/domainProviderDNS.png
[domainValidated]: medias/domainValidated.png


[cf_addSite]: medias/cf_addSite.png
[cf_DNS_Rules]: medias/cf_DNS_Rules.png
[cl_dns_details]: medias/cl_dns_details.png
[cf_rules_details]: medias/cf_rules_details.png
[Rule1]: medias/Rule1.png

