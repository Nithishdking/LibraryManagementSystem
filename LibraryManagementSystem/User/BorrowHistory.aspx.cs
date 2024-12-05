using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace LibraryManagementSystem.User
{
    public partial class BorrowHistory : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBorrowRecords();
            }

        }

        // Load Borrowed Books for Logged-in User
        private void LoadBorrowRecords()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT b.BookId, b.Title, b.Author, b.Category, br.BorrowDate, br.ReturnDate, br.ReturnedDate, br.IsReturned
                    FROM BorrowRecords br
                    INNER JOIN Books b ON br.BookId = b.BookId
                    WHERE br.UserId = @UserId";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@UserId", GetLoggedInUserID()); // Replace with your method to get logged-in user ID
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvBorrowRecords.DataSource = dt;
                gvBorrowRecords.DataBind();
            }
        }

        // Handle Return Button Click
        protected void gvBorrowRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Return")
            {
                int bookID = Convert.ToInt32(e.CommandArgument);
                ReturnBook(bookID);
            }
        }

        // Return Book
        private void ReturnBook(int bookID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string updateBorrowQuery = @"
                    UPDATE BorrowRecords
                    SET IsReturned = 1 , ReturnedDate = GETDATE()
                    WHERE BookId = @BookId AND UserId = @UserId AND IsReturned = 0";

                string updateBookQuery = @"
                    UPDATE Books
                    SET IsAvailable = 1 
                    WHERE BookId = @BookId";

                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Update BorrowRecords
                        SqlCommand cmdBorrow = new SqlCommand(updateBorrowQuery, conn, transaction);
                        cmdBorrow.Parameters.AddWithValue("@BookId", bookID);
                        cmdBorrow.Parameters.AddWithValue("@UserId", GetLoggedInUserID());
                        cmdBorrow.ExecuteNonQuery();

                        // Update Books
                        SqlCommand cmdBook = new SqlCommand(updateBookQuery, conn, transaction);
                        cmdBook.Parameters.AddWithValue("@BookId", bookID);
                        cmdBook.ExecuteNonQuery();

                        transaction.Commit();
                        LoadBorrowRecords(); // Reload Borrow Records
                        Response.Write("<script>alert('Book returned successfully!');</script>");
                    }
                    catch
                    {
                        transaction.Rollback();
                        Response.Write("<script>alert('Error returning book!');</script>");
                    }
                }
            }
        }

        // Dummy method to get logged-in user ID
        private int GetLoggedInUserID()
        {
            // Retrieve the User ID from the session
            if (Session["UserId"] != null)
            {
                return Convert.ToInt32(Session["UserId"]);
            }
            else
            {
                throw new Exception("User is not logged in.");
            }
        }

    }
}