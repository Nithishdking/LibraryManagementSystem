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


        // Load available books from the database
        private void LoadAvailableBooks()
        {
            string query = "SELECT BookId, Title, Author, Category, PublishedYear FROM Books WHERE IsAvailable = 1";

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
            // Queries for updating Books table and inserting into BorrowRecords
           // lblWelcome.Text = GetUserIDByUsername(Session["Username"].ToString()).ToString();
            string updateBookQuery = "UPDATE Books SET IsAvailable = 0 WHERE BookId = @BookId";
            string insertBorrowRecordQuery = @"
                INSERT INTO BorrowRecords (BookId, UserId, BorrowDate, ReturnDate, IsReturned)
                VALUES (@BookId, @UserId, GETDATE(), DATEADD(DAY, 14, GETDATE()), 0)";

             //lblWelcome.Text = GetUserIDByUsername(Session["Username"].ToString()).ToString() + "9";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    //lblWelcome.Text = GetUserIDByUsername(Session["Username"].ToString()).ToString() + "10";
                    try
                    {
                        // Update the Books table to mark the book as borrowed
                        SqlCommand updateBookCmd = new SqlCommand(updateBookQuery, conn, transaction);
                        updateBookCmd.Parameters.AddWithValue("@BookId", bookID);
                        updateBookCmd.ExecuteNonQuery();
                        //lblWelcome.Text = GetUserIDByUsername(Session["Username"].ToString()).ToString() + "11";
                        // Insert a new record into BorrowRecords table
                        SqlCommand insertBorrowCmd = new SqlCommand(insertBorrowRecordQuery, conn, transaction);
                        insertBorrowCmd.Parameters.AddWithValue("@BookId", bookID);
                        //lblWelcome.Text = GetUserIDByUsername(Session["Username"].ToString()).ToString()+"8";
                        insertBorrowCmd.Parameters.AddWithValue("@UserId", GetUserIDByUsername(Session["Username"].ToString())); 
                        // Method to get current user ID
                        insertBorrowCmd.ExecuteNonQuery();
                        //lblWelcome.Text = GetUserIDByUsername(Session["Username"].ToString()).ToString()+"00";

                        // Commit the transaction
                        transaction.Commit();

                        // Refresh the available books list after borrowing
                        LoadAvailableBooks();

                        // Display a success message
                        Response.Write("<script>alert('Book borrowed successfully!');</script>");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of error
                        transaction.Rollback();
                        Response.Write($"<script>alert('Error borrowing book: {ex.Message}');</script>");
                        //lblWelcome.Text = ex.Message;
                    }
                }
            }
        }

    }
}
