<#
  This script deploys a package in the development-only user profile source to a remote development web server.
  Deployment Config is created here without reference to a CSV file. This is only allowed for dev web sites.
  Application parameters are still referenced from the source-controlled location, so get latest as needed from TFS. 
  
  The application deployed here is: 
  
  Name: GeoDataApi
  Team: Services 
#>

#Requires –Modules CorDeploy

$applicationName = "GeoDataApi"
$site = "starappdev"
$appPool = "starapp_CommunityServices_4.0"
$DevParamFile = "Parameters\Development\CommunityServices\GeoDataApi.xml"

Import-Module CorDeploy

$model = New-CorDeploymentConfig $applicationName (Join-Path $WebDeployRoot $DevParamFile) $site $appPool

$PackagePath = Get-CorPackagePath $model.Application $model.Version -UseDevPackageSource

#Write-Output $PackagePath

#this is the contents of the external .xml file
$deploymentParams = Get-WDParameters $model.ParamFile


$publishSettings = Get-CorPublishSettings $model.Site


#Write-Output $publishSettings

Write-Output $deploymentParams


Restore-WDPackage $PackagePath -DestinationPublishSettings $publishSettings -Parameters @{ 
	"IIS Web Application Name"= $deploymentParams.IISWebApplicationName
	"AddressCandidatesApiUrl" = $deploymentParams.AddressCandidatesApiUrl
	"CommonBoundariesApiUrl" = $deploymentParams.CommonBoundariesApiUrl
	"MapServerApiUrl" = $deploymentParams.MapServerApiUrl
	"ELMAHApplicationName" = $deploymentParams.ELMAHApplicationName
	"ELMAHEmailSenderAlias" = $deploymentParams.ELMAHEmailSenderAlias
	"ELMAHEmailRecipients" = $deploymentParams.ELMAHEmailRecipients
	"ELMAHEmailSubject" = $deploymentParams.ELMAHEmailSubject
}

#passing parameters like this is necessary
#Restore-WDPackage $PackagePath -DestinationPublishSettings $publishSettings -Parameters @{ 
#	"IIS Web Application Name"="starappdev/services/geodataapi"
#	"AddressCandidatesApiUrl" = "https://gisdev.richmondgov.com/arcgis/rest/services/Geocode/RichmondAddress/GeocodeServer/findAddressCandidates?Street=&ZIP=&Single+Line+Input={0}&category=&outFields=*&maxLocations=&outSR=&searchExtent=&location=&distance=&magicKey=&f=pjson"
#	"CommonBoundariesApiUrl" = "https://gisdev.richmondgov.com/arcgis/rest/services/StatePlane4502/CommonBoundaries/MapServer/identify"
#	"MapServerApiUrl" = "https://gisdev.richmondgov.com/arcgis/rest/services/StatePlane4502/Addresses/MapServer/0/query?where=AddressLabel={0}&outFields=*&returnGeometry=false&returnIdsOnly=false&f=json"
#}



#New-CorDeploymentConfig $applicationName (Join-Path $WebDeployRoot $DevParamFile) $site $appPool | Publish-CorWebApp -UseDevPackageSource