<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="LibraryManagementSystem.User.UserDashboard" %>

<!DOCTYPE html>
<html>
<head>
    <title>User Dashboard - Library Management System</title>
    <style>
        /* General Reset */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f9f9f9;
            color: #333;
            line-height: 1.6;
        }

        /* Navbar Styles */
        .navbar {
            background-color: #4CAF50;
            color: #fff;
            padding: 15px 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            position: sticky;
            top: 0;
            z-index: 1000;
        }

        .navbar .navbar-left {
            font-size: 24px;
            font-weight: bold;
        }

        .navbar .navbar-right a {
            text-decoration: none;
            color: #fff;
            padding: 8px 15px;
            border-radius: 5px;
            transition: background 0.3s;
        }

        .navbar .navbar-right a:hover {
            background-color: #2e7d32;
        }

        /* Dashboard Container */
        .dashboard-container {
            max-width: 1200px;
            margin: 80px auto;
            background-color: #fff;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }

        .welcome-message {
            font-size: 28px;
            color: #4CAF50;
            text-align: center;
            margin-bottom: 25px;
        }

        /* Search Form */
        .search-container {
            display: flex;
            justify-content: center;
            gap: 10px;
            margin-bottom: 20px;
        }

        .search-input {
            padding: 12px;
            border: 2px solid #ddd;
            border-radius: 5px;
            width: 350px;
            font-size: 16px;
        }

        .search-btn, .clear-btn {
            padding: 12px 18px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            color: #fff;
            transition: background 0.3s;
        }

        .search-btn {
            background-color: #4CAF50;
        }

        .search-btn:hover {
            background-color: #45a049;
        }

        .clear-btn {
            background-color: #f44336;
        }

        .clear-btn:hover {
            background-color: #e53935;
        }

        /* Borrow History Link */
        .borrow-details-link {
            text-align: right;
            margin-bottom: 15px;
        }

        .link-btn {
            text-decoration: none;
            background-color: #007bff;
            color: #fff;
            padding: 10px 20px;
            border-radius: 5px;
            font-weight: bold;
            transition: background 0.3s, transform 0.2s;
        }

        .link-btn:hover {
            background-color: #0056b3;
            transform: scale(1.05);
        }

        /* Table Styles */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
            background-color: #fff;
            border: 1px solid #ddd;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }

        table th, table td {
            padding: 12px 15px;
            text-align: center;
            border-bottom: 1px solid #ddd;
        }

        table th {
            background-color: #4CAF50;
            color: white;
            text-transform: uppercase;
        }

        table tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        .action-btn {
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 8px 12px;
            border-radius: 5px;
            text-decoration: none;
            transition: background 0.3s;
            font-weight: bold;
        }

        .action-btn:hover {
            background-color: #0056b3;
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .search-container {
                flex-direction: column;
                align-items: center;
            }

            .search-input {
                width: 100%;
            }

            .dashboard-container {
                padding: 15px;
            }
        }
    </style>

</head>
<body>
    <!-- Navbar -->
    <div class="navbar">
        <div class="navbar-left">User Dashboard</div>
        <div class="navbar-right">
            <a href="/Home.aspx" id="btnLogout" runat="server" OnClick="Logout_Click">Logout</a>
        </div>
    </div>

    <!-- Main Dashboard Content -->
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <div class="welcome-message">
                <asp:Label ID="lblWelcome" runat="server" Text="Welcome to the Library!"></asp:Label>
            </div>

            <!-- Borrow History Link -->
            <div class="borrow-details-link">
                <a href="BorrowHistory.aspx" class="link-btn">View Borrow History</a>
            </div>

            <!-- Search Section -->
            <div class="search-container">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by Title, Author, or Category" CssClass="search-input"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="SearchBooks" CssClass="search-btn" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="clear-btn" OnClick="ClearSearch" />
            </div>

            <!-- Table Section -->
            <table>
                <asp:GridView ID="gvAvailableBooks" runat="server" AutoGenerateColumns="False" OnRowCommand="gvAvailableBooks_RowCommand" CssClass="grid">
                    <Columns>
                        <asp:BoundField DataField="BookID" HeaderText="Book ID" />
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Author" HeaderText="Author" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                        <asp:BoundField DataField="PublishedYear" HeaderText="Year of Publication" />
                        <asp:BoundField DataField="Copies" HeaderText="Available Copies" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnBorrow" runat="server" Text="Borrow" CssClass="action-btn" CommandName="Borrow" CommandArgument='<%# Eval("BookID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </table>
        </div>
    </form>
     <script type="text/javascript">
         function ClearSearch() {
             document.getElementById('<%= txtSearch.ClientID %>').value = ''; // Clear the input field
         }
     </script>

</body>
</html>


