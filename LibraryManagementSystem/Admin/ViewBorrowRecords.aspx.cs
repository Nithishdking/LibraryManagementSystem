using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryManagementSystem.Admin
{
    public partial class ViewBorrowRecords : System.Web.UI.Page
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
                string query = @"
                    SELECT br.BorrowID, u.Username, b.Title AS BookTitle, br.BorrowDate, br.ReturnDate, br.ReturnedDate, br.IsReturned
                    FROM BorrowRecords br
                    JOIN Users u ON br.UserID = u.UserID
                    JOIN Books b ON br.BookID = b.BookID";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvBorrowRecords.DataSource = dt;
                gvBorrowRecords.DataBind();
            }
        }
        protected void gvBorrowRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Format BorrowDate
                DateTime borrowDate;
                if (DateTime.TryParse(e.Row.Cells[3].Text, out borrowDate))
                {
                    e.Row.Cells[3].Text = borrowDate.ToString("yyyy-MM-dd");
                }

                // Format ReturnDate
                DateTime returnDate;
                if (DateTime.TryParse(e.Row.Cells[4].Text, out returnDate))
                {
                    e.Row.Cells[4].Text = returnDate.ToString("yyyy-MM-dd");
                }

                // Format ReturnedDate
                DateTime returnedDate;
                if (DateTime.TryParse(e.Row.Cells[5].Text, out returnedDate))
                {
                    e.Row.Cells[5].Text = returnedDate.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void gvBorrowRecords_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBorrowRecords.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void gvBorrowRecords_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvBorrowRecords.Rows[e.RowIndex];
            int borrowID = Convert.ToInt32(gvBorrowRecords.DataKeys[e.RowIndex].Value);
            bool isReturned = ((CheckBox)row.Cells[6].Controls[0]).Checked;
            string returnedDate = ((TextBox)row.Cells[5].Controls[0]).Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE BorrowRecords SET IsReturned=@IsReturned, ReturnedDate=@ReturnedDate WHERE BorrowID=@BorrowID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IsReturned", isReturned);
                cmd.Parameters.AddWithValue("@ReturnedDate", string.IsNullOrEmpty(returnedDate) ? DBNull.Value : (object)DateTime.Parse(returnedDate));
                cmd.Parameters.AddWithValue("@BorrowID", borrowID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvBorrowRecords.EditIndex = -1;
            BindGridView();
        }

        protected void gvBorrowRecords_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBorrowRecords.EditIndex = -1;
            BindGridView();
        }

        protected void gvBorrowRecords_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int borrowID = Convert.ToInt32(gvBorrowRecords.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM BorrowRecords WHERE BorrowID=@BorrowID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BorrowID", borrowID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            BindGridView();
        }
    }
}
