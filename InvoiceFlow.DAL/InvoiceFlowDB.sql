
/* ========================= DROP & CREATE DATABASE ========================= */ 
USE master;
GO

IF DB_ID('InvoiceFlowDB') IS NOT NULL
BEGIN
    ALTER DATABASE InvoiceFlowDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE InvoiceFlowDB;
END
GO

CREATE DATABASE InvoiceFlowDB;
GO

USE InvoiceFlowDB;
GO

/* =========================
   DROP ALL OBJECTS
========================= */

IF OBJECT_ID('fn_GetInvoiceDetails', 'IF') IS NOT NULL DROP FUNCTION fn_GetInvoiceDetails;
IF OBJECT_ID('fn_GetDashboardSummary', 'IF') IS NOT NULL DROP FUNCTION fn_GetDashboardSummary;

IF OBJECT_ID('sp_UpdatePaymentStatus', 'P') IS NOT NULL DROP PROCEDURE sp_UpdatePaymentStatus;
IF OBJECT_ID('sp_CreateInvoice', 'P') IS NOT NULL DROP PROCEDURE sp_CreateInvoice;
IF OBJECT_ID('sp_GetClientsByUser', 'P') IS NOT NULL DROP PROCEDURE sp_GetClientsByUser;
IF OBJECT_ID('sp_AddClient', 'P') IS NOT NULL DROP PROCEDURE sp_AddClient;
IF OBJECT_ID('sp_LoginUser', 'P') IS NOT NULL DROP PROCEDURE sp_LoginUser;
IF OBJECT_ID('sp_RegisterUser', 'P') IS NOT NULL DROP PROCEDURE sp_RegisterUser;

IF OBJECT_ID('InvoiceItems', 'U') IS NOT NULL DROP TABLE InvoiceItems;
IF OBJECT_ID('Invoices', 'U') IS NOT NULL DROP TABLE Invoices;
IF OBJECT_ID('Products', 'U') IS NOT NULL DROP TABLE Products;
IF OBJECT_ID('Clients', 'U') IS NOT NULL DROP TABLE Clients;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;


/* =========================
   TABLES
========================= */

