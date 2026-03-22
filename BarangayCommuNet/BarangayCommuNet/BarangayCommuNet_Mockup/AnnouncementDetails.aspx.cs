using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace BarangayCommuNet_Mockup
{
    public partial class AnnouncementDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idParam = Request.QueryString["id"];

                if (idParam != null)
                {
                    int annID = Convert.ToInt32(idParam);
                    LoadAnnouncement(annID);
                    LoadRelated(annID);
                }
                else
                {
                    pnlAnnouncement.Visible = false;
                    pnlNotFound.Visible = true;
                }
            }
        }

        private void LoadAnnouncement(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                string query = "SELECT Title, Content, Category, PublishedAt, ImagePath " +
                               "FROM Announcements WHERE AnnouncementID = @ID AND IsPublished = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    litTitle.Text = dr["Title"].ToString();
                    litCategory.Text = dr["Category"].ToString();
                    litDate.Text = Convert.ToDateTime(dr["PublishedAt"]).ToString("MMMM dd, yyyy");

                    if (dr["ImagePath"] != DBNull.Value && dr["ImagePath"].ToString().Trim() != "")
                    {
                        imgAnnouncement.ImageUrl = dr["ImagePath"].ToString();
                        imgAnnouncement.Visible = true;
                    }

                    // wrap content paragraphs so line breaks render properly
                    string content = dr["Content"].ToString();
                    content = System.Web.HttpUtility.HtmlEncode(content);
                    content = "<p>" + content.Replace("\n", "</p><p>") + "</p>";
                    litContent.Text = content;
                }
                else
                {
                    pnlAnnouncement.Visible = false;
                    pnlNotFound.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadAnnouncement error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadRelated(int currentID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                // show 3 other announcements at the bottom
                string query = "SELECT TOP 3 AnnouncementID, Title, PublishedAt FROM Announcements " +
                               "WHERE IsPublished = 1 AND AnnouncementID <> @ID ORDER BY PublishedAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", currentID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptRelated.DataSource = dt;
                rptRelated.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadRelated error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}