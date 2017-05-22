cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="G0NeXBT7RibttPvr49i3fczn01fo9CJFGAMzEufgoZWgzGbndtGwFfDlZnXy"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password