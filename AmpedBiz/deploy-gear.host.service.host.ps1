$env:username="`$ampbiz-api"
$env:password="qmLbwLTNRK3b87hksGgd75QmHdq7jQGdoBLtAi3p2MWKiKY14GcHlh1ekJy6"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy