$deployName = Get-Date -Format "yyyyMMddHHmmss"
$resourceGroup = "URL-SHORTENER"

az deployment group create -n $deployName -g $resourceGroup --template-file .\main.bicep --parameters '@.\parameters\development\parameters.json'