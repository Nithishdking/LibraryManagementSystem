using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace LibraryManagementSystem.Admin
{
    public partial class ManageBooks : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind the GridView only on the first page load
                BindGridView();

                }
        }



        private void BindGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Books";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvBooks.DataSource = dt;
                gvBooks.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int bookId = Convert.ToInt32(txtbookid.Text);
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string category = txtCategory.Text;
            int publishedYear = Convert.ToInt32(txtPublishedYear.Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Books (BookId,Title, Author, Category, PublishedYear, IsAvailable) VALUES (@BookId, @Title, @Author, @Category, @PublishedYear, @IsAvailable)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Author", author);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@PublishedYear", publishedYear);
                cmd.Parameters.AddWithValue("@IsAvailable", true);  // Inserting CheckBox value
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            lblMessage.Text = "Book added successfully!";

            txtbookid.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtAuthor.Text = string.Empty;
            txtCategory.Text = string.Empty;
            txtPublishedYear.Text = string.Empty;

            
            BindGridView();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Query to search in all relevant fields
                string query = @"
            SELECT * 
            FROM Books 
            WHERE 
                Title LIKE @SearchQuery 
                OR Author LIKE @SearchQuery 
                OR Category LIKE @SearchQuery";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvBooks.DataSource = dt;
                gvBooks.DataBind();
            }
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            // Clear the search field
            txtSearch.Text = string.Empty;

            // Rebind the GridView with all records
            BindGridView();
        }

        protected void gvBooks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBooks.EditIndex = e.NewEditIndex;
            BindGridView();  // Rebind the GridView to reflect changes
        }

        protected void gvBooks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int bookID = Convert.ToInt32(gvBooks.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check for active borrow records
                string checkQuery = "SELECT COUNT(*) FROM BorrowRecords WHERE BookId = @BookID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@BookID", bookID);

                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                conn.Close();

                if (count > 0)
                {
                    // Display error message
                    lblError.Text = "Cannot delete this book. It is currently referenced in active borrow records.";
                    lblError.Visible = true;
                }
                else
                {
                    // Proceed with deletion
                    string deleteQuery = "DELETE FROM Books WHERE BookID = @BookID";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@BookID", bookID);

                    conn.Open();
                    deleteCmd.ExecuteNonQuery();
                    conn.Close();

                    BindGridView(); // Refresh the grid view
                }
            }
        }


        protected void gvBooks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBooks.EditIndex = -1;  // Exit Edit mode
            BindGridView();  // Rebind the GridView to reflect changes
        }

        protected void gvBooks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvBooks.Rows[e.RowIndex];
            int bookID = Convert.ToInt32(gvBooks.DataKeys[e.RowIndex].Value);
            string title = ((TextBox)row.Cells[1].Controls[0]).Text;
            string author = ((TextBox)row.Cells[2].Controls[0]).Text;
            string category = ((TextBox)row.Cells[3].Controls[0]).Text;
            int publishedYear = Convert.ToInt32(((TextBox)row.Cells[4].Controls[0]).Text);
            bool isAvailable = ((CheckBox)row.Cells[5].FindControl("chkIsAvailable")).Checked;
            // Get the availability value

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Books SET Title=@Title, Author=@Author, Category=@Category, PublishedYear=@PublishedYear, IsAvailable=@IsAvailable WHERE BookID=@BookID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Author", author);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@PublishedYear", publishedYear);
                cmd.Parameters.AddWithValue("@IsAvailable", isAvailable);
                cmd.Parameters.AddWithValue("@BookID", bookID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvBooks.EditIndex = -1;  // Exit Edit mode
            BindGridView();  // Rebind the GridView to reflect changes
        }
    }
}
