$env:username="`$ampbiz-api"
$env:password="i1JRyJXk4Zvl1jxnp0qwDMlkmD6Gw2Amce00HyHew1qQL7HFQpBPcbhqmwQe"
$env:publish_profile="ampbiz-api.staging.gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy