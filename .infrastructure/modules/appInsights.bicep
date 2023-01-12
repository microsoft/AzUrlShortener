param appInsightsName string
param location string

resource insightsApp 'microsoft.insights/components@2015-05-01' = {
  name: appInsightsName
  location: location
  kind: ''
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

output name string = insightsApp.name
