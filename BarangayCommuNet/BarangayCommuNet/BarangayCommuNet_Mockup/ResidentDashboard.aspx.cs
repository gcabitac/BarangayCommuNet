using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class ResidentDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCollectionStatus();
                BindNotifications();

                if (Session["Username"] != null)
                {
                    lblUserName.Text = Session["FirstName"].ToString();
                }
                else
                {
                    lblUserName.Text = "Guest";
                }
            }
        }

        private void BindCollectionStatus()
        {
            string connString = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT TOP 5 Phase, CollectionType, [Status] FROM CollectionSchedule ORDER BY CollectionDate DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                rptCollectionStatus.DataSource = cmd.ExecuteReader();
                rptCollectionStatus.DataBind();
            }
        }

        private void BindNotifications()
        {
            string connString = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

            bool isLoggedIn = Session["UserID"] != null;
            int currentUserId = isLoggedIn ? Convert.ToInt32(Session["UserID"]) : 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query;

                if (isLoggedIn)
                {
                    // Show:
                    //   1. Global broadcast notifications (UserID IS NULL) — announcements, schedules
                    //   2. Notifications explicitly targeted to this user — concern updates
                    // LEFT JOIN NotificationReads to determine per-user read state
                    query = @"
                        SELECT TOP 5
                            n.NotificationID,
                            n.Title,
                            n.Description,
                            n.Category,
                            n.CreatedAt,
                            CASE WHEN nr.UserID IS NOT NULL THEN 1 ELSE 0 END AS IsRead
                        FROM Notifications n
                        LEFT JOIN NotificationReads nr
                            ON nr.NotificationID = n.NotificationID
                            AND nr.UserID = @UserID
                        WHERE
                            (n.Category <> 'Concern' AND n.UserID IS NULL)
                            OR (n.UserID = @UserID)
                        ORDER BY n.CreatedAt DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", currentUserId);

                    rptNotifications.DataSource = cmd.ExecuteReader();
                }
                else
                {
                    // Guest: show only global broadcast notifications (no concerns)
                    query = @"
                        SELECT TOP 5
                            NotificationID,
                            Title,
                            Description,
                            Category,
                            CreatedAt,
                            0 AS IsRead
                        FROM Notifications
                        WHERE UserID IS NULL
                        AND Category <> 'Concern'
                        ORDER BY CreatedAt DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    rptNotifications.DataSource = cmd.ExecuteReader();
                }

                rptNotifications.DataBind();
            }
        }

        protected string GetStatusClass(string status)
        {
            switch (status.ToLower())
            {
                case "last collected": return "<div class='stat-card gray'>";
                case "active": return "<div class='stat-card green'>";
                case "upcoming": return "<div class='stat-card blue'>";
                default: return "<div class='stat-card gray'>";
            }
        }

        // Routes the resident to the correct page based on notification category
        protected string GetCategoryUrl(string category)
        {
            switch (category.ToLower())
            {
                case "garbage collection":
                case "collection":
                case "schedule":
                    return "Collection.aspx";
                case "concern":
                case "concerns":
                    return "TrackConcernPage.aspx";
                case "announcement":
                case "announcements":
                    return "AnnouncementsPage.aspx";
                default:
                    return "AnnouncementsPage.aspx";
            }
        }

        // Returns CSS class for the notification item based on read state
        protected string GetNotifItemClass(object isReadObj)
        {
            bool isRead = Convert.ToBoolean(isReadObj);
            return isRead ? "rd-notif-item rd-notif-link rd-notif-read"
                          : "rd-notif-item rd-notif-link rd-notif-unread";
        }

        public string GetRelativeTime(DateTime dt)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60) return "Just now";
            if (delta < 120) return "1 minute ago";
            if (delta < 2700) return ts.Minutes + " minutes ago";
            if (dt.Date == DateTime.Today) return dt.ToString("h:mm tt");
            if (dt.Date == DateTime.Today.AddDays(-1)) return "Yesterday";

            return dt.ToString("MMM dd");
        }
    }
}