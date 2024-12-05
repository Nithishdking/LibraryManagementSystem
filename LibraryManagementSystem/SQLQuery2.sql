INSERT INTO Admins (Username, Password, Email)
VALUES ('admin', 'admin', 'admin@gmail.com');

Select*from Admins;



INSERT INTO BorrowRecords (UserID, BookID, BorrowDate, DueDate, ReturnDate, IsReturned)
VALUES
(1, 20, '2024-11-01', '2024-11-15', NULL, 0),
(2, 23, '2024-11-03', '2024-11-17', NULL, 0),
(3, 24, '2024-11-05', '2024-11-19', NULL, 0),
(4, 25, '2024-11-06', '2024-11-20', NULL, 0),
(5, 26, '2024-11-07', '2024-11-21', NULL, 0),
(6, 27, '2024-11-08', '2024-11-22', NULL, 0),
(7, 28, '2024-11-09', '2024-11-23', NULL, 0),
(8, 29, '2024-11-10', '2024-11-24', NULL, 0),
(9, 22, '2024-11-11', '2024-11-25', NULL, 0),
(10, 21, '2024-11-12', '2024-11-26', NULL, 0);

Select * from BorrowRecords;

INSERT INTO Users (Username, Password, Email, FullName, ContactNumber)
VALUES
('john_doe', 'password123', 'john.doe@example.com', 'John Doe', '123-456-7890'),
('jane_smith', 'password456', 'jane.smith@example.com', 'Jane Smith', '234-567-8901'),
('robert_brown', 'password789', 'robert.brown@example.com', 'Robert Brown', '345-678-9012'),
('emily_jones', 'password321', 'emily.jones@example.com', 'Emily Jones', '456-789-0123'),
('michael_white', 'password654', 'michael.white@example.com', 'Michael White', '567-890-1234'),
('susan_black', 'password987', 'susan.black@example.com', 'Susan Black', '678-901-2345'),
('william_green', 'password111', 'william.green@example.com', 'William Green', '789-012-3456'),
('patricia_blue', 'password222', 'patricia.blue@example.com', 'Patricia Blue', '890-123-4567'),
('james_red', 'password333', 'james.red@example.com', 'James Red', '901-234-5678'),
('linda_yellow', 'password444', 'linda.yellow@example.com', 'Linda Yellow', '012-345-6789');

Select* from Users;

INSERT INTO Books (Title, Author, Category, PublishedYear, IsAvailable)
VALUES
('To Kill a Mockingbird', 'Harper Lee', 'Fiction', 1960, 1),
('1984', 'George Orwell', 'Dystopian', 1949, 1),
('The Great Gatsby', 'F. Scott Fitzgerald', 'Classic', 1925, 1),
('Moby Dick', 'Herman Melville', 'Adventure', 1851, 1),
('Pride and Prejudice', 'Jane Austen', 'Romance', 1813, 1),
('The Catcher in the Rye', 'J.D. Salinger', 'Fiction', 1951, 1),
('The Hobbit', 'J.R.R. Tolkien', 'Fantasy', 1937, 1),
('The Odyssey', 'Homer', 'Epic', -800, 0),
('War and Peace', 'Leo Tolstoy', 'Historical Fiction', 1869, 1),
('Crime and Punishment', 'Fyodor Dostoevsky', 'Psychological Fiction', 1866, 1);

Select * from Books;

ALTER TABLE Users
ADD IsActive BIT DEFAULT 0; -- Default is inactive (0)
