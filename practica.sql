create table logi(
ID_logi serial not null constraint PK_ID_logi primary key,
adress varchar not null,
vrema timestamp with time zone not null,
http_method varchar not null,
http_status int not null
);
create table users(
ID_users serial not null constraint PK_ID_users primary key,
name_user varchar not null,
password_user varchar not null
);

select*from logi;