param cosmosAccountName string
param regions array

var locations = [for region in regions: {
  locationName: region
  failoverPriority: indexOf(regions, region)
  isZoneRedundant: false
}]

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2022-08-15' = {
  name: cosmosAccountName
  location: regions[0]
  tags: {
    defaultExperience: 'Azure Table'
  }
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    publicNetworkAccess: 'Enabled'
    enableMultipleWriteLocations: length(regions) > 1
    enableAutomaticFailover: length(regions) > 1
    consistencyPolicy: {
      defaultConsistencyLevel: 'BoundedStaleness'
      maxIntervalInSeconds: 86400
      maxStalenessPrefix: 1000000
    }
    locations: locations
    capabilities: [
      {
        name: 'EnableTable'
      }
    ]
    backupPolicy: {
      type: 'Periodic'
      periodicModeProperties: {
        backupIntervalInMinutes: 360
        backupRetentionIntervalInHours: 24
        backupStorageRedundancy: 'Local'
      }
    }
    ipRules: [
      {
        ipAddressOrRange: '104.42.195.92'
      }
      {
        ipAddressOrRange: '40.76.54.131'
      }
      {
        ipAddressOrRange: '52.176.6.30'
      }
      {
        ipAddressOrRange: '52.169.50.45'
      }
      {
        ipAddressOrRange: '52.187.184.26'
      }
      {
        ipAddressOrRange: '0.0.0.0'
      }
    ]
  }
}

output id string = cosmosAccount.id
output apiVersion string = cosmosAccount.apiVersion
