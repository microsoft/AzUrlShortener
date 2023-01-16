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
  }
}

output id string = cosmosAccount.id
output apiVersion string = cosmosAccount.apiVersion
