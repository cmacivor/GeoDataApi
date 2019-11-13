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

#Write-Output $model.AppPool

#Write-Output $model.Application

#Write-Output $model.ParamFile

#Write-Output $(Join-Path $WebDeployRoot $DevParamFile)

Write-Output "testing functions"

$PackagePath = Get-CorPackagePath $model.Application $model.Version -UseDevPackageSource

Write-Output $PackagePath

$deploymentParams = Get-WDParameters $model.ParamFile

Write-Output $deploymentParams

$applicationPath = Get-CorApplicationPath $model.ParamFile

#Write-Output $applicationPath

Write-Output $model.Site

$publishSettings = Get-CorPublishSettings $model.Site


Write-Output $publishSettings

Restore-WDPackage $PackagePath -DestinationPublishSettings $publishSettings -Parameters @{ 
	"IIS Web Application Name"="starappdev/services/geodataapi"
	"AddressCandidatesApiUrl" = "https://gisdev.richmondgov.com/arcgis/rest/services/Geocode/RichmondAddress/GeocodeServer/findAddressCandidates?Street=&ZIP=&Single+Line+Input={0}&category=&outFields=*&maxLocations=&outSR=&searchExtent=&location=&distance=&magicKey=&f=pjson"
	"CommonBoundariesApiUrl" = "https://gisdev.richmondgov.com/arcgis/rest/services/StatePlane4502/CommonBoundaries/MapServer/identify"
	"MapServerApiUrl" = "https://gisdev.richmondgov.com/arcgis/rest/services/StatePlane4502/Addresses/MapServer/0/query?where=AddressLabel={0}&outFields=*&returnGeometry=false&returnIdsOnly=false&f=json"
}



#New-CorDeploymentConfig $applicationName (Join-Path $WebDeployRoot $DevParamFile) $site $appPool | Publish-CorWebApp -UseDevPackageSource