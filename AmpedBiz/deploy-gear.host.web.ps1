cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="p5a3TseMgd3pGAkue8R4rwJb082jwcm51uDtf7gadmjLJ7sBQ91GhcEe6FaS"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password