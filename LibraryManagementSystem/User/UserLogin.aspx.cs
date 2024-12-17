using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.User
{
    public partial class UserLogin : System.Web.UI.Page
    {
        // Database connection string
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear any existing sessions when the page loads initially
            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        /// <summary>
        /// Hashes the password using SHA256 to match the stored hash in the database.
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Authenticates the user based on the provided credentials.
        /// Compares the hashed password with the stored hash.
        /// </summary>
        private bool AuthenticateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();

                    return count > 0;
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string userName = username.Text.Trim();
            string userPassword = password.Text.Trim();

            bool isAuthenticated = AuthenticateUser(userName, userPassword);

            if (isAuthenticated)
            {
                // Retrieve the user status
                int userStatus = GetUserStatus(userName);

                if (userStatus == 1)
                {
                    int userId = GetUserId(userName);
                    Session["Username"] = userName;
                    Session["UserId"] = userId;

                    Response.Redirect("UserDashboard.aspx");
                }
                else if (userStatus == 0)
                {
                    loginError.Text = "Your account is pending approval. Please wait 24 hours for approval.";
                    loginError.Visible = true;
                }
                else if (userStatus == -1)
                {
                    loginError.Text = "Your account has been rejected. Please contact the admin.";
                    loginError.Visible = true;
                }
            }
            else
            {
                loginError.Text = "Invalid Username or Password!";
                loginError.Visible = true;
            }
        }

        /// <summary>
        /// Retrieves the user's status from the database.
        /// </summary>
        private int GetUserStatus(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT IsApproved FROM Users WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("User status not found for the provided username.");
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the UserID from the database based on the provided username.
        /// </summary>
        private int GetUserId(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserId FROM Users WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("UserId not found for the provided username.");
                    }
                }
            }
        }
    }
}
