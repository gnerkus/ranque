@description('Base name of the resource such as web app name and app service plan ')
@minLength(2)
param webAppName string = 'nanotome-bookmark'

@description('Azure resource deployjment location.')
param location string = resourceGroup().location // Bicep function returning the resource group location

param linuxFxVersion string = 'DOTNETCORE:8.0'

param repositoryUrl string = 'https://github.com/gnerkus/bookmark'

@description('The text to replace the default subtitle with.')
param textToReplaceSubtitleWith string = 'Bookmarking app for studying OSS'

@description('Branch of the repository for deployment.')
param repositoryBranch string = 'master'

var webSiteName = '${webAppName}-api'
var appServicePlanName = toLower('AppServicePlan-${webAppName}')

// App Service Plan Creation
resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1'
  }
  kind: 'linux'
  properties: {
    reserved: false
  }
}

// Web App Creation
resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: webSiteName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
    }
  }
}

// Source Control Integration
resource srcControls 'Microsoft.Web/sites/sourcecontrols@2021-01-01' = {
  parent: appService
  name: 'web'
  properties: {
    repoUrl: repositoryUrl
    branch: repositoryBranch
    isManualIntegration: true
  }
}
