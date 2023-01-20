param frontDoorName string
param functionAppHostNames array

var profileName = '${frontDoorName}-profile'
var endpointName = '${frontDoorName}-endpoint'
var originName = '${frontDoorName}-origingroup'

var hostNames = [for functionApp in functionAppHostNames: '${functionApp}-fa.azurewebsites.net']

resource frontDoorProfile 'Microsoft.Cdn/profiles@2022-11-01-preview' = {
  name: profileName
  location: 'global'
  sku: {
    name: 'Premium_AzureFrontDoor'
  }
}

resource frontDoorEndpoint 'Microsoft.Cdn/profiles/afdEndpoints@2022-11-01-preview' = {
  name: endpointName
  parent: frontDoorProfile
  location: 'global'
  properties: {
    enabledState: 'Enabled'
  }
}

resource originGroup 'Microsoft.Cdn/profiles/originGroups@2022-11-01-preview' = {
  name: originName
  parent: frontDoorProfile
  properties: {
    loadBalancingSettings: {
      sampleSize: 4
      successfulSamplesRequired: 2
    }
    healthProbeSettings: {
      probePath: '/'
      probeRequestType: 'GET'
      probeProtocol: 'Https'
      probeIntervalInSeconds: 30
    }
  }
}

resource frontDoorOrigin 'Microsoft.Cdn/profiles/originGroups/origins@2022-11-01-preview' = [for (hostName, i) in hostNames: {
  name: functionAppHostNames[i]
  parent: originGroup
  properties: {
    hostName: hostName
    httpPort: 80
    httpsPort: 443
    originHostHeader: hostName
    priority: 1
    weight: 1000
  }
}]

resource frontDoorRoute 'Microsoft.Cdn/profiles/afdEndpoints/routes@2022-11-01-preview' = {
  name: 'urlshortner'
  parent: frontDoorEndpoint
  properties: {
    originGroup: {
      id: originGroup.id
    }
    supportedProtocols: [
      'Http'
      'Https'
    ]
    patternsToMatch: [
      '/*'
    ]
    forwardingProtocol: 'HttpOnly'
    linkToDefaultDomain: 'Enabled'
    httpsRedirect: 'Enabled'
    cacheConfiguration: {
      queryStringCachingBehavior: 'IgnoreQueryString'
    }
  }
  dependsOn: [
    frontDoorOrigin
  ]
}

output frontDoorId string = frontDoorProfile.properties.frontDoorId
