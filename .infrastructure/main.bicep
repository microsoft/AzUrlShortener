@description('Name used as base-template to name the resources to be deployed in Azure.')
param baseName string = 'ShortenerTools'

@description('Default URL used when key passed by the user is not found.')
param defaultRedirectUrl string = 'https://azure.com'

@description('An array of locations that the Function App will be deployed to.  Defaults to one region, East US')
param regions array = [
  'eastus'
]

@description('The location of the storage account that will be used to store the URL data.')
param sharedResourceRegion string = 'eastus'

var suffix = substring(toLower(uniqueString(resourceGroup().id, resourceGroup().location)), 0, 5)
var UrlsStorageAccountName = 'urldata${suffix}stg'

resource UrlsStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: UrlsStorageAccountName
  location: sharedResourceRegion
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

module appInsights './modules/appInsights.bicep' = {
  name: 'appInsights-${deployment().name}'
  params: {
    appInsightsName: '${baseName}-${sharedResourceRegion}-${uniqueString('${baseName}${sharedResourceRegion}')}-ai'
    location: sharedResourceRegion
  }
}

module functionApp './modules/functionApp.bicep' = [for location in regions: {
  name: '${baseName}-${location}-${uniqueString('${baseName}${location}')}'
  params: {
    functionAppName: '${baseName}-${location}-${uniqueString('${baseName}${location}')}'
    storageAccountName: '${baseName}${location}'
    appInsightsName: appInsights.name
    urlsStorageAccountName: UrlsStorageAccount.name
    defaultRedirectUrl: defaultRedirectUrl
    location: location
  }
}]
