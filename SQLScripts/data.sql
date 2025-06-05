USE [ics.employees]
GO

INSERT INTO Positions (Name) VALUES
    (N'Разработчик'),
    (N'Тестировщик'),
    (N'Аналитик'),
    (N'HR');
GO

INSERT INTO Employees (FirstName, LastName, PositionId, BirthYear, Salary) VALUES
    (N'Иван', N'Иванов', 1, 1990, 2000.00),
    (N'Елена', N'Сидорова', 2, 1992, 1700.00),
    (N'Пётр', N'Петров', 3, 1985, 2200.00),
    (N'Анна', N'Кузнецова', 1, 1995, 2100.00),
    (N'Максим', N'Смирнов', 4, 1988, 1500.00),
    (N'Ольга', N'Николаева', 2, 1991, 1650.00);
GO