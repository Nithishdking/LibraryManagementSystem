using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

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
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;  // Clears the search text box
            BindApprovedUsersGridView();    // Rebinds the GridView with all approved users
        }

        // Bind Pending Users (IsApproved = 0) to GridView
        private void BindPendingUsersGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, FullName, ContactNumber, Email, IsApproved FROM Users WHERE IsApproved = 0";
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
                string query = "SELECT UserID, Username, FullName, Email, ContactNumber, IsApproved FROM Users";
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

                    // Reject the user by updating the IsApproved field to -1
                    string rejectQuery = "UPDATE Users SET IsApproved = -1 WHERE UserID = @UserID";
                    SqlCommand rejectCmd = new SqlCommand(rejectQuery, conn);
                    rejectCmd.Parameters.AddWithValue("@UserID", userID);
                    rejectCmd.ExecuteNonQuery();
                }

                // Refresh the pending users grid
                BindPendingUsersGridView();
                BindApprovedUsersGridView();
            }
        }
        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Check if the row is a data row
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                // Find the DropDownList control in the EditItemTemplate
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");

                // Get the current status value from the data-bound item
                int status = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsApproved"));

                // Set the selected value of the dropdown based on the current status
                if (ddlStatus != null)
                {
                    ddlStatus.SelectedValue = status.ToString();

                    // Disable the dropdown if the status is 'Approved' (1) or 'Pending' (0)
                    if (status == 1 || status == 0)
                    {
                        ddlStatus.Enabled = false; // Disable the dropdown
                    }
                    else
                    {
                        ddlStatus.Enabled = true; // Enable the dropdown for other statuses (like 'Rejected')
                    }
                }
            }
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

            // Get the updated status from the DropDownList
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
            int updatedStatus = Convert.ToInt32(ddlStatus.SelectedValue);

            // Update the user's status in the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET IsApproved = @Status WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", updatedStatus);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Exit edit mode and rebind the grid
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
