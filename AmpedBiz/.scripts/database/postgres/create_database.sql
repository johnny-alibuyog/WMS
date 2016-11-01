--create schema dbo;

create database ampedbizdb
   with owner postgres 
   template template0
   encoding 'SQL_ASCII'
   tablespace  pg_default
   lc_collate  'C'
   lc_ctype  'C'
   connection limit  -1;