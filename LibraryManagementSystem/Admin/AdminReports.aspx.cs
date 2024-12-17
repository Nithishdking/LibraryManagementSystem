using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
                ShowBookReport(sender, e); // Load Book Borrow Report by default
            }
        }

        // Show Book Borrow Report
        protected void ShowBookReport(object sender, EventArgs e)
        {
            pnlBookReport.Visible = true;
            pnlUserReport.Visible = false;

            gvBookBorrowReport.DataSource = GetBookBorrowReportData();
            gvBookBorrowReport.DataBind();
        }

        // Show User Borrow Report
        protected void ShowUserReport(object sender, EventArgs e)
        {
            pnlBookReport.Visible = false;
            pnlUserReport.Visible = true;

            gvUserBorrowReport.DataSource = GetUserBorrowReportData();
            gvUserBorrowReport.DataBind();
        }

        // Download Book Borrow Report
        protected void DownloadBookReport(object sender, EventArgs e)
        {
            DataTable dt = GetBookBorrowReportData();
            ExportToExcel(dt, "BookBorrowReport");
        }

        // Download User Borrow Report
        protected void DownloadUserReport(object sender, EventArgs e)
        {
            DataTable dt = GetUserBorrowReportData();
            ExportToExcel(dt, "UserBorrowReport");
        }

        // Fetch Book Borrow Report Data
        private DataTable GetBookBorrowReportData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
    SELECT 
    br.BorrowID AS [Borrow ID],
    u.Username AS [Username],
    b.Title AS [Book Title],
    CONVERT(VARCHAR(10), br.BorrowDate, 120) AS [Borrow Date],
    CONVERT(VARCHAR(10), br.ReturnDate, 120) AS [Due Date],
    CONVERT(VARCHAR(10), br.ReturnedDate, 120) AS [Returned Date],
    CASE 
        WHEN br.IsReturned = 1 THEN 'Returned'
        ELSE 'Not Returned'
    END AS [Status]
FROM 
    BorrowRecords br
JOIN 
    Users u ON br.UserID = u.UserID
JOIN 
    Books b ON br.BookID = b.BookID
WHERE 
    u.IsApproved = 1
ORDER BY 
    br.BorrowID ASC;";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Fetch User Borrow Report Data
        private DataTable GetUserBorrowReportData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                   SELECT 
    u.UserID AS [User ID],
    u.Username AS [Username],
    u.Email AS [Email],
    COUNT(br.BorrowID) AS [Total Books Borrowed],
    STRING_AGG(b.Title, ', ') AS [Borrowed Books] -- Combine book titles into a single string
FROM 
    Users u
LEFT JOIN 
    BorrowRecords br ON u.UserID = br.UserID
LEFT JOIN 
    Books b ON br.BookID = b.BookID
WHERE 
    ISNULL(u.IsApproved, 0) = 1 -- Only approved users
GROUP BY 
    u.UserID, u.Username, u.Email
ORDER BY 
    u.UserID ASC;
";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Export DataTable to Excel
        private void ExportToExcel(DataTable dt, string fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename={fileName}.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView gv = new GridView
                {
                    DataSource = dt,
                    AutoGenerateColumns = true
                };
                gv.DataBind();

                gv.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required to avoid runtime error for exporting GridView
        }
    }
}
