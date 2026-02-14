USE ERP_DB;
GO

-- ===== Tables =====
IF OBJECT_ID('dbo.InvoiceDetails', 'U') IS NOT NULL DROP TABLE dbo.InvoiceDetails;
IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL DROP TABLE dbo.Invoices;
IF OBJECT_ID('dbo.Items', 'U') IS NOT NULL DROP TABLE dbo.Items;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
GO

CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Items (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);

CREATE TABLE Invoices (
    Id INT PRIMARY KEY IDENTITY,
    InvoiceNumber INT NULL,
    CustomerId INT NOT NULL,
    Date DATETIME NOT NULL DEFAULT(GETDATE()),
    Total DECIMAL(10,2) NOT NULL DEFAULT(0)
);

CREATE TABLE InvoiceDetails (
    Id INT PRIMARY KEY IDENTITY,
    InvoiceId INT NOT NULL,
    ItemId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Total DECIMAL(10,2) NOT NULL
);

-- ===== Foreign Keys =====
ALTER TABLE Invoices
ADD CONSTRAINT FK_Invoices_Customers
FOREIGN KEY (CustomerId) REFERENCES Customers(Id);

ALTER TABLE InvoiceDetails
ADD CONSTRAINT FK_InvoiceDetails_Invoices
FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE;

ALTER TABLE InvoiceDetails
ADD CONSTRAINT FK_InvoiceDetails_Items
FOREIGN KEY (ItemId) REFERENCES Items(Id);

GO

-- ===== Auto Invoice Number Trigger =====
CREATE OR ALTER TRIGGER trg_SetInvoiceNumber
ON Invoices
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE i
    SET InvoiceNumber = i.Id
    FROM Invoices i
    JOIN inserted ins ON ins.Id = i.Id
    WHERE i.InvoiceNumber IS NULL OR i.InvoiceNumber = 0;
END
GO

-- ===== Sample Data =====
INSERT INTO Customers(Name) VALUES ('Customer A'), ('Customer B');

INSERT INTO Items(Name, Price)
VALUES ('Item 1', 10.00),
       ('Item 2', 25.50),
       ('Item 3', 5.75);
GO
