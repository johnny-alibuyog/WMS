$env:username="`$ampbiz-api"
$env:password="3isNu4MGtxmS3pSb4jqeslHsdnagHYgFszJ6TAuRK2ltnJkxL7BwedysfxLd"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy