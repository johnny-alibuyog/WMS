$env:source_path = "./AmpedBiz.Service.Host/obj/Release/Package/AmpedBiz.Service.Host.zip"
$env:publish_url = "publish.gear.host"
$env:site_name = "ampbiz-api"
$env:username = "$ampbiz-api"
$env:password = "ZHPsTLr8JReX0G8g0TwPm06AfGfpm4cX2t4kPh0ofAl5LTg3xiGn1B9FLSwM"

.\build.ps1 -target Deploy