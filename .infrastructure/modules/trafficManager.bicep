param trafficManagerProfileName string
param functionAppNames array
param functionRegions array
// Create a Azure Traffic Manager profile with the name specified by trafficManagerProfileName.
// The profile should have an array of endpoints that are Azure Function resources.

var endpoints = [for (name, i) in functionAppNames: {
  name: name
  type: 'Microsoft.Network/trafficManagerProfiles/azureEndpoints'
  properties: {
    targetResourceId: '/subscriptions/${subscription().subscriptionId}/resourceGroups/${resourceGroup().name}/providers/Microsoft.Web/sites/${name}-fa'
    endpointStatus: 'Enabled'
    priority: i+1
    endpointLocation: functionRegions[i]
    weight: 1
  }
}]

resource profile 'Microsoft.Network/trafficmanagerprofiles@2022-04-01-preview' = {
  name: trafficManagerProfileName
  location: 'global'
  properties: {
    profileStatus: 'Enabled'
    trafficRoutingMethod: 'Performance'
    dnsConfig: {
      relativeName: trafficManagerProfileName
      ttl: 60
    }
    monitorConfig: {
      protocol: 'https'
      port: 443
      path: '/'
    }
    endpoints: endpoints
  }
}
