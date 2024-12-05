<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewBorrowRecords.aspx.cs" Inherits="LibraryManagementSystem.Admin.ViewBorrowRecords" %>

<!DOCTYPE html>
<html>
<head>
    <title>View Borrow Records</title>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <a href="AdminDashboard.aspx" class="back-button">Back to Dashboard</a>
        <h1>Borrow Records</h1>
        <asp:GridView ID="gvBorrowRecords" runat="server" AutoGenerateColumns="False" DataKeyNames="BorrowID"
                      OnRowEditing="gvBorrowRecords_RowEditing" OnRowDeleting="gvBorrowRecords_RowDeleting"
                      OnRowCancelingEdit="gvBorrowRecords_RowCancelingEdit" OnRowUpdating="gvBorrowRecords_RowUpdating" OnRowDataBound="gvBorrowRecords_RowDataBound">
            <Columns>
                <asp:BoundField DataField="BorrowID" HeaderText="Borrow ID" ReadOnly="True" SortExpression="BorrowID" />
                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                <asp:BoundField DataField="BookTitle" HeaderText="Book Title" SortExpression="BookTitle" />
                <asp:BoundField DataField="BorrowDate" HeaderText="Borrow Date" SortExpression="BorrowDate" />
                <asp:BoundField DataField="ReturnDate" HeaderText="Due Date" SortExpression="ReturnDate" />
                <asp:BoundField DataField="ReturnedDate" HeaderText="Returned Date" SortExpression="ReturnDate" />
                <asp:CheckBoxField DataField="IsReturned" HeaderText="Returned" SortExpression="IsReturned" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
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
