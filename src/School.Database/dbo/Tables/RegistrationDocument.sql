CREATE TABLE [dbo].[RegistrationDocument]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[CreateDate] DATE NULL,
	[StartDate] DATE NULL,
	[DirectionDocument] NVARCHAR(50) NULL,
	[TypeDocumentId] INT NULL,
	[EmployeeCreatorId] INT NULL,
	[EmployeeApproverId] INT NULL,
	CONSTRAINT FK_TypeDocument_RegistrationDocument FOREIGN KEY ([TypeDocumentId]) 
	REFERENCES [dbo].[TypeDocument]([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_EmployeeCreator_RegistrationDocument FOREIGN KEY ([EmployeeCreatorId]) 
	REFERENCES [dbo].[Employee]([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_EmployeeApprover_RegistrationDocument FOREIGN KEY ([EmployeeApproverId]) 
	REFERENCES [dbo].[Employee]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
