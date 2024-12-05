<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="LibraryManagementSystem.Admin.AdminDashboard" %>

<!DOCTYPE html>
<html>
<head>
    <title>Admin Dashboard - Library Management System</title>
    <style>
        /* Reset styles */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
        }

        /* Navbar Styles */
        .navbar {
            background-color: #4CAF50;
            color: white;
            padding: 10px 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            z-index: 1000;
        }

        .navbar .navbar-left {
            font-size: 20px;
            font-weight: bold;
        }

        .navbar .navbar-right {
            display: flex;
            font-size: 18px;
            gap: 15px;
        }

        .navbar .navbar-right a {
            color: white;
            text-decoration: none;
            padding: 8px 12px;
            border-radius: 5px;
            transition: background-color 0.3s ease;
        }

        .navbar .navbar-right a:hover {
            background-color: #0056b3;
        }

        /* Dashboard Container */
        .dashboard-container {
            width: 80%;
            margin: 100px auto; /* Account for the fixed navbar */
            text-align: center;
        }

        .dashboard-container h3 {
            color: #007bff;
            margin-bottom: 30px;
        }

        /* Card Styles */
        .card {
            width: 200px;
            margin: 20px;
            padding: 20px;
            background-color: white;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            display: inline-block;
            text-align: center;
            border-radius: 10px;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }

        .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.2);
        }

        .card h4 {
            margin-bottom: 10px;
        }

        .card a {
            text-decoration: none;
            color: #007bff;
            font-size: 18px;
            transition: color 0.3s ease;
        }

        .card a:hover {
            color: #0056b3;
        }
    </style>
</head>
<body>

    <!-- Navigation Bar -->
    <div class="navbar">
        <!-- Left side: Website name -->
        <div class="navbar-left">
            Admin Dashboard
        </div>

        <!-- Right side: Logout button -->
        <div class="navbar-right">
            <a href="/Home.aspx" id="btnLogout" runat="server" OnClick="Logout_Click">Logout</a>
        </div>
    </div>

    <!-- Dashboard Container -->
    <div class="dashboard-container">
        <h3>Welcome, Admin!</h3>
        <p>Manage books, users, borrow records, and generate reports from here.</p>

        <!-- Dashboard Cards -->
        <div class="card">
            <h4>Manage Books</h4>
            <a href="ManageBooks.aspx">Go to Books</a>
        </div>

        <div class="card">
            <h4>Manage Users</h4>
            <a href="ManageUsers.aspx">Go to Users</a>
        </div>

        <div class="card">
            <h4>Borrow Records</h4>
            <a href="ViewBorrowRecords.aspx">Go to Records</a>
        </div>

        <div class="card">
            <h4>Generate Reports</h4>
            <a href="AdminReports.aspx">Go to Reports</a>
        </div>
    </div>

</body>
</html>
