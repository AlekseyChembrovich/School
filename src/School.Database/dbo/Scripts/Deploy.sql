GO
USE [School]

GO
INSERT INTO [dbo].[Position] ([dbo].[Position].[Name])
	VALUES ('test1'), ('test2'), ('test3'), ('test4'), ('test5')

GO
INSERT INTO [dbo].[TypeDocument] ([dbo].[TypeDocument].[Name], [dbo].[TypeDocument].[PositionId])
	VALUES ('test1', 1), ('test2', 2), ('test3', 3), ('test4', 4), ('test5', 5)

GO
INSERT INTO [dbo].[Employee] ([dbo].[Employee].[Surname], [dbo].[Employee].[Name], 
							  [dbo].[Employee].[Patronymic], [dbo].[Employee].[Phone], [dbo].[Employee].[PositionId])
	VALUES ('test1', 'test1', 'test1', '+375-29-556-06-66', 1),
		   ('test2', 'test2', 'test2', '+375-29-556-06-66', 2),
		   ('test3', 'test3', 'test3', '+375-29-556-06-66', 3),
		   ('test4', 'test4', 'test4', '+375-29-556-06-66', 4),
		   ('test5', 'test5', 'test5', '+375-29-556-06-66', 5)

GO
INSERT INTO [dbo].[RegistrationDocument] ([dbo].[RegistrationDocument].[CreateDate], [dbo].[RegistrationDocument].[StartDate], 
										  [dbo].[RegistrationDocument].[DirectionDocument], [dbo].[RegistrationDocument].[TypeDocumentId], 
										  [dbo].[RegistrationDocument].[EmployeeCreatorId], [dbo].[RegistrationDocument].[EmployeeApproverId])
		VALUES ('2021-09-01', '2021-09-01', 'test1', 1, 1, 1), 
			   ('2021-09-01', '2021-09-01', 'test2', 2, 2, 2), 
			   ('2021-09-01', '2021-09-01', 'test1', 3, 3, 3), 
			   ('2021-09-01', '2021-09-01', 'test2', 4, 4, 4), 
			   ('2021-09-01', '2021-09-01', 'test1', 5, 5, 5)
