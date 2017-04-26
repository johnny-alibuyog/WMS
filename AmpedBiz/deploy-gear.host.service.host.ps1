$env:username="`$ampbiz-api"
$env:password="0zv0mMcLkuyJpQkyfQfNmWAtnzXQdJnZi7CzHbGSlFkYdz0fEW6rtQF8LJq4"
$env:publish_profile="staging-gear.host.pubxml"
$env:database_config="database.staging.gear.host.config.json"

.\build.ps1 -target Deploy