$env:username="`$ampbiz-api"
$env:password="7XhRchMowBKY9feulRobaCkSnBE5FDi8ZL55nedcp6ESAu7YmCcfFhEvrRlm"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy