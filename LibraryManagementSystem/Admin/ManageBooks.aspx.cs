using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string category = txtCategory.Text;
            int publishedYear = Convert.ToInt32(txtPublishedYear.Text);
            int copies = Convert.ToInt32(txtCopies.Text);

            // Check if the book title already exists
            if (CheckIfTitleExists(title))
            {
                lblMessage.Text = "The book title is already entered.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Books (Title, Author, Category, PublishedYear, Copies) " +
                                   "VALUES (@Title, @Author, @Category, @PublishedYear, @Copies)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@PublishedYear", publishedYear);
                    cmd.Parameters.AddWithValue("@Copies", copies);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    lblMessage.Text = "Book added successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }

                // Clear the input fields
                txtTitle.Text = string.Empty;
                txtAuthor.Text = string.Empty;
                txtCategory.Text = string.Empty;
                txtPublishedYear.Text = string.Empty;
                txtCopies.Text = string.Empty;

                // Refresh the GridView
                BindGridView();
            }
            catch (SqlException ex)
            {
                // If it's a unique constraint violation, show a custom error message
                if (ex.Number == 2627)  // Error number for unique constraint violation
                {
                    lblMessage.Text = "The book title is already entered.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    
                }
                else
                {
                    // Handle other SQL exceptions
                    lblMessage.Text = "An error occurred while adding the book.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                
            }
        }

        private bool CheckIfTitleExists(string title)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Books WHERE Title = @Title";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();

                return count > 0; // Returns true if the title already exists
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
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
            txtSearch.Text = string.Empty;
            BindGridView();
        }

        protected void gvBooks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBooks.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void gvBooks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int bookID = Convert.ToInt32(gvBooks.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM BorrowRecords WHERE BookId = @BookID AND IsReturned = 0";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@BookID", bookID);

                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                conn.Close();

                if (count > 0)
                {
                    lblMessage.Text = "Cannot delete this book. It is currently borrowed.";
                    lblMessage.Visible = true;
                }
                else
                {
                    string deleteQuery = "DELETE FROM Books WHERE BookID = @BookID";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@BookID", bookID);

                    conn.Open();
                    deleteCmd.ExecuteNonQuery();
                    conn.Close();

                    BindGridView();
                }
            }
        }
        protected void gvBooks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBooks.EditIndex = -1;
            BindGridView();
        }

        protected void gvBooks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvBooks.Rows[e.RowIndex];
            int bookID = Convert.ToInt32(gvBooks.DataKeys[e.RowIndex].Value);
            string title = ((TextBox)row.Cells[1].Controls[0]).Text;
            string author = ((TextBox)row.Cells[2].Controls[0]).Text;
            string category = ((TextBox)row.Cells[3].Controls[0]).Text;
            int publishedYear = Convert.ToInt32(((TextBox)row.Cells[4].Controls[0]).Text);
            int copies = Convert.ToInt32(((TextBox)row.Cells[5].Controls[0]).Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Books SET Title=@Title, Author=@Author, Category=@Category, PublishedYear=@PublishedYear, Copies=@Copies WHERE BookID=@BookID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Author", author);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@PublishedYear", publishedYear);
                cmd.Parameters.AddWithValue("@Copies", copies);
                cmd.Parameters.AddWithValue("@BookID", bookID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvBooks.EditIndex = -1;
            BindGridView();
        }
    }
}
