$env:username="`$ampbiz-api"
$env:password="CA1Ky9AEAXTgZL0GX28TtrGYzzlyRAr6zoyvK8L5C72Xfh7CzPmrZpsDsqoY"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy