IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ics.employees')
BEGIN
EXEC('CREATE DATABASE [ics.employees]')
END
GO

USE [ics.employees]
GO

CREATE TABLE [Positions](

	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,

	CONSTRAINT [PK_Positions] PRIMARY KEY ([Id])
)
GO

CREATE TABLE [Employees](
  
    [Id] INT IDENTITY(1,1) NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [PositionId] INT NOT NULL,
    [BirthYear] INT NOT NULL,
    [Salary] DECIMAL(18,2) NOT NULL,

	CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),

	CONSTRAINT [FK_Employees_Positions_PositionId] FOREIGN KEY ([PositionId]) REFERENCES [Positions] ([Id])
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
)
GO
