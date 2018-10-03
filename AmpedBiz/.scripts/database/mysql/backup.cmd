set User=root
set Password=123!@#qwe
set Host=localhost
set Database=ampedbizdb
set Port=3306
set BackupLocaltion=C:\_repo\Narasoft\tinda\backup\

mysqldump.exe --user=%User% --password=%Password%  --host=%Host% --port=%Port% --result-file="%BackupLocaltion%%Database%.%date:~10,4%%date:~7,2%%date:~4,2%.sql" --default-character-set=utf8 --single-transaction=TRUE --databases "%Database%"