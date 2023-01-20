@description('Name used as base-template to name the resources to be deployed in Azure.')
param baseName string = 'ShortenerTools'

@description('Default URL used when key passed by the user is not found.')
param defaultRedirectUrl string = 'https://azure.com'

@description('An array of locations that the Function App will be deployed to.  Defaults to one region, East US')
param regions array = [
  'eastus'
]

@description('Name of the Azure Front Door resource to deploy')
param frontDoorName string = '${baseName}-afd}'

@description('Set to true to deploy the Azure Front Door resource, otherwise false to not deploy AFD')
param deployFrontDoor bool = false

param buildId string

@description('The location of the storage account that will be used to store the URL data.')
param sharedResourceRegion string = 'eastus'

var UrlsStorageAccountName = '${baseName}urlstorage'
var functionAcount = length(regions)
var appInsightsDeploymentName = '${baseName}-${sharedResourceRegion}-ai-${buildId}'
var functionAppNames = [for location in regions: '${baseName}-${location}']
var frontDoorDeploymentName= '${baseName}-${frontDoorName}-fd-${buildId}'
var cosmosAccountName = '${baseName}-cdb'
var cosmosDeploymentName = '${cosmosAccountName}-${buildId}'

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
    appInsightsName: '${baseName}-ai'
    logAnalyticsWorkspaceName: '${baseName}-la'
    location: sharedResourceRegion
  }
}

module functionApps './modules/functionApp.bicep' = [for (location, i) in regions: {
  name: '${baseName}-${location}-fa-${buildId}'
  params: {
    functionAppName: functionAppNames[i]
    storageAccountName: '${baseName}fa${location}'
    appInsightsName: appInsights.outputs.name
    defaultRedirectUrl: defaultRedirectUrl
    location: location
    cosmosDbAccountName: cosmosAccountName
    cosmosDbApiVersion: cosmosDb.outputs.apiVersion
    cosmosDbResourceId: cosmosDb.outputs.id
  }
}]

module frontDoor './modules/frontDoor.bicep' = if (deployFrontDoor) {
  name: frontDoorDeploymentName
  params: {
    frontDoorName: frontDoorName
    functionAppHostNames: functionAppNames
  }
  dependsOn: [
    functionApps
  ]
}

module cosmosDb './modules/cosmosDb.bicep' = {
  name: cosmosDeploymentName
  params: {
    cosmosAccountName: cosmosAccountName
    regions: regions
  }
}

output functionAppNames array = [for i in range(0, functionAcount): '${functionApps[i].name}']
