using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace BarangayCommuNet_Mockup
{
    public partial class AnnouncementsPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAnnouncements();
            }
        }

        private void LoadAnnouncements()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                // ImagePath is now included so the image column can render it
                string query = "SELECT AnnouncementID, Title, Content, Category, ImagePath, PublishedAt " +
                               "FROM Announcements WHERE IsPublished = 1 ORDER BY PublishedAt DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptAnnouncements.DataSource = dt;
                rptAnnouncements.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadAnnouncements error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // cuts long content down to a short excerpt for the row
        protected string TruncateText(string text, int max)
        {
            if (string.IsNullOrEmpty(text)) return "";
            if (text.Length > max)
                return text.Substring(0, max) + "...";
            return text;
        }

        // maps category to a short css class suffix used for badge color and image placeholder
        protected string GetBadgeClass(string category)
        {
            if (string.IsNullOrEmpty(category)) return "general";
            string cat = category.ToLower();

            if (cat.Contains("garbage")) return "gc";
            if (cat.Contains("health")) return "health";
            if (cat.Contains("event")) return "events";
            if (cat.Contains("advisory")) return "advisory";
            return "general";
        }

        // if the DB has an image path, render a real <img>
        // if not, render a colored placeholder div so the column never looks empty
        protected string BuildImageOrPlaceholder(object imagePath, string category)
        {
            string path = imagePath != null ? imagePath.ToString().Trim() : "";

            if (!string.IsNullOrEmpty(path))
            {
                return "<img src=\"" + path + "\" alt=\"\" style=\"width:100%;height:100%;object-fit:cover;display:block;\" />";
            }

            // fallback placeholder initials per category
            string label = "GN";
            string cat = category.ToLower();
            if (cat.Contains("garbage")) label = "GC";
            if (cat.Contains("health")) label = "HS";
            if (cat.Contains("event")) label = "EV";
            if (cat.Contains("advisory")) label = "AD";

            return "<div class=\"ann-img-placeholder\">" + label + "</div>";
        }
    }
}