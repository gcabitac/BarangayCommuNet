using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class AnnouncementManagementPage : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAnnouncements();
            }
        }


        protected string GetStatusClass(string status)
        {
            switch (status.ToLower())
            {
                case "published": return "published";
                case "draft": return "draft";
                case "archived": return "archived";
                default: return "draft";
            }
        }

        private string GetStatusFromIsPublished(object isPublished)
        {
            if (isPublished == DBNull.Value) return "Draft";
            return Convert.ToBoolean(isPublished) ? "Published" : "Draft";
        }

        private void LoadAnnouncements()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT AnnouncementID, Title, Category, IsPublished, PublishedAt 
                                 FROM Announcements 
                                 ORDER BY PublishedAt DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dt.Columns.Add("Status", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["Status"] = GetStatusFromIsPublished(row["IsPublished"]);
                }

                gvAnnouncements.DataSource = dt;
                gvAnnouncements.DataBind();
            }
        }

        private void ClearForm()
        {
            hfAnnouncementID.Value = "";
            txtTitle.Text = "";
            txtContent.Text = "";
            ddlCategory.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            lblMessage.Text = "";
        }

        private void ShowMessage(string text, string color = "#2d6a4f")
        {
            lblMessage.Text = text;
            lblMessage.Style["color"] = color;
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            bool isEdit = !string.IsNullOrEmpty(hfAnnouncementID.Value);

            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtContent.Text))
            {
                ShowMessage("Title and content are required.", "#c0392b");
                return;
            }

            bool isPublished = ddlStatus.SelectedValue == "Published";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                if (isEdit)
                {
                    string update = @"UPDATE Announcements 
                                      SET Title       = @Title,
                                          Category    = @Category,
                                          Content     = @Content,
                                          IsPublished = @IsPublished,
                                          UpdatedAt   = @UpdatedAt
                                      WHERE AnnouncementID = @ID";

                    using (SqlCommand cmd = new SqlCommand(update, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsPublished", isPublished);
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ID", int.Parse(hfAnnouncementID.Value));
                        cmd.ExecuteNonQuery();
                    }

                    if (isPublished)
                    {
                        NotificationHelper.AddAnnouncementNotification(txtTitle.Text.Trim());
                    }

                    ShowMessage("Announcement updated successfully.");
                }
                else
                {
                    int postedBy = Convert.ToInt32(Session["UserID"]);

                    string insert = @"INSERT INTO Announcements 
                                        (PostedBy, Title, Category, Content, IsPublished, PublishedAt, CreatedAt)
                                      VALUES 
                                        (@PostedBy, @Title, @Category, @Content, @IsPublished, @PublishedAt, @CreatedAt)";

                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostedBy", postedBy);
                        cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsPublished", isPublished);
                        cmd.Parameters.AddWithValue("@PublishedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    if (isPublished)
                    {
                        NotificationHelper.AddAnnouncementNotification(txtTitle.Text.Trim());
                    }

                    ShowMessage("Announcement published successfully.");
                }
            }

            ClearForm();
            LoadAnnouncements();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }


        protected void gvAnnouncements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditAnnouncement") return;

            int rowIndex = int.Parse(e.CommandArgument.ToString());
            int id = (int)gvAnnouncements.DataKeys[rowIndex].Value;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Announcements WHERE AnnouncementID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        hfAnnouncementID.Value = dr["AnnouncementID"].ToString();
                        txtTitle.Text = dr["Title"].ToString();
                        txtContent.Text = dr["Content"].ToString();
                        ddlCategory.SelectedValue = dr["Category"].ToString();

                        bool published = Convert.ToBoolean(dr["IsPublished"]);
                        ddlStatus.SelectedValue = published ? "Published" : "Draft";
                    }
                }
            }

            ShowMessage("Editing announcement — make changes and click Publish.");
        }

        protected void gvAnnouncements_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)gvAnnouncements.DataKeys[e.RowIndex].Value;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Announcements WHERE AnnouncementID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            ShowMessage("Announcement deleted.");
            LoadAnnouncements();
        }
    }
}