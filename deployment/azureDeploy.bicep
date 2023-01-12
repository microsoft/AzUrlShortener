@description('Name used as base-template to name the resources to be deployed in Azure.')
param baseName string = 'ShortenerTools'

@description('Default URL used when key passed by the user is not found.')
param defaultRedirectUrl string = 'https://azure.com'

@description('An array of locations that the Function App will be deployed to.  Defaults to one region, East US')
param regions array = [
  'eastus'
]

@description('The location of the storage account that will be used to store the URL data.')
param urlStorageLocation string = 'eastus'

var suffix = substring(toLower(uniqueString(resourceGroup().id, resourceGroup().location)), 0, 5)
//var funcAppName = toLower(concat(baseName, suffix))
//var funcStorageAccountName = toLower('${substring(baseName, 0, min(length(baseName), 16))}${suffix}stg')
//var funcHhostingPlanName = '${substring(baseName, 0, min(length(baseName), 14))}${suffix}plan'
//var insightsAppName = '${substring(baseName, 0, min(length(baseName), 14))}${suffix}-meta'
var UrlsStorageAccountName = 'urldata${suffix}stg'

//var storageAccountNames = [for (location, i )in regions: '${baseName}${location}${i}']

// resource funcApp 'Microsoft.Web/sites@2022-03-01' = [for (location, i) in regions: {
//   name: '${funcAppName}-${location}-${i}'
//   kind: 'functionapp'
//   location: location
//   properties: {
//     name: funcAppName
//     siteConfig: {
//       appSettings: [
//         {
//           name: 'FUNCTIONS_EXTENSION_VERSION'
//           value: '~4'
//         }
//         {
//           name: 'FUNCTIONS_WORKER_RUNTIME'
//           value: 'dotnet-isolated'
//         }
//         {
//           name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
//           value: reference('microsoft.insights/components/${insightsAppName}', '2015-05-01').InstrumentationKey
//         }
//         {
//           name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
//           value: reference('microsoft.insights/components/${insightsAppName}', '2015-05-01').ConnectionString
//         }
//         {
//           name: 'AzureWebJobsStorage'
//           value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountNames[i]};AccountKey=${listKeys(resourceId('Microsoft.Storage/storageAccounts', storageAccountNames[i]), '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
//         }
//         {
//           name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
//           value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountNames[i]};AccountKey=${listKeys(resourceId('Microsoft.Storage/storageAccounts', storageAccountNames[i]), '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
//         }
//         {
//           name: 'WEBSITE_CONTENTSHARE'
//           value: '${funcAppName}ba91'
//         }
//         {
//           name: 'UlsDataStorage'
//           value: 'DefaultEndpointsProtocol=https;AccountName=${UrlsStorageAccountName};AccountKey=${listKeys(UrlsStorageAccount.id, '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
//         }
//         {
//           name: 'defaultRedirectUrl'
//           value: defaultRedirectUrl
//         }
//       ]
//     }
//     serverFarmId: funcHhostingPlan[i].id
//     use32BitWorkerProcess: true
//     netFrameworkVersion: 'v6.0'
//     clientAffinityEnabled: true
//   }
//   // dependsOn: [
//   //   insightsApp

//   // ]
// }]

// resource funcHhostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = [for (location, i) in regions: {
//   name: '${funcHhostingPlanName}-${location}-${i}'
//   location: location
//   kind: 'functionapp'
//   properties: {
//   }
//   sku: {
//     tier: 'Dynamic'
//     name: 'Y1'
//   }
// }]

// resource insightsApp 'microsoft.insights/components@2015-05-01' = {
//   name: insightsAppName
//   location: resourceGroup().location
//   tags: {
//     Owner: OwnerName
//     ExpireOn: ExpireOn
//   }
//   kind: ''
//   properties: {
//     Application_Type: 'web'
//     ApplicationId: funcAppName
//     Request_Source: 'rest'
//   }
// }


// resource funcStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = [for (location, i) in regions:{
//   name: '${toLower(funcStorageAccountName)}${toLower(location)}${i}}'
//   location: location
//   kind: 'StorageV2'
//   sku: {
//     name: 'Standard_LRS'
//   }
//   properties: {
//     supportsHttpsTrafficOnly: true
//   }
// }]

resource UrlsStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: UrlsStorageAccountName
  location: urlStorageLocation
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

module functionApp './modules/functionApp.bicep' = [for location in regions: {
  name: '${baseName}-${location}-${uniqueString(concat(baseName, location))}'
  params: {
    functionAppName: '${baseName}-${location}-${uniqueString(concat(baseName, location))}'
    storageAccountName: '${baseName}${location}${uniqueString(concat(baseName, location))}'
    appInsightsName: '${baseName}-${location}-${uniqueString(concat(baseName, location))}-ai'
    urlsStorageAccountName: UrlsStorageAccount.name
    defaultRedirectUrl: defaultRedirectUrl
    location: location
  }
}]