CREATE TABLE Users (
    UserId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(150) UNIQUE,
    Password NVARCHAR(200),
    BusinessName NVARCHAR(150),
    BusinessAddress NVARCHAR(255),
    Phone NVARCHAR(20),
    Industry NVARCHAR(100),
    Location NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Clients (
    ClientId INT IDENTITY PRIMARY KEY,
    UserId INT,
    Name NVARCHAR(100),
    Email NVARCHAR(150),
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Products (
    ProductId INT IDENTITY PRIMARY KEY,
    UserId INT,
    Name NVARCHAR(150),
    Price DECIMAL(10,2),
    Description NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Invoices (
    InvoiceId INT IDENTITY PRIMARY KEY,
    UserId INT,
    ClientId INT,
    InvoiceNumber NVARCHAR(50),
    TotalAmount DECIMAL(10,2),
    Status NVARCHAR(20),
    PaymentType NVARCHAR(50),
    PaymentLink NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    DueDate DATETIME,
    UpdatedDate DATETIME NULL,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (ClientId) REFERENCES Clients(ClientId)
);

CREATE TABLE InvoiceItems (
    ItemId INT IDENTITY PRIMARY KEY,
    InvoiceId INT,
    ProductId INT NULL,
    ItemName NVARCHAR(150),
    Quantity INT,
    Price DECIMAL(10,2),
    Total AS (Quantity * Price),
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);


/* =========================
   STORED PROCEDURES
========================= */

GO

CREATE PROCEDURE sp_RegisterUser
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @Password NVARCHAR(200),
    @BusinessName NVARCHAR(150),
    @Phone NVARCHAR(20)
AS
BEGIN
    INSERT INTO Users(Name, Email, Password, BusinessName, Phone)
    VALUES(@Name, @Email, @Password, @BusinessName, @Phone)
END
GO

CREATE PROCEDURE sp_LoginUser
    @Email NVARCHAR(150),
    @Password NVARCHAR(200)
AS
BEGIN
    SELECT UserId, Name, BusinessName
    FROM Users
    WHERE Email = @Email AND Password = @Password AND IsDeleted = 0
END
GO

CREATE PROCEDURE sp_AddClient
    @UserId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @Phone NVARCHAR(20)
AS
BEGIN
    INSERT INTO Clients(UserId, Name, Email, Phone)
    VALUES(@UserId, @Name, @Email, @Phone)
END
GO

CREATE PROCEDURE sp_GetClientsByUser
    @UserId INT
AS
BEGIN
    SELECT * FROM Clients WHERE UserId = @UserId AND IsDeleted = 0
END
GO

CREATE PROCEDURE sp_CreateInvoice
    @UserId INT,
    @ClientId INT,
    @InvoiceNumber NVARCHAR(50),
    @TotalAmount DECIMAL(10,2),
    @Status NVARCHAR(20),
    @PaymentLink NVARCHAR(255)
AS
BEGIN
    INSERT INTO Invoices(UserId, ClientId, InvoiceNumber, TotalAmount, Status, PaymentLink)
    VALUES(@UserId, @ClientId, @InvoiceNumber, @TotalAmount, @Status, @PaymentLink)

    SELECT SCOPE_IDENTITY() AS InvoiceId
END
GO

CREATE PROCEDURE sp_UpdatePaymentStatus
    @InvoiceId INT,
    @Status NVARCHAR(20),
    @PaymentType NVARCHAR(50)
AS
BEGIN
    UPDATE Invoices
    SET Status = @Status,
        PaymentType = @PaymentType,
        UpdatedDate = GETDATE()
    WHERE InvoiceId = @InvoiceId
END
GO


/* =========================
   FUNCTIONS
========================= */

GO

CREATE FUNCTION fn_GetDashboardSummary(@UserId INT)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        SUM(CASE WHEN Status = 'Paid' THEN TotalAmount ELSE 0 END) AS TotalRevenue,
        SUM(CASE WHEN Status = 'Pending' THEN TotalAmount ELSE 0 END) AS PendingAmount,
        COUNT(*) AS TotalInvoices
    FROM Invoices
    WHERE UserId = @UserId AND IsDeleted = 0
);
GO

CREATE FUNCTION fn_GetInvoiceDetails(@InvoiceId INT)
RETURNS TABLE
AS
RETURN
(
    SELECT i.InvoiceId, u.BusinessName, c.Name AS ClientName,
           it.ItemName, it.Quantity, it.Price, it.Total, i.TotalAmount, i.Status, i.PaymentType, i.PaymentLink
    FROM Invoices i
    JOIN Users u ON i.UserId = u.UserId
    JOIN Clients c ON i.ClientId = c.ClientId
    JOIN InvoiceItems it ON i.InvoiceId = it.InvoiceId
    WHERE i.InvoiceId = @InvoiceId
);
GO


/* =========================
   SAMPLE DATA
========================= */

INSERT INTO Users(Name, Email, Password, BusinessName, Phone, Industry, Location)
VALUES ('Tejas', 'tejas@test.com', '1234', 'Tejas Solutions', '9999999999', 'IT', 'India');

INSERT INTO Clients(UserId, Name, Email, Phone)
VALUES 
(1, 'Rahul', 'rahul@test.com', '8888888888'),
(1, 'Amit', 'amit@test.com', '7777777777');

INSERT INTO Products(UserId, Name, Price, Description)
VALUES 
(1, 'Website Development', 5000, 'Full website build'),
(1, 'Mobile App', 8000, 'Android app development');

INSERT INTO Invoices(UserId, ClientId, InvoiceNumber, TotalAmount, Status)
VALUES 
(1, 1, 'INV001', 5000, 'Pending'),
(1, 2, 'INV002', 8000, 'Paid');

INSERT INTO InvoiceItems(InvoiceId, ProductId, ItemName, Quantity, Price)
VALUES 
(1, 1, 'Website Development', 1, 5000),
(2, 2, 'Mobile App', 1, 8000);

select * from Users;
select * from Clients;
select * from Products;
select * from Invoices;
select * from InvoiceItems;

