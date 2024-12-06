<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageBooks.aspx.cs" Inherits="LibraryManagementSystem.Admin.ManageBooks" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Books</title>
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
        <h1>Manage Books</h1>
         <h3>Add New Book</h3>
  <div class="form-group">
      <label for="txtbookid">Book ID</label>
      <asp:TextBox ID="txtbookid" runat="server" Placeholder="Enter book id"></asp:TextBox>
  </div>
 <div class="form-group">
     <label for="txtTitle">Title</label>
     <asp:TextBox ID="txtTitle" runat="server" Placeholder="Enter book title"></asp:TextBox>
 </div>
 <div class="form-group">
     <label for="txtAuthor">Author</label>
     <asp:TextBox ID="txtAuthor" runat="server" Placeholder="Enter author name"></asp:TextBox>
 </div>
 <div class="form-group">
     <label for="txtCategory">Category</label>
     <asp:TextBox ID="txtCategory" runat="server" Placeholder="Enter category"></asp:TextBox>
 </div>
 <div class="form-group">
     <label for="txtPublishedYear">Published Year</label>
     <asp:TextBox ID="txtPublishedYear" runat="server" Placeholder="Enter year"></asp:TextBox>
 </div>


 <div class="actions">
     <asp:Button ID="btnAdd" runat="server" Text="Add Book" OnClick="btnAdd_Click" />
     <asp:Label ID="lblMessage" runat="server" CssClass="message-label" ForeColor="Red"></asp:Label>
 </div>

        
<div class="form-group">
    <label for="txtSearch">Search</label>
    <asp:TextBox ID="txtSearch" runat="server" Placeholder="Search by title, author, or category"></asp:TextBox>
</div>
<div class="actions">
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <asp:Button ID="btnClearSearch" runat="server" Text="Clear" OnClick="btnClearSearch_Click" />
</div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>

       <asp:GridView ID="gvBooks" runat="server" AutoGenerateColumns="False" DataKeyNames="BookID"
              OnRowEditing="gvBooks_RowEditing" OnRowDeleting="gvBooks_RowDeleting"
              OnRowCancelingEdit="gvBooks_RowCancelingEdit" OnRowUpdating="gvBooks_RowUpdating">
    <Columns>
        <asp:BoundField DataField="BookID" HeaderText="ID" ReadOnly="True" />
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="Author" HeaderText="Author" />
        <asp:BoundField DataField="Category" HeaderText="Category" />
        <asp:BoundField DataField="PublishedYear" HeaderText="Published Year" />
        
        
        <asp:TemplateField HeaderText="Availability">
            <ItemTemplate>
               
                <asp:CheckBox ID="chkIsAvailable" runat="server" Disabled="true" 
                              Checked='<%# Eval("IsAvailable") %>' />
            </ItemTemplate>
            <EditItemTemplate>
               
                <asp:CheckBox ID="chkIsAvailable" runat="server" 
                              Checked='<%# Eval("IsAvailable") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

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
