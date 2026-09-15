use master;
go

if not exists(select * from sys.databases where name = 'PostCacheAPIDB') 
	create database PostCacheAPIDB;
go

use PostCacheAPIDB;
go

create table dbo.PostCache (
	Id int identity(1,1) primary key,
	UserId int not null,
	Title nvarchar(255) not null,
	Body nvarchar(max) not null,
	CreatedAt datetime2 default sysutcdatetime()
);
go