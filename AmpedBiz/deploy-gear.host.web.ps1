cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="yw3JwdwM3bxrmsskWNDwG9vsLbuhP8vsPkmGrRszZQjijHntB9rXfzl0YrAm"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password