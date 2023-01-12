@description('Name used as base-template to name the resources to be deployed in Azure.')
param baseName string = 'ShortenerTools'

@description('Default URL used when key passed by the user is not found.')
param defaultRedirectUrl string = 'https://azure.com'

@description('An array of locations that the Function App will be deployed to.  Defaults to one region, East US')
param regions array = [
  'eastus'
]

param frontDoorName string

param deploymentTime string = utcNow('yyyy-MM-dd-HH-mm-ss')

@description('The location of the storage account that will be used to store the URL data.')
param sharedResourceRegion string = 'eastus'

var suffix = substring(toLower(uniqueString(resourceGroup().id, resourceGroup().location)), 0, 5)
var UrlsStorageAccountName = 'urldata${suffix}stg'
var functionAcount = length(regions)
var appInsightsDeploymentName = '${baseName}--${sharedResourceRegion}-ai-${deploymentTime}'
var functionAppNames = [for location in regions: '${baseName}-${location}-${uniqueString('${baseName}${location}')}']
var frontDoorDeploymentName= '${baseName}--${frontDoorName}-fd-${deploymentTime}'

resource UrlsStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: UrlsStorageAccountName
  location: sharedResourceRegion
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

module appInsights './modules/appInsights.bicep' = {
  name: appInsightsDeploymentName
  params: {
    appInsightsName: '${baseName}-${sharedResourceRegion}-${uniqueString('${baseName}${sharedResourceRegion}')}-ai'
    location: sharedResourceRegion
  }
}

module functionApps './modules/functionApp.bicep' = [for (location, i) in regions: {
  name: '${baseName}-${location}-${uniqueString('${baseName}${location}')}'
  params: {
    functionAppName: functionAppNames[i]
    storageAccountName: '${baseName}${location}'
    appInsightsName: appInsights.outputs.name
    urlsStorageAccountName: UrlsStorageAccount.name
    defaultRedirectUrl: defaultRedirectUrl
    location: location
  }
}]

module frontDooor './modules/frontDoor.bicep' = {
  name: frontDoorDeploymentName
  params: {
    frontDoorName: frontDoorName
    functionAppHostNames: functionAppNames
  }
  dependsOn: [
    functionApps
  ]
}

output functionAppNames array = [for i in range(0, functionAcount): '${functionApps[i].name}']
