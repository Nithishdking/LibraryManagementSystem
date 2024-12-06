<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="LibraryManagementSystem.Admin.ManageUsers" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Users</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        form {
            width: 80%;
            margin: 50px auto;
            padding: 20px;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
        h1, h3 {
            text-align: center;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .form-group input {
            width: 100%;
            padding: 8px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }
        .actions {
            text-align: center;
            margin-top: 20px;
        }
        .actions button, .actions input[type="submit"] {
            padding: 10px 20px;
            background: #4CAF50;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            margin-right: 10px;
        }
        .actions button:hover, .actions input[type="submit"]:hover {
            background: #45a049;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <a href="AdminDashboard.aspx" class="back-button">Back to Dashboard</a>

        <h1>Manage Users</h1>

        <h3>Pending Users</h3>
<asp:GridView ID="gvPendingUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID" OnRowCommand ="gvPendingUsers_RowCommand">
    <Columns>
        <asp:BoundField DataField="UserID" HeaderText="UserID" Visible="false" />
        <asp:BoundField DataField="Username" HeaderText="Username" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserID") %>' Text="Approve" />
                <asp:Button ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserID") %>' Text="Reject" CssClass="btn-danger" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

        
        <div class="form-group">
            <label for="txtSearch">Search by Username:</label>
            <asp:TextBox ID="txtSearch" runat="server" Placeholder="Enter username"></asp:TextBox>
        </div>
        <div class="actions">
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClearSearch" runat="server" Text="Clear" OnClick="btnClearSearch_Click" />
        </div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>

        <!-- GridView for displaying users -->
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
              OnRowEditing="gvUsers_RowEditing" OnRowDeleting="gvUsers_RowDeleting"
              OnRowCancelingEdit="gvUsers_RowCancelingEdit" OnRowUpdating="gvUsers_RowUpdating">
    <Columns>
        <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
        <asp:BoundField DataField="Username" HeaderText="Username" />
        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" />
        <%--<asp:BoundField DataField="IsAdmin" HeaderText="Is Admin" />--%>
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete"></asp:LinkButton>
                
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update"></asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>
     </Columns>
          

    </asp:GridView>
     
    </form>
    
</body>
</html>