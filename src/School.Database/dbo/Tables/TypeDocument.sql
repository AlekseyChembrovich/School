﻿CREATE TABLE [dbo].[TypeDocument]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name] NVARCHAR(100) NULL,
	[PositionId] INT NULL,
	CONSTRAINT FK_Position_TypeDocument FOREIGN KEY ([PositionId]) REFERENCES [dbo].[Position]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)