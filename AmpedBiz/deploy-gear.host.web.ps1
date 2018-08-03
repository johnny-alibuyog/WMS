cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="65to4ldYZFmaubEeBhTn97aow7aXbWo07EAdWHYGHTdgMCoTz4Zrl2wKBFYv"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password