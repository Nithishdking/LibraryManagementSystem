

<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="Home.aspx.cs" Inherits="LibraryManagementSystem.Home" %>


<!DOCTYPE html>
<html>
<head>
    <title>E-Library</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f9f9f9;
        }
        .navbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background-color: #4CAF50;
            padding: 10px 20px;
        }
        .navbar .logo {
            color: white;
            font-size: 20px;
            font-weight: bold;
            text-decoration: none;
        }
        .navbar .links {
            display: flex;
            gap: 15px;
        }
        .navbar .links a {
            color: white;
            text-decoration: none;
            padding: 8px 12px;
            /*background-color: #007bff;
            border-radius: 4px;
            transition: background-color 0.3s;*/
        }
        .navbar .links a:hover {
            background-color: #0056b3;
        }
        .main-content {
            text-align: center;
            margin-top: 50px;
        }
        .main-content h1 {
            font-size: 36px;
            color: #333;
        }
        .main-content p {
            font-size: 18px;
            color: #666;
            margin: 20px 0;
        }
        .main-content a {
            font-size: 20px;
            color: #007bff;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <!-- Navigation Bar -->
    <div class="navbar">
        <a href="Home.aspx" class="logo">E-Library</a>
        <div class="links">
            <a href="Admin/AdminLogin.aspx">Admin</a>
            <a href="User/UserLogin.aspx">User</a>
            <a href="User/Register.aspx">New Register</a>
        </div>
    </div>

    <!-- Main Content -->
    <div class="main-content">
        <h1>Welcome to the Library Management System</h1>
        <p>
            Manage your books and borrowing records seamlessly. <br />
            Please log in or register to get started.
        </p>
         <a href="User/UserLogin.aspx">Login in</a>
    </div>
</body>
</html>

