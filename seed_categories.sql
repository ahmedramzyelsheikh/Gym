SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (Id, CategoryName, CreateAt, UpdateAt) VALUES (1, 'Cardio', GETDATE(), GETDATE());
INSERT INTO Categories (Id, CategoryName, CreateAt, UpdateAt) VALUES (2, 'Strength', GETDATE(), GETDATE());
INSERT INTO Categories (Id, CategoryName, CreateAt, UpdateAt) VALUES (3, 'Training', GETDATE(), GETDATE());
INSERT INTO Categories (Id, CategoryName, CreateAt, UpdateAt) VALUES (4, 'Yoga', GETDATE(), GETDATE());
SET IDENTITY_INSERT Categories OFF;
SELECT Id, CategoryName FROM Categories;
