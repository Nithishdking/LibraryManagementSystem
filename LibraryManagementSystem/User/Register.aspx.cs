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

            if (txtPassword.Text.Length < 6)
            {
                lblMessage.Text = "Password must be at least 6 characters long.";
                return;
            }

            try
            {
                // Hash the Password
                string hashedPassword = HashPassword(txtPassword.Text);

                // Insert User into Database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Users (Username, Password, FullName, Email, ContactNumber, IsApproved) VALUES (@Username, @Password, @FullName, @Email, @ContactNumber, 0)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword); // Save hashed password
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
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
