cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="RTYC18aGXg1eceupwFccP8MZPokkQiMihlKY32ciduZtNARGs1YpZuRgasLk"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password