using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace LibraryManagementSystem.User
{
    public partial class UserDashboard : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        

        protected void Page_Load(object sender, EventArgs e)
       {

            if (!IsPostBack) // Avoid re-binding on postbacks
            {
                LoadAvailableBooks();
            }
            if (Session["Username"] != null)
            {
                  // Display the username
                lblWelcome.Text = $"Welcome, {Session["Username"].ToString()}!";
            }
        }
        protected void ClearSearch(object sender, EventArgs e)
        {
            // Clear the TextBox when the user clicks the "Clear" button
            txtSearch.Text = string.Empty;

            // Optionally, you can also trigger the logic to reload the books or update the search results
            // (For example, if you're filtering books based on the search input, you may need to reload the entire list)
            LoadAvailableBooks();
        }
        protected void SearchBooks(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                // If search text is empty, load all books
                LoadAvailableBooks();
            }
            else
            {
                // Build the SQL query to search across title, author, and category
                string query = "SELECT BookID, Title, Author, Category, PublishedYear FROM Books WHERE Title LIKE @SearchText OR Author LIKE @SearchText OR Category LIKE @SearchText";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    // Add parameter to prevent SQL injection
                    cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvAvailableBooks.DataSource = dt;
                    gvAvailableBooks.DataBind();
                }
            }
        }

        // Load available books from the database
        private void LoadAvailableBooks()
        {
            string query = "SELECT BookId, Title, Author, Category, PublishedYear, Copies FROM Books ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAvailableBooks.DataSource = dt;
                gvAvailableBooks.DataBind();
            }
        }

        // Handle the borrow book action
        protected void gvAvailableBooks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Borrow")
            {
                int bookID = Convert.ToInt32(e.CommandArgument);
                BorrowBook(bookID);
            }
        }

        // Method to update the book's availability and mark it as borrowed
        private int GetUserIDByUsername(string username)
        {
            string query = "SELECT UserId FROM Users WHERE Username = @Username";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result); // Return UserID if found
                }
                else
                {
                    // Handle case where the username does not exist in the database
                    return -1; // Or any other way you want to handle not found
                }
            }
        }

        private void BorrowBook(int bookID)
        {
            string checkCopiesQuery = "SELECT Copies FROM Books WHERE BookId = @BookId";
            string checkExistingBorrowQuery = "SELECT COUNT(*) FROM BorrowRecords WHERE BookId = @BookId AND UserId = @UserId AND IsReturned = 0";
            string updateBookCopiesQuery = "UPDATE Books SET Copies = Copies - 1 WHERE BookId = @BookId";
            string insertBorrowRecordQuery = "INSERT INTO BorrowRecords (BookId, UserId, BorrowDate, ReturnDate, IsReturned) VALUES (@BookId, @UserId, GETDATE(), DATEADD(DAY, 14, GETDATE()), 0)";

            int userId = GetUserIDByUsername(Session["Username"].ToString());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Check if user already borrowed the book
                        SqlCommand checkExistingBorrowCmd = new SqlCommand(checkExistingBorrowQuery, conn, transaction);
                        checkExistingBorrowCmd.Parameters.AddWithValue("@BookId", bookID);
                        checkExistingBorrowCmd.Parameters.AddWithValue("@UserId", userId);
                        int borrowCount = (int)checkExistingBorrowCmd.ExecuteScalar();

                        if (borrowCount > 0)
                        {
                            Response.Write("<script>alert('You have already borrowed this book and not returned it yet.');</script>");
                            transaction.Rollback();
                            return;
                        }

                        // Check copies available
                        SqlCommand checkCopiesCmd = new SqlCommand(checkCopiesQuery, conn, transaction);
                        checkCopiesCmd.Parameters.AddWithValue("@BookId", bookID);
                        int copiesAvailable = (int)checkCopiesCmd.ExecuteScalar();

                        if (copiesAvailable <= 0)
                        {
                            Response.Write("<script>alert('No copies of this book are currently available.');</script>");
                            transaction.Rollback();
                            return;
                        }

                        // Reduce copies in Books table
                        SqlCommand updateBookCmd = new SqlCommand(updateBookCopiesQuery, conn, transaction);
                        updateBookCmd.Parameters.AddWithValue("@BookId", bookID);
                        updateBookCmd.ExecuteNonQuery();

                        // Insert a new record in BorrowRecords table
                        SqlCommand insertBorrowCmd = new SqlCommand(insertBorrowRecordQuery, conn, transaction);
                        insertBorrowCmd.Parameters.AddWithValue("@BookId", bookID);
                        insertBorrowCmd.Parameters.AddWithValue("@UserId", userId);
                        insertBorrowCmd.ExecuteNonQuery();

                        transaction.Commit();

                        LoadAvailableBooks();
                        Response.Write("<script>alert('Book borrowed successfully!');</script>");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                    }
                }
            }
        }

    }
}
