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
        .btn-back {
    display: inline-block;
    background-color: #007bff;
    color: #fff;
    padding: 10px 15px;
    text-decoration: none;
    border-radius: 5px;
    margin-bottom: 15px;
}

.btn-back:hover {
    background-color: #0056b3;
}
        form {
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
            margin-bottom: 20px;
        }
        .toggle-button {
            padding: 10px 20px;
            background-color: #4CAF50;
            color: white;
            text-decoration: none;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }
        .toggle-button:hover {
            background-color: #45a049;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <a href="AdminDashboard.aspx" class="btn-back">Back to Dashboard</a>
        <h1>Admin Reports</h1>
       <div class="button-container">
    <asp:Button ID="btnSwitchToBookReport" runat="server" Text="Book Borrow Report" CssClass="toggle-button" OnClick="ShowBookReport" />
    <asp:Button ID="btnSwitchToUserReport" runat="server" Text="User Borrow Report" CssClass="toggle-button" OnClick="ShowUserReport" />
    <!-- Buttons for downloading reports -->
    <asp:Button ID="btnDownloadBookReport" runat="server" Text="Download Book Report" CssClass="toggle-button" OnClick="DownloadBookReport" />
    <asp:Button ID="btnDownloadUserReport" runat="server" Text="Download User Report" CssClass="toggle-button" OnClick="DownloadUserReport" />
</div>


        <!-- Panel for Book Borrow Report -->
        <asp:Panel ID="pnlBookReport" runat="server" Visible="true">
            <asp:GridView ID="gvBookBorrowReport" runat="server" AutoGenerateColumns="True" ></asp:GridView>
        </asp:Panel>

        <!-- Panel for User Borrow Report -->
        <asp:Panel ID="pnlUserReport" runat="server" Visible="false">
            <asp:GridView ID="gvUserBorrowReport" runat="server" AutoGenerateColumns="True"></asp:GridView>
        </asp:Panel>
    </form>
</body>
</html>
