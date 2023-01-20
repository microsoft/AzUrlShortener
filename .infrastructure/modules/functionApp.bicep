param functionAppName string
param storageAccountName string
param appInsightsName string
param defaultRedirectUrl string
param cosmosDbAccountName string
param cosmosDbResourceId string
param cosmosDbApiVersion string
// param frontDoorId string
param location string

var cosmosDbKey = listKeys(cosmosDbResourceId, cosmosDbApiVersion).primaryMasterKey
var tableEndpoint = 'https://${cosmosDbAccountName}.table.cosmos.azure.com:443/'
var cosmosDbConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${cosmosDbAccountName};AccountKey=${cosmosDbKey};TableEndpoint=${tableEndpoint}'

resource funcStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    supportsHttpsTrafficOnly: true
  }
}

resource funcHhostingPlan 'Microsoft.Web/serverfarms@2021-03-01' ={
  name: '${functionAppName}-asp'
  location: location
  kind: 'functionapp'
  properties: {
  }
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

resource funcApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${functionAppName}-fa'
  kind: 'functionapp'
  location: location
  properties: {
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
          value: reference('microsoft.insights/components/${appInsightsName}', '2015-05-01').InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: reference('microsoft.insights/components/${appInsightsName}', '2015-05-01').ConnectionString
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${funcStorageAccount.name};AccountKey=${listKeys(resourceId('Microsoft.Storage/storageAccounts', funcStorageAccount.name), '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${funcStorageAccount.name};AccountKey=${listKeys(resourceId('Microsoft.Storage/storageAccounts', funcStorageAccount.name), '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: '${functionAppName}ba91'
        }
        {
          name: 'UlsDataStorage'
          value: cosmosDbConnectionString
        }
        {
          name: 'defaultRedirectUrl'
          value: defaultRedirectUrl
        }
      ]
      // ipSecurityRestrictions: [
      //   {
      //     tag: 'ServiceTag'
      //     ipAddress: 'AzureFrontDoor.Backend'
      //     action: 'Allow'
      //     priority: 100
      //     headers: {
      //       'x-azure-fdid': [
      //         frontDoorId
      //       ]
      //     }
      //     name: 'Allow Azure Front Door'
      //   }
      // ]
    }
    serverFarmId: funcHhostingPlan.id
    use32BitWorkerProcess: true
    netFrameworkVersion: 'v6.0'
    clientAffinityEnabled: true
    httpsOnly: true
  }
}
