Drop table Admins;

-- Admins Table
CREATE TABLE Admins (
    AdminID INT IDENTITY(1,1) PRIMARY KEY,
    AdminName NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL
);

-- Books Table (Updated: Removed IsAvailable, Added Copies)
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Category NVARCHAR(255),
    PublishedYear INT,
    Copies INT NOT NULL -- Number of copies for each book
);

-- Users Table
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255),
    Email NVARCHAR(255) NOT NULL,
    ContactNumber NVARCHAR(15),
    Password NVARCHAR(255) NOT NULL,
    IsApproved INT NOT NULL DEFAULT 0
);

-- BorrowRecords Table
CREATE TABLE BorrowRecords (
    BorrowId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    BorrowDate DATE NOT NULL,
    ReturnDate DATE NOT NULL,
    IsReturned BIT NOT NULL DEFAULT 0,
    ReturnedDate DATE NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE
);
Select * from Books;

-- Step 1: Modify the Title column to NOT NULL (if needed)
ALTER TABLE Books
ALTER COLUMN Title NVARCHAR(255) NOT NULL;

-- Step 2: Add a UNIQUE constraint on the Title column
ALTER TABLE Books
ADD CONSTRAINT UQ_Books_Title UNIQUE (Title);


INSERT INTO Admins (AdminID, AdminName, Password)
VALUES (1, 'Admin', 'Admin@01');
