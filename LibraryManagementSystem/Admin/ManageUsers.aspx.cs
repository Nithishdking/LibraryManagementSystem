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
                BindGridView();
            }
        }

        // Bind GridView with data
        private void BindGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Fullname, Email, ContactNumber FROM Users";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        // RowEditing event
        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        // RowCancelingEdit event
        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindGridView();
        }

        // RowUpdating event
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
                string query = "UPDATE Users SET Username = @Username, Email = @Email , Fullname = @Fullname, ContactNumber = @ContactNumber WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Fullname", fullname);
                cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvUsers.EditIndex = -1;
            BindGridView();
        }

        // RowDeleting event
        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userID = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            BindGridView();
        }
    }
}