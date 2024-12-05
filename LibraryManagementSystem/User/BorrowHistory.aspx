<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BorrowHistory.aspx.cs" Inherits="LibraryManagementSystem.User.BorrowHistory" %>

<!DOCTYPE html>
<html>
<head>
    <title>User Borrow History</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
        }
        .container {
            width: 80%;
            margin: 50px auto;
            text-align: center;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            background-color: #fff;
            margin: 20px 0;
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
        .return-btn {
            background-color: #f44336;
            color: white;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
        }
        .return-btn:hover {
            background-color: #d32f2f;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
    <div class="container">
        <a href="UserDashboard.aspx" class="back-button">Back to Dashboard</a>
        <h2>Borrow History</h2>
        <asp:GridView ID="gvBorrowRecords" runat="server" AutoGenerateColumns="False" OnRowCommand="gvBorrowRecords_RowCommand">
            <Columns>
                <asp:BoundField DataField="BookID" HeaderText="Book ID" />
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="Author" HeaderText="Author" />
                <asp:BoundField DataField="Category" HeaderText="Category" />
                <asp:BoundField DataField="BorrowDate" HeaderText="Borrow Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="ReturnDate" HeaderText="Due Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="ReturnedDate" HeaderText="Returned Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:TemplateField HeaderText="Return Status">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("IsReturned")) ? "Returned" : "Not Returned" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="return-btn" CommandName="Return" CommandArgument='<%# Eval("BookID") %>' Visible='<%# !(bool)Eval("IsReturned") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
        </form>
</body>
</html>
