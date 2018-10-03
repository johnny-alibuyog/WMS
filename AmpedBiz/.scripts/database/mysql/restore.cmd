set User=root
set Password=123!@#qwe
set Host=localhost
set Database=ampedbizdb
set BackupFile=C:\_repo\Narasoft\tinda\backup\ampedbizdb.20181109.sql

mysql --user=%User% --password=%Password% --host=%Host% %Database% ^< %BackupFile%