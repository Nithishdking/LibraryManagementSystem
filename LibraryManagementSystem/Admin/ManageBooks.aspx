<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageBooks.aspx.cs" Inherits="LibraryManagementSystem.Admin.ManageBooks" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Manage Books</title>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <a href="AdminDashboard.aspx" class="btn-back">Back to Dashboard</a>
            <h1>Manage Books</h1>

            <!-- Add New Book Section -->
            <h3>Add New Book</h3>
            <div class="form-inline">
                <asp:TextBox ID="txtTitle" runat="server" Placeholder="Title"></asp:TextBox>
                <asp:TextBox ID="txtAuthor" runat="server" Placeholder="Author"></asp:TextBox>
                <asp:TextBox ID="txtCategory" runat="server" Placeholder="Category"></asp:TextBox>
                <asp:TextBox ID="txtPublishedYear" runat="server" Placeholder="Published Year"></asp:TextBox>
                <asp:TextBox ID="txtCopies" runat="server" Placeholder="No. of Copies"></asp:TextBox>
                <asp:Button ID="btnAdd" runat="server" Text="Add Book" CssClass="btn" OnClick="btnAdd_Click" />
            </div>

            <!-- Search Section -->
            <h3>Search Books</h3>
            <div class="form-inline">
                <asp:TextBox ID="txtSearch" runat="server" Placeholder="Search by Title, Author, or Category"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClearSearch" runat="server" Text="Clear" CssClass="btn btn-clear" OnClick="btnClearSearch_Click" />
            </div>

            <!-- Table Section -->
            <div class="table-container">
                <asp:GridView ID="gvBooks" runat="server" AutoGenerateColumns="False" DataKeyNames="BookID"
                    CssClass="table" OnRowEditing="gvBooks_RowEditing" OnRowDeleting="gvBooks_RowDeleting"
                    OnRowCancelingEdit="gvBooks_RowCancelingEdit" OnRowUpdating="gvBooks_RowUpdating">
                    <Columns>
                        <asp:BoundField DataField="BookID" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Author" HeaderText="Author" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                        <asp:BoundField DataField="PublishedYear" HeaderText="Published Year" />
                        <asp:BoundField DataField="Copies" HeaderText="Copies" />
                        
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" ForeColor="Red"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update"></asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ForeColor="Red"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <!-- Error Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
