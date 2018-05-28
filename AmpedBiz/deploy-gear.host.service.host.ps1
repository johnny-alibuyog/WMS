$env:username="`$ampbiz-api"
$env:password="j9Bct7XYXbtwhboZgsAtYwQL7fmcjyfEXzrQeE9RKfEKBgek44LuSz0eNQzs"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy