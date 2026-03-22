using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BarangayCommuNet_Mockup
{
    public partial class TrackConcernPage : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Resident")
            {
                Response.Redirect("LandingPage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadConcerns();
            }
        }

        private void LoadConcerns()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT ConcernID, Category, Description, Status, DateSubmitted, AdminResponse
                 FROM vw_UserConcerns
                 WHERE UserID = @UserID
                 ORDER BY 
                     DateResponded DESC,
                     DateSubmitted DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvConcerns.DataSource = dt;
                gvConcerns.DataBind();

                if (dt.Rows.Count == 0)
                {
                    gvConcerns.EmptyDataText = "No concerns submitted yet.";
                }
            }
        }
    }
}