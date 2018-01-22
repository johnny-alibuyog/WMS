cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="5zCGATrSwM1sXi5tNPjfiBo8wAqshvtaZgfduMYdlcb6j9SHwJuBGzBMeGn1"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password