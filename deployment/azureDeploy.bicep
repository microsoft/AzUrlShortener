@description('Name used as base-template to name the resources to be deployed in Azure.')
param baseName string = 'ShortenerTools'

@description('Default URL used when key passed by the user is not found.')
param defaultRedirectUrl string = 'https://azure.com'

@description('The URL of GitHub (ending by .git)')
param GitHubURL string = 'https://github.com/fboucher/AzUrlShortener.git'

@description('Name of the branch to use when deploying (Default = main).')
param GitHubBranch string = 'main'

@description('Just a text value (format: yyyy-MM-dd) that express when it is safe to delete these resources')
param ExpireOn string = utcNow('yyyy-MM-dd')

@description('Owner of this deployment, person to contact for question.')
param OwnerName string = ''

var suffix = substring(toLower(uniqueString(resourceGroup().id, resourceGroup().location)), 0, 5)
var funcAppName = toLower(concat(baseName, suffix))
var funcStorageAccountName = toLower('${substring(baseName, 0, min(length(baseName), 16))}${suffix}stg')
var funcHhostingPlanName = '${substring(baseName, 0, min(length(baseName), 14))}${suffix}plan'
var insightsAppName = '${substring(baseName, 0, min(length(baseName), 14))}${suffix}-meta'
var functionProjectFolder = 'src'
var UrlsStorageAccountName = 'urldata${suffix}stg'

resource funcApp 'Microsoft.Web/sites@2022-03-01' = {
  name: funcAppName
  kind: 'functionapp'
  location: resourceGroup().location
  tags: {
    Owner: OwnerName
    ExpireOn: ExpireOn
  }
  properties: {
    name: funcAppName
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: reference('microsoft.insights/components/${insightsAppName}', '2015-05-01').InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: reference('microsoft.insights/components/${insightsAppName}', '2015-05-01').ConnectionString
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${funcStorageAccountName};AccountKey=${listKeys(funcStorageAccount.id, '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${funcStorageAccountName};AccountKey=${listKeys(funcStorageAccount.id, '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: '${funcAppName}ba91'
        }
        {
          name: 'UlsDataStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${UrlsStorageAccountName};AccountKey=${listKeys(UrlsStorageAccount.id, '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'defaultRedirectUrl'
          value: defaultRedirectUrl
        }
      ]
    }
    serverFarmId: funcHhostingPlan.id
    use32BitWorkerProcess: true
    netFrameworkVersion: 'v6.0'
    clientAffinityEnabled: true
  }
  dependsOn: [
    insightsApp

  ]
}

resource funcAppName_web 'Microsoft.Web/sites/sourcecontrols@2022-03-01' = {
  parent: funcApp
  name: 'web'
  properties: {
    repoUrl: GitHubURL
    branch: GitHubBranch
    isManualIntegration: true
  }
}

resource funcHhostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: funcHhostingPlanName
  location: resourceGroup().location
  kind: ''
  tags: {
    Owner: OwnerName
    ExpireOn: ExpireOn
  }
  properties: {
    name: funcHhostingPlanName
    computeMode: 'Dynamic'
  }
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

resource insightsApp 'microsoft.insights/components@2015-05-01' = {
  name: insightsAppName
  location: resourceGroup().location
  tags: {
    Owner: OwnerName
    ExpireOn: ExpireOn
  }
  kind: ''
  properties: {
    Application_Type: 'web'
    ApplicationId: funcAppName
    Request_Source: 'rest'
  }
}

resource funcStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: funcStorageAccountName
  location: resourceGroup().location
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  properties: {
    supportsHttpsTrafficOnly: true
  }
}

resource UrlsStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: UrlsStorageAccountName
  location: resourceGroup().location
  tags: {
    displayName: UrlsStorageAccountName
    Owner: OwnerName
    ExpireOn: ExpireOn
  }
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
}