<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="LibraryManagementSystem.User.UserLogin" %>

<!DOCTYPE html>
<html>
<head>
    <title>Admin Login</title>
    <style>
        body {
            font-family: 'Arial', sans-serif;
            background: #f0f4f8;
            margin: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .login-container {
            width: 100%;
            max-width: 400px;
            background: #fff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 8px 15px rgba(0, 0, 0, 0.1);
        }

        .login-container h2 {
            text-align: center;
            font-size: 24px;
            font-weight: bold;
            color: #333;
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            font-size: 14px;
            color: #555;
            margin-bottom: 5px;
            display: inline-block;
        }

        input[type="text"], input[type="password"] {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            box-sizing: border-box;
            transition: all 0.3s ease;
        }

        input[type="text"]:focus, input[type="password"]:focus {
            border-color: #007bff;
            box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
            outline: none;
        }

        .btn-login {
            width: 100%;
            padding: 12px;
            background-color: #4CAF50;
            color: #fff;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .btn-login:hover {
            background-color: #4CAF50;
        }

        .error-message {
            font-size: 12px;
            color: red;
            margin-top: 5px;
        }

        .footer {
            text-align: center;
            margin-top: 20px;
            font-size: 12px;
            color: #666;
        }

        .footer a {
            color: #007bff;
            text-decoration: none;
        }

        .footer a:hover {
            text-decoration: underline;
        }
    </style>
    <script>
        function validateForm() {
            const username = document.getElementById('username');
            const password = document.getElementById('password');
            let isValid = true;

            if (!username.value.trim()) {
                document.getElementById('usernameError').style.display = 'block';
                isValid = false;
            } else {
                document.getElementById('usernameError').style.display = 'none';
            }

            if (!password.value.trim()) {
                document.getElementById('passwordError').style.display = 'block';
                isValid = false;
            } else {
                document.getElementById('passwordError').style.display = 'none';
            }

            return isValid;
        }
    </script>
</head>
<body>
    <form id="loginForm" runat="server" onsubmit="return validateForm();">
        <div class="login-container">
            <h2>User Login</h2>

            <!-- Username Field -->
            <div class="form-group">
                <label for="username">Username:</label>
                <asp:TextBox ID="username" runat="server" CssClass="form-control" placeholder="Enter Username" />
                <span id="usernameError" class="error-message" style="display:none;">Username is required.</span>
            </div>

            <!-- Password Field -->
            <div class="form-group">
                <label for="password">Password:</label>
                <asp:TextBox ID="password" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter Password" />
                <span id="passwordError" class="error-message" style="display:none;">Password is required.</span>
            </div>
            <asp:Label ID="loginError" runat="server" CssClass="error-message" Visible="true"></asp:Label>
            <!-- Login Button -->
            <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" CssClass="btn-login" />

           <%-- <!-- Footer -->
            <div class="footer">
                <p>Forgot your password? <a href="#">Reset here</a></p>
            </div>--%>
        </div>
    </form>
</body>
</html>



