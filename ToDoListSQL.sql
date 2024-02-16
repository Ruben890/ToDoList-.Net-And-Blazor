
CREATE DATABASE ToDoList;
USE ToDoList;

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1, 1),
    Name VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL
);

CREATE TABLE Tasks (
    TasksId INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(50) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    IsCompleted BIT DEFAULT 0, -- BIT para representar un valor booleano
    DateCreated DATETIME DEFAULT GETDATE(), -- Cambié el nombre de la columna para hacerlo más claro
    UserID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserId)
);

