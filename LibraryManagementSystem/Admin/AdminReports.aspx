<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminReports.aspx.cs" Inherits="LibraryManagementSystem.AdminReports" %>

<!DOCTYPE html>
<html>
<head>
    <title>Admin Reports</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }
        form1 {
            width: 90%;
            margin: 20px auto;
            padding: 20px;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
        h1 {
            text-align: center;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        table, th, td {
            border: 1px solid #ddd;
        }
        th, td {
            padding: 10px;
            text-align: left;
        }
        th {
            background-color: #4CAF50;
            color: white;
        }
        .button-container {
            text-align: center;
            margin-top: 20px;
        }
        .back-button {
            padding: 10px 20px;
            background-color: #4CAF50;
            color: white;
            text-decoration: none;
            border-radius: 5px;
        }
        .back-button:hover {
            background-color: #45a049;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Admin Reports</h1>
        <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="True" OnRowDataBound="gvBorrowRecords_RowDataBound"></asp:GridView>
        <div class="button-container">
            <a href="AdminDashboard.aspx" class="back-button">Back to Dashboard</a>
        </div>
    </form>
</body>
</html>
