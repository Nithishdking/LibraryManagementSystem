<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="LibraryManagementSystem.User.UserDashboard" %>

<!DOCTYPE html>
<html>
<head>
    <title>User Dashboard - Library Management System</title>
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
    
    margin-bottom: 30px;
}
.borrow-details-link {
    position: absolute; /* Position it relative to the navbar */
    top: 60px; /* Space below the navbar */
    right: 20px; /* Align to the left */
}

.borrow-details-link .link-btn {
    display: inline-block;
    background-color: #007bff; /* Button background color */
    color: white; /* Text color */
    text-decoration: none; /* Remove underline */
    font-size: 14px; /* Font size */
    font-weight: bold; /* Make the text bold */
    padding: 10px 20px; /* Add padding for size */
    border-radius: 5px; /* Rounded corners */ /* Subtle shadow */
}

.borrow-details-link .link-btn:hover {
    background-color: #0056b3; /* Darker blue on hover */
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3); /* Add a shadow effect */
    transform: scale(1.05); /* Slightly enlarge the button */
}


        .welcome-message {
            font-size: 24px;
            color: #007bff;
            margin-bottom: 20px;
            margin: 50px auto;
text-align: center
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        table th, table td {
            padding: 10px;
            text-align: left;
            border: 1px solid #ddd;
        }
        table th {
            background-color: #4CAF50;
            color: white;
        }
        table tr:nth-child(even) {
            background-color: #f2f2f2;
        }
        .action-btn {
            color: #007bff;
            text-decoration: none;
            font-weight: bold;
        }
        .action-btn:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
       <div class="navbar">
    <!-- Left side: Website name -->
    <div class="navbar-left">
         Dashboard
    </div>

    <!-- Right side: Logout button -->
    <div class="navbar-right">
        <a href="/Home.aspx" id="btnLogout" runat="server" OnClick="Logout_Click">Logout</a>
    </div>
</div>
    <form id="form1" runat="server">
    <!-- Navigation Bar -->
    <!-- Navigation Bar -->
    
     
    <!-- Dashboard Container -->
        
    <div class="dashboard-container">
         
        <div class="welcome-message">
            <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label>
        </div>
             <div class="borrow-details-link">
    <a href="BorrowHistory.aspx" class="link-btn">View Borrow History</a>
</div>
        <h3>Available Books</h3>

        <!-- Available Books Table -->
        <table>
            <%--<thead>
                <tr>
                    <th>Book ID</th>
                    <th>Title</th>
                    <th>Author</th>
                    <th>Category</th>
                    <th>Year of Publication</th>
                    <th>Actions</th>
                </tr>
            </thead>--%>
            <tbody>
                <asp:GridView ID="gvAvailableBooks" runat="server" AutoGenerateColumns="False" OnRowCommand="gvAvailableBooks_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="BookID" HeaderText="Book ID" SortExpression="BookID" />
                        <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                        <asp:BoundField DataField="Author" HeaderText="Author" SortExpression="Author" />
                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                        <asp:BoundField DataField="PublishedYear" HeaderText="Year of Publication" SortExpression="PublishedYear" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnBorrow" runat="server" Text="Borrow" CssClass="action-btn" CommandName="Borrow" CommandArgument='<%# Eval("BookID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tbody>
        </table>
    </div>
</form>
</body>
</html>
