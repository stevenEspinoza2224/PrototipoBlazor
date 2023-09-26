
sp_addlogin Prototipo,'P@ssw0rd2023##'

CREATE DATABASE Prototipo;
GO

CREATE DATABASE Prototipo
ON (NAME = 'Prototipo', FILENAME = 'C:\Users\SE8463NI\SQLArchivos\Prototipo.mdf')



USE Prototipo;
GO

CREATE TABLE Cliente(
	[IdCliente]          INT IDENTITY (1, 1) NOT NULL,
	[Categoria]          VARCHAR(50) NOT NULL,
	[NombreCliente]      VARCHAR(50) NOT NULL,
	[ApellidoCliente]    VARCHAR(50) NOT NULL,
	[Activo]			 BIT DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED ([IdCliente] ASC)
);


USE Prototipo

sp_adduser Prototipo,Prototipo

USE Prototipo;
GRANT SELECT, INSERT, UPDATE, DELETE TO Prototipo;

