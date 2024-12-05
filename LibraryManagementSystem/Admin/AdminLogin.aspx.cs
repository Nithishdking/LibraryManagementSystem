using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace LibraryManagementSystem.Admin
{
    public partial class AdminLogin : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Additional logic (if needed)
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string Username = username.Text.Trim();
            string Password = password.Text.Trim();

            // Reset error messages
            loginError.Visible = false;

            if (ValidateLogin(Username, Password))
            {
                // Store session and redirect to Admin Dashboard
                Session["AdminName"] = Username;
                Session["Password"]= Password;
                Response.Redirect("AdminDashboard.aspx");
            }
            else
            {
                // Display error for invalid credentials
                loginError.Visible = true;
                loginError.Text = "Invalid username or password.";
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM Admins WHERE AdminName = @AdminName AND Password = @Password";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AdminName", username);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    conn.Open();
                    int result = (int)cmd.ExecuteScalar();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    loginError.Visible = true;
                    loginError.Text = "Database error: " + ex.Message;
                    return false;
                }
            }
        }
    }
}
