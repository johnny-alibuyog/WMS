cd .\AmpedBiz.Web\

$env:host="ftp.gear.host"
$env:folder="/site/wwwroot"
$env:username="ampbiz"
$env:password="Pz700Rt22kL3scoQ0WfGkQYqkEDSdDTvXKH41WdT4kFCb8Js1D4JtqF6WdBl"

gulp deploy --host $env:host --folder $env:folder --username $env:username --password $env:password