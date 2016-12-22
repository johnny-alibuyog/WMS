$env:username="`$ampbiz-api"
$env:password="ZHPsTLr8JReX0G8g0TwPm06AfGfpm4cX2t4kPh0ofAl5LTg3xiGn1B9FLSwM"
$env:publish_profile="ampbiz-api.staging.gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy