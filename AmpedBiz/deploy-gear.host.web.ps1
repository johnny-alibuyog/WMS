cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="MWtRomtQvdzr39nsNDa6vhRiAdPLtFg3t9s0pok6FLla5rhZeH6W3EesNNAn"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password