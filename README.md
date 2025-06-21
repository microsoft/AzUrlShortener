# AzUrlShortener

## Project Summary
AzUrlShortener is a lightweight URL shortening solution built on Azure Functions. It allows teams to create, manage and track short links for marketing campaigns or operational communications. This repository contains the backend functions and a set of optional admin front‑ends used by our consultancy when delivering client solutions.

## Business Context
This project is typically deployed as part of client engagements where a custom URL shortener is required. Business users such as marketing managers or product owners can administer short links through a variety of front‑ends (Postman scripts, Blazor websites, PowerShell or SharePoint). The backend runs entirely on Azure and can be deployed in a client subscription.

## Technical Overview
- **Backend**: Azure Functions v3 (.NET Core 3.1)
- **Data Storage**: Azure Table Storage
- **Core functions**:
  - `UrlShortener` – create new short URLs
  - `UrlRedirect` – redirect based on vanity code
  - `UrlUpdate` – edit existing entries
  - `UrlArchive` – soft‑delete (archive) URLs
  - `UrlList` – list all active URLs
  - `UrlClickStats` – retrieve click statistics
- **Admin Tools**: optional front‑ends located under `src/adminTools`

## Setup Instructions
1. **Prerequisites**
   - .NET Core SDK 3.1
   - Azure CLI
   - Access to an Azure subscription
2. **Deployment**
   - Use the ARM template in `deployment/azureDeploy.json` to provision the Function App, storage and supporting resources.
   - Example:
     ```bash
     az deployment group create \
       --resource-group <rg> \
       --template-file deployment/azureDeploy.json \
       --parameters baseName=<name> frontend=none
     ```
   - The template also supports deploying the optional Blazor admin site.
3. **Configuration**
   - After deployment, set required app settings such as `UlsDataStorage`, `defaultRedirectUrl` and optional `customDomain`. See `src/shortenerTools/settings.json` for reference.

## Running Locally
1. Clone the repository and open `AzUrlShortener.sln` in Visual Studio or VS Code.
2. Copy `src/shortenerTools/local.settings.json` from `settings.json` and provide local values for storage and other secrets.
3. Start the Functions host:
   ```bash
   func start
   ```
4. Execute functions via HTTP using tools such as Postman or the provided admin front‑ends.

## Usage and Functionality
The primary API surface is exposed through HTTP triggers. Example payloads can be found in inline comments of each function. Typical usage:
```bash
# Create a new short URL
POST /api/UrlShortener
{
  "url": "https://contoso.com/landing",
  "title": "Campaign Landing Page",
  "vanity": "promo1"
}
```
See `doc/how-it-works.md` for detailed behaviour and additional examples.

## Deployment and Environments
This repo does not include CI/CD definitions. Deployments are normally run manually or integrated into client pipelines. The ARM template provisions a consumption‑plan Function App, Application Insights and storage accounts. Staging slots or additional environments can be added as needed.

## Support and Ownership
The project is maintained internally by the development team. Recent contributors include `Joel Jeffery` and `Frank`. For assistance contact the team through the usual internal channels.

## Licence
Internal proprietary codebase – not for redistribution.
