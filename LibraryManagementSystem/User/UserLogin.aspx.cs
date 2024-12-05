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
            string hashedPassword = HashPassword(password); // Hash the entered password

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword); // Compare hashed passwords

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();

                    // Debugging output to confirm query result
                    System.Diagnostics.Debug.WriteLine("Authentication Count: " + count);

                    return count > 0;
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string userName = username.Text.Trim();
            string userPassword = password.Text.Trim();

            // Debugging output for entered values
            System.Diagnostics.Debug.WriteLine("Entered Username: " + userName);
            System.Diagnostics.Debug.WriteLine("Entered Password: " + userPassword);

            bool isAuthenticated = AuthenticateUser(userName, userPassword);

            if (isAuthenticated)
            {
                // Retrieve the UserID from the database
                int userId = GetUserId(userName);

                // Set session variables for authenticated user
                Session["Username"] = userName;
                Session["UserId"] = userId; // Set the logged-in user's ID

                // Debugging output
                System.Diagnostics.Debug.WriteLine("User Authenticated. UserId: " + userId);

                // Redirect to User Dashboard
                Response.Redirect("UserDashboard.aspx");
            }
            else
            {
                // Display login error message
                loginError.Text = "Invalid Username or Password!";
                loginError.Visible = true;

                // Debugging output
                System.Diagnostics.Debug.WriteLine("Authentication Failed!");
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
                        return Convert.ToInt32(result); // Return the UserID if found
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
