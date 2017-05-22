$env:username="`$ampbiz-api"
$env:password="iQ3D0FfLBETDGzA7RWmWGX6DiGYMf7wqfia5raZrnWdbwkJg7Zij0v9c2fvt"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy