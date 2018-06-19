cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="4imgLh7bqyqimdqMhMr0R0gLtbZmCiZ49SDPMSr94ioBl7pB0B5Xzj9Zbgkn"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password