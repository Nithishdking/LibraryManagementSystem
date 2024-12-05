<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LibraryManagementSystem.Register" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Register</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .form-container {
            width: 40%;
            margin: 100px auto;
            padding: 30px;
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
        }
        h1 {
            text-align: center;
            font-size: 24px;
            margin-bottom: 20px;
            color: #333;
        }
        .form-group {
            margin-bottom: 25px;
            position: relative;
        }
        .form-group label {
            display: block;
            font-weight: bold;
            margin-bottom: 8px;
            font-size: 14px;
            color: #555;
        }
        .form-group input {
            width: 100%;
            padding: 12px 14px;
            font-size: 14px;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-sizing: border-box;
            transition: border-color 0.3s;
        }
        .form-group input:focus {
            border-color: #4CAF50;
            outline: none;
        }
        .form-group .required {
            color: red;
        }
        .form-group .toggle-password {
            position: absolute;
            top: 50%;
            right: 15px;
            transform: translateY(-50%);
            cursor: pointer;
            font-size: 14px;
            color: #007bff;
        }
        .submit-btn {
            display: block;
            width: 100%;
            padding: 12px;
            font-size: 16px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        .submit-btn:hover {
            background-color: #45a049;
        }
        .error {
            color: red;
            font-size: 12px;
            margin-top: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h1>User Registration</h1>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
            <div class="form-group">
                <label for="txtUsername">Username <span class="required">*</span></label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="true"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtPassword">Password <span class="required">*</span></label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <span class="toggle-password" onclick="togglePassword()">Show</span>
            </div>
            <div class="form-group">
                <label for="txtFullName">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revFullName" runat="server"
                    ControlToValidate="txtFullName" ErrorMessage="Only alphabets are allowed!"
                    ValidationExpression="^[a-zA-Z\s]*$" ForeColor="Red" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail" ErrorMessage="Enter a valid email!"
                    ValidationExpression="^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$" ForeColor="Red" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtContactNumber">Contact Number</label>
                <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revContactNumber" runat="server"
                    ControlToValidate="txtContactNumber" ErrorMessage="Only numbers are allowed!"
                    ValidationExpression="^\d{10}$" ForeColor="Red" Display="Dynamic" />
            </div>
            <asp:Button ID="btnRegister" runat="server" CssClass="submit-btn" Text="Register" OnClick="btnRegister_Click" />
        </div>
    </form>
    <script>
        function togglePassword() {
            const passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
            const type = passwordInput.type === 'password' ? 'text' : 'password';
            passwordInput.type = type;
        }
    </script>
</body>
</html>
