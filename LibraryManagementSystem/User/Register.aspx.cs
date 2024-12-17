using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        // Method to Hash Password
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Input Validation
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblMessage.Text = "Username and Password are required!";
                return;
            }

            

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the username already exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand cmdCheckUser = new SqlCommand(checkUserQuery, conn))
                    {
                        cmdCheckUser.Parameters.AddWithValue("@Username", txtUsername.Text);
                        int userCount = Convert.ToInt32(cmdCheckUser.ExecuteScalar());
                        if (userCount > 0)
                        {
                            lblMessage.Text = "Username already exists. Please choose a different username.";
                            return;
                        }
                    }

                    // Check if the email already exists
                    string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SqlCommand cmdCheckEmail = new SqlCommand(checkEmailQuery, conn))
                    {
                        cmdCheckEmail.Parameters.AddWithValue("@Email", txtEmail.Text);
                        int emailCount = Convert.ToInt32(cmdCheckEmail.ExecuteScalar());
                        if (emailCount > 0)
                        {
                            lblMessage.Text = "Email ID is already registered. Please use a different email.";
                            return;
                        }
                    }

                    // Hash the Password
                    string hashedPassword = HashPassword(txtPassword.Text);

                    // Insert User into Database
                    string insertQuery = "INSERT INTO Users (Username, Password, FullName, Email, ContactNumber, IsApproved) VALUES (@Username, @Password, @FullName, @Email, @ContactNumber, 0)";
                    using (SqlCommand cmdInsert = new SqlCommand(insertQuery, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@Username", txtUsername.Text);
                        cmdInsert.Parameters.AddWithValue("@Password", hashedPassword);
                        cmdInsert.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        cmdInsert.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmdInsert.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text);

                        cmdInsert.ExecuteNonQuery();
                    }
                }

                Response.Write("<script>alert('Registration successful! Wait few minutes for the approval.'); window.location='UserLogin.aspx';</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message;
            }
        }

    }
}
