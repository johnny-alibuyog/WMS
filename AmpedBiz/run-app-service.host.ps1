$path = $PSScriptRoot + '\AmpedBiz.Service.Host'
$port = 49561

"=============================================="
" Building API "
"=============================================="
.\build.ps1

"=============================================="
" Running API "
"=============================================="
cd '\Program Files\IIS Express'
.\iisexpress.exe -path:$path -port:$port