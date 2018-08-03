$env:username="`$ampbiz-api"
$env:password="F1d1mtdk0YkA2wKSTMsQS29hz58SSj65S4Ln7MJS6m9FHnruxFyL4zeDYl4p"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy