param appInsightsName string
param logAnalyticsWorkspaceName string
param location string

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
}

resource insightsApp 'Microsoft.Insights/components@2020-02-02' ={
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
    WorkspaceResourceId: logAnalytics.id
  }
}

output name string = insightsApp.name
output id string = insightsApp.id
