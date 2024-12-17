<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BorrowHistory.aspx.cs" Inherits="LibraryManagementSystem.User.BorrowHistory" %>

<!DOCTYPE html>
<html>
<head>
    <title>User Borrow History</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }

        .container {
            width: 90%;
            margin: 50px auto;
            
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
            padding: 20px;
        }

        h2 {
            color: #333333;
            text-align: center;
            font-size: 26px;
            margin-bottom: 20px;
        }

        .back-button {
            text-decoration: none;
            color: #ffffff;
            background-color: #007BFF;
            padding: 8px 15px;
            border-radius: 5px;
            display: inline-block;
            margin-bottom: 20px;
            transition: background-color 0.3s ease;
        }

        .back-button:hover {
            background-color: #0056b3;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
            overflow: hidden;
            border-radius: 10px;
            background-color: #fff;
        }

        table th {
            background-color: #28a745;
            color: #ffffff;
            font-weight: 600;
            padding: 12px;
            text-align: center;
        }

        table td {
            padding: 10px;
            text-align: center;
            border-bottom: 1px solid #ddd;
            color: #555;
        }

        table tr:nth-child(even) {
            background-color: #f8f9fa;
        }

        table tr:hover {
            background-color: #f1f1f1;
        }

        .actions {
            display: flex;
            justify-content: center;
            gap: 10px;
        }

        .actions button {
            padding: 6px 12px;
            border: none;
            border-radius: 5px;
            color: white;
            cursor: pointer;
            transition: transform 0.2s ease;
        }

        .extend-btn {
            background-color: #28a745;
        }

        .extend-btn:hover {
            background-color: #218838;
        }

        .return-btn {
            background-color: #dc3545;
        }

        .return-btn:hover {
            background-color: #c82333;
        }

        .actions button:hover {
            transform: scale(1.05);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="container">
            <a href="UserDashboard.aspx" class="back-button">Back to Dashboard</a>
            <h2>User Borrow History</h2>

            <asp:GridView ID="gvBorrowRecords" runat="server" AutoGenerateColumns="False" OnRowCommand="gvBorrowRecords_RowCommand" CssClass="grid-view">
                <Columns>
    <asp:BoundField DataField="BookID" HeaderText="Book ID" />
    <asp:BoundField DataField="Title" HeaderText="Title" />
    <asp:BoundField DataField="Author" HeaderText="Author" />
    <asp:BoundField DataField="Category" HeaderText="Category" />
    <asp:BoundField DataField="BorrowDate" HeaderText="Borrow Date" DataFormatString="{0:yyyy-MM-dd}" />
    <asp:BoundField DataField="ReturnDate" HeaderText="Due Date" DataFormatString="{0:yyyy-MM-dd}" />
    
    <%-- Actions Buttons --%>
    <asp:TemplateField HeaderText="Actions">
        <ItemTemplate>
            <div class="actions">
                <asp:Button ID="btnExtendDueDate" runat="server" Text="Extend Due Date" 
                            CommandName="ExtendDueDate" CommandArgument='<%# Eval("BookID") %>' CssClass="extend-btn" />
            </div>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:BoundField DataField="ReturnedDate" HeaderText="Returned Date" DataFormatString="{0:yyyy-MM-dd}" />

    <%-- Return Status --%>
    <asp:TemplateField HeaderText="Return Status">
        <ItemTemplate>
            <%# Convert.ToBoolean(Eval("IsReturned")) ? "<span style='color: green;'>Returned</span>" : "<span style='color: red;'>Not Returned</span>" %>
        </ItemTemplate>
    </asp:TemplateField>

    <%-- Return Button --%>
    <asp:TemplateField>
        <ItemTemplate>
            <div class="actions">
                <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="return-btn" 
                            CommandName="Return" CommandArgument='<%# Eval("BookID") %>' 
                            Visible='<%# !(bool)Eval("IsReturned") %>' />
            </div>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
