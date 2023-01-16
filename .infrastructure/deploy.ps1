$deployName = Get-Date -Format "yyyyMMddHHmmss"

az deployment group create -n $deployName -g $resourceGroup --template-file .\main.bicep --parameters '@.\parameters\development\parameters.json'