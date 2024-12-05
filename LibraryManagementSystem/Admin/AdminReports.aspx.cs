using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryManagementSystem
{
    public partial class AdminReports : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBorrowedBooksReport();
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

        private void LoadBorrowedBooksReport()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        br.BorrowID,
                        u.FullName AS Username,
                        b.Title AS BookTitle,
                        br.BorrowDate,
                        br.ReturnDate,
                        br.ReturnedDate,
                        CASE 
                            WHEN br.IsReturned = 1 THEN 'Returned'
                            ELSE 'Not Returned'
                        END AS Status
                    FROM 
                        BorrowRecords br
                    JOIN 
                        Users u ON br.UserID = u.UserID
                    JOIN 
                        Books b ON br.BookID = b.BookID
                    ORDER BY 
                        br.BorrowID ASC;";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);



                gvReport.DataSource = dt;
                gvReport.DataBind();
            }
        }
    }
}
