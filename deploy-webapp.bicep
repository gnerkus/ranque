@description('Base name of the resource such as web app name and app service plan ')
@minLength(2)
param webAppName string = 'nanotome-bookmark'

@description('Azure resource deployjment location.')
param location string = resourceGroup().location // Bicep function returning the resource group location

param sku string = 'F1'

param repositoryUrl string = 'https://github.com/gnerkus/bookmark'

@description('Branch of the repository for deployment.')
param repositoryBranch string = 'master'

var webSiteName = '${webAppName}-api'
var appServicePlanName = 'AppServicePlan-${webAppName}'

// App Service Plan Creation
resource asp 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: sku
  }
}

// Web App Creation
resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: webSiteName
  location: location
  identity: { type: 'SystemAssigned' }
  properties: {
    serverFarmId: asp.id
    httpsOnly: true
    siteConfig: {
      minTlsVersion: '1.2'
      scmMinTlsVersion: '1.2'
      ftpsState: 'FtpsOnly'
    }
  }
}

// Source Control Integration
resource gitsource 'Microsoft.Web/sites/sourcecontrols@2022-03-01' = {
  parent: webApp
  name: 'web'
  properties: {
    repoUrl: repositoryUrl
    branch: repositoryBranch
    isManualIntegration: true
  }
}
