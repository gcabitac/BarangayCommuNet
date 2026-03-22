using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class AdminConcerns : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("LandingPage.aspx");
            }

            if (!IsPostBack)
            {
                BindConcerns();
            }
        }

        private void BindConcerns()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
            SELECT 
                C.ConcernID, 
                C.UserID,
                (U.FirstName + ' ' + U.LastName) AS FullName, 
                C.Category, 
                C.Description,
                C.PhotoPath,
                C.DateSubmitted, 
                ISNULL(R.Status, 'Pending') AS CurrentStatus,
                ISNULL(R.ResponseText, '') AS LatestResponse
            FROM Concerns C
            INNER JOIN Users U ON C.UserID = U.UserID
            LEFT JOIN (
                SELECT ConcernID, Status, ResponseText, 
                       ROW_NUMBER() OVER (PARTITION BY ConcernID ORDER BY DateResponded DESC) as rn
                FROM ConcernResponses
            ) R ON C.ConcernID = R.ConcernID AND R.rn = 1
            ORDER BY C.DateSubmitted DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvConcerns.DataSource = dt;
                gvConcerns.DataBind();
            }
        }

        protected void gvConcerns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                HiddenField hf = (HiddenField)e.Row.FindControl("hfCurrentStatus");

                if (ddlStatus != null && hf != null)
                {
                    ddlStatus.SelectedValue = hf.Value;

                    switch (hf.Value)
                    {
                        case "Pending":
                            ddlStatus.Style.Add("background-color", "#ffe5e5");
                            ddlStatus.Style.Add("color", "#b91c1c");
                            break;
                        case "In Progress":
                            ddlStatus.Style.Add("background-color", "#fffbeb");
                            ddlStatus.Style.Add("color", "#92400e");
                            break;
                        case "Resolved":
                            ddlStatus.Style.Add("background-color", "#f0fdf4");
                            ddlStatus.Style.Add("color", "#166534");
                            break;
                    }
                }
            }
        }

        protected void gvConcerns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SaveResponse")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int concernId = Convert.ToInt32(gvConcerns.DataKeys[rowIndex].Value);

                GridViewRow row = gvConcerns.Rows[rowIndex];
                DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
                TextBox txtResponse = (TextBox)row.FindControl("txtAdminResponse");

                string category = row.Cells[2].Text;

                // Retrieve the hidden resident UserID for this concern row
                HiddenField hfResidentId = (HiddenField)row.FindControl("hfResidentUserID");
                int residentUserId = Convert.ToInt32(hfResidentId.Value);

                SaveToDatabase(concernId, ddlStatus.SelectedValue, txtResponse.Text, category, residentUserId);
            }
        }

        private void SaveToDatabase(int concernId, string status, string responseText, string category, int residentUserId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = @"INSERT INTO ConcernResponses (ConcernID, UserID, ResponseText, Status, DateResponded) 
                            VALUES (@CID, @UID, @Text, @Status, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CID", concernId);
                    cmd.Parameters.AddWithValue("@UID", Session["UserID"].ToString());
                    cmd.Parameters.AddWithValue("@Text", responseText);
                    cmd.Parameters.AddWithValue("@Status", status);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    // Pass residentUserId so the notification is targeted to that resident only
                    NotificationHelper.AddConcernNotification(concernId.ToString(), status, category, residentUserId);

                    lblAdminMsg.Text = "Status and Response updated for Concern #" + concernId;
                    lblAdminMsg.ForeColor = System.Drawing.Color.Green;

                    BindConcerns();
                }
            }
            catch (Exception ex)
            {
                lblAdminMsg.Text = "Error: " + ex.Message;
                lblAdminMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected string GetPhotoUrl(object photoPath)
        {
            if (photoPath == DBNull.Value || string.IsNullOrEmpty(photoPath?.ToString()))
                return "";
            return ResolveUrl("~/" + photoPath.ToString());
        }
    }
}