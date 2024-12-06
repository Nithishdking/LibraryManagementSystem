using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace LibraryManagementSystem.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindApprovedUsersGridView();
                BindPendingUsersGridView();
            }
        }

        // Bind Pending Users (IsApproved = 0) to GridView
        private void BindPendingUsersGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, FullName, ContactNumber, Email FROM Users WHERE IsApproved = 0";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPendingUsers.DataSource = dt;
                gvPendingUsers.DataBind();
            }
        }

        // Bind Approved Users (IsApproved = 1) to GridView
        private void BindApprovedUsersGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, FullName, Email, ContactNumber FROM Users WHERE IsApproved = 1";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        // Handle Approve and Reject actions in Pending Users GridView
        protected void gvPendingUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Approve")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Approve the user by updating the IsApproved field to 1
                    string approveQuery = "UPDATE Users SET IsApproved = 1 WHERE UserID = @UserID";
                    SqlCommand approveCmd = new SqlCommand(approveQuery, conn);
                    approveCmd.Parameters.AddWithValue("@UserID", userID);
                    approveCmd.ExecuteNonQuery();
                }

                // Refresh both grids
                BindPendingUsersGridView();
                BindApprovedUsersGridView();
            }
            else if (e.CommandName == "Reject")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Reject the user by deleting from the Users table
                    string rejectQuery = "DELETE FROM Users WHERE UserID = @UserID AND IsApproved = 0";
                    SqlCommand rejectCmd = new SqlCommand(rejectQuery, conn);
                    rejectCmd.Parameters.AddWithValue("@UserID", userID);
                    rejectCmd.ExecuteNonQuery();
                }

                // Refresh the pending users grid
                BindPendingUsersGridView();
            }
        }

        // Search functionality for Users GridView
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string username = txtSearch.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, FullName, Email, ContactNumber FROM Users WHERE Username LIKE @Username AND IsApproved = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", "%" + username + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        // Clear search results and reload the approved users
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindApprovedUsersGridView();
        }

        // Edit User details in GridView
        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindApprovedUsersGridView();
        }

        // Cancel editing in GridView
        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindApprovedUsersGridView();
        }

        // Update user details in GridView
        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvUsers.Rows[e.RowIndex];
            int userID = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            string username = ((TextBox)row.Cells[1].Controls[0]).Text;
            string fullname = ((TextBox)row.Cells[2].Controls[0]).Text;
            string email = ((TextBox)row.Cells[3].Controls[0]).Text;
            string contactNumber = ((TextBox)row.Cells[4].Controls[0]).Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET Username = @Username, FullName = @FullName, Email = @Email, ContactNumber = @ContactNumber WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@FullName", fullname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvUsers.EditIndex = -1;
            BindApprovedUsersGridView();
        }

        // Delete User from GridView
        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userID = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check if the user has active borrow records
                string checkQuery = "SELECT COUNT(*) FROM BorrowRecords WHERE UserID = @UserID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                int borrowCount = (int)checkCmd.ExecuteScalar();

                if (borrowCount > 0)
                {
                    lblError.Text = "Cannot delete user. This user has active borrow records.";
                    lblError.Visible = true;
                }
                else
                {
                    // Delete the user if no active borrow records
                    string deleteQuery = "DELETE FROM Users WHERE UserID = @UserID";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@UserID", userID);
                    deleteCmd.ExecuteNonQuery();

                    lblError.Text = string.Empty; // Clear error message
                    lblError.Visible = false;
                }

                conn.Close();
            }

            BindApprovedUsersGridView();
        }
    }
}
