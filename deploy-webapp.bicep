@description('Azure resource deployment location.')
param location string = resourceGroup().location // Bicep function returning the resource group location

@description('The text to replace the default subtitle with.')
param textToReplaceSubtitleWith string = 'Bookmarking app for studying OSS'

@description('Branch of the repository for deployment.')
param repositoryBranch string = 'master'

// App Service Plan Creation
resource appServicePlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: 'bookmarkAppServicePlan'
  location: location
  sku: {
    name: 'F1'
  }
  kind: 'app'
  properties: {
    reserved: false
  }
}

// Web App Creation
resource appService 'Microsoft.Web/sites@2020-12-01' = {
  name: 'nt-bookmark-api'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      appSettings: [
        {
          name: 'TEXT_TO_REPLACE_SUBTITLE_WITH' // This value needs to match the name of the environment variable in the application code
          value: textToReplaceSubtitleWith
        }
        {
          name: 'SCM_DO_BUILD_DURING_DEPLOYMENT' // Build the application during deployment
          value: 'true'
        }
        {
          name: 'WEBSITE_NODE_DEFAULT_VERSION' // Set the default node version
          value: '~20'
        }
      ]
      publicNetworkAccess: 'Enabled'
    }
  }
}

// Source Control Integration
resource srcControls 'Microsoft.Web/sites/sourcecontrols@2021-01-01' = {
  parent: appService
  name: 'web'
  properties: {
    repoUrl: 'https://192.168.2.41:3000/nanotome/bookmark'
    branch: repositoryBranch
    isManualIntegration: true
  }
}
