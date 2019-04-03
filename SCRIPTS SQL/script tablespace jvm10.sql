create tablespace TBSJVM datafile 'C:\app\Erik\oradata\produ\dtfappjvm.dbf' size 1024m extent management local segment space management auto;

create user jvm10 identified by jvm10 default tablespace tbsjvm temporary tablespace temp account unlock;

grant connect, resource to jvm10;
grant alter any index to jvm10;
grant alter any sequence to jvm10;
grant alter any table to jvm10;
grant alter any trigger to jvm10;
grant alter any procedure to jvm10;
grant create any index to jvm10;
grant create any sequence to jvm10;
grant create any synonym to jvm10;
grant create any table to jvm10;
grant create any trigger to jvm10;
grant create any view to jvm10;
grant create procedure to jvm10;
grant create public synonym to jvm10;
grant create trigger to jvm10;
grant create view to jvm10;
grant delete any table to jvm10;
grant drop any index to jvm10;
grant drop any sequence to jvm10;
grant drop any table to jvm10;
grant drop any trigger to jvm10;
grant drop any view to jvm10;
grant insert any table to jvm10;
grant query rewrite to jvm10;
grant select any table to jvm10;
grant unlimited tablespace to jvm10;

grant execute any procedure to jvm10;

select * from dba_profiles where resource_name like 'PASSWORD_LIFE_TIME';

alter profile default limit password_life_time unlimited;