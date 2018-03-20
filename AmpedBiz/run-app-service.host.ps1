$path = $PSScriptRoot + '\AmpedBiz.Service.Host'
$port = 49561

"=============================================="
" Building API "
"=============================================="
.\build.ps1


"=============================================="
" Building API "
"=============================================="
.\build.ps1 -target Seed-Data


"=============================================="
" Running API "
" 32-bit version C:\Program Files (x86)\IIS Express "
" 64-bit version C:\Program Files\IIS Express "
"=============================================="
cd '\Program Files\IIS Express'
.\iisexpress.exe -path:$path -port:$port