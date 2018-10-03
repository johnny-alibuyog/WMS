mysql -u root -p ampedbizdb < 1_upgrade_script_ddl_pre.sql
mysql -u root -p ampedbizdb < 2_upgrade_script_dml.sql
mysql -u root -p ampedbizdb < 3_upgrade_script_ddl_post.sql