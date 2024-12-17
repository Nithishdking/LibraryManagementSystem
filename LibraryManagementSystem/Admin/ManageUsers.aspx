<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="LibraryManagementSystem.Admin.ManageUsers" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Manage Users</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f8f9fa;
            margin: 0;
            padding: 0;
            color: #333;
        }

        .container {
            width: 90%;
            margin: 30px auto;
            padding: 20px;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        h1, h3 {
            text-align: center;
            margin-bottom: 10px;
            color: #333;
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

        .form-inline {
            display: flex;
            justify-content: center;
            align-items: center;
            margin-bottom: 20px;
        }

        .form-inline input {
            padding: 8px;
            margin-right: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
        }

        .btn {
            background-color: #28a745;
            color: white;
            border: none;
            padding: 8px 12px;
            margin-right: 10px;
            border-radius: 5px;
            cursor: pointer;
            font-size: 14px;
        }

        .btn-clear {
            background-color: #dc3545;
        }

        .btn:hover {
            opacity: 0.9;
        }

        .table-container {
            margin-top: 20px;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            padding: 10px;
            border: 1px solid #ddd;
            text-align: center;
        }

        th {
            background-color: #28a745;
            color: white;
        }

        td a {
            margin-right: 10px;
            text-decoration: none;
            font-weight: bold;
            color: #007bff;
        }

        td a:hover {
            text-decoration: underline;
        }

        .status-approved {
            color: #28a745;
            font-weight: bold;
        }

        .status-rejected {
            color: #dc3545;
            font-weight: bold;
        }

        .status-pending {
            color: #ffc107;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Back Button -->
            <a href="AdminDashboard.aspx" class="btn-back">Back to Dashboard</a>
            <h1>Manage Users</h1>

            <!-- Pending Users Section -->
            <h3>Pending Users</h3>
            <asp:GridView ID="gvPendingUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID" OnRowCommand="gvPendingUsers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="UserID" Visible="false" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserID") %>' Text="Approve" CssClass="btn" />
                            <asp:Button ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserID") %>' Text="Reject" CssClass="btn btn-clear" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Search Section -->
            <h3>Search Users</h3>
            <div class="form-inline">
                <asp:TextBox ID="txtSearch" runat="server" Placeholder="Search by Username"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-clear" OnClick="btnClearSearch_Click" />
            </div>

            <!-- Approved Users Section -->
            <h3>Approved Users</h3>
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
                          OnRowEditing="gvUsers_RowEditing" OnRowDeleting="gvUsers_RowDeleting" OnRowDataBound="gvUsers_RowDataBound"
                          OnRowCancelingEdit="gvUsers_RowCancelingEdit" OnRowUpdating="gvUsers_RowUpdating">
                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# Eval("IsApproved") != null && Convert.ToInt32(Eval("IsApproved")) == 1 ? 
                                "<span class='status-approved'>Approved</span>" : 
                                Convert.ToInt32(Eval("IsApproved")) == -1 ? 
                                "<span class='status-rejected'>Rejected</span>" : 
                                "<span class='status-pending'>Pending</span>" %>
                        </ItemTemplate>
                        <EditItemTemplate>
        <asp:DropDownList ID="ddlStatus" runat="server">
            <asp:ListItem Value="1" Text="Approved"/>
            <asp:ListItem Value="-1" Text="Rejected"/>
            
        </asp:DropDownList>
    </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" ></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" ForeColor="Red" ></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" ></asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ForeColor="Red" ></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
