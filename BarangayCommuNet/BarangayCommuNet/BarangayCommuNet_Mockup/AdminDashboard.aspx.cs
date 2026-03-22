using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class AdminDashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("AdminLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStats();
                LoadWeeklySchedule();
                LoadRecentConcerns();
            }
        }

        private void LoadStats()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();

                // total concerns today
                SqlCommand cmd1 = new SqlCommand(
                    "SELECT COUNT(*) FROM Concerns WHERE CAST(DateSubmitted AS DATE) = CAST(GETDATE() AS DATE)", conn);
                lblTotalConcerns.Text = cmd1.ExecuteScalar().ToString();

                // resolved today
                SqlCommand cmd2 = new SqlCommand(
                    "SELECT COUNT(DISTINCT cr.ConcernID) FROM ConcernResponses cr " +
                    "INNER JOIN (SELECT ConcernID, MAX(DateResponded) AS LastDate FROM ConcernResponses GROUP BY ConcernID) latest " +
                    "ON cr.ConcernID = latest.ConcernID AND cr.DateResponded = latest.LastDate " +
                    "WHERE cr.Status = 'Resolved' AND CAST(cr.DateResponded AS DATE) = CAST(GETDATE() AS DATE)", conn);
                lblResolvedToday.Text = cmd2.ExecuteScalar().ToString();

                // missed collections today
                SqlCommand cmd3 = new SqlCommand(
                    "SELECT COUNT(*) FROM Concerns WHERE Category = 'MissedCollection' " +
                    "AND CAST(DateSubmitted AS DATE) = CAST(GETDATE() AS DATE)", conn);
                lblMissedCollections.Text = cmd3.ExecuteScalar().ToString();

                // total published announcements
                SqlCommand cmd4 = new SqlCommand(
                    "SELECT COUNT(*) FROM Announcements WHERE IsPublished = 1", conn);
                lblAnnouncements.Text = cmd4.ExecuteScalar().ToString();

                // pending
                SqlCommand cmd5 = new SqlCommand(
                    "SELECT COUNT(*) FROM Concerns c WHERE c.ConcernID NOT IN " +
                    "(SELECT ConcernID FROM ConcernResponses WHERE Status IN ('In Progress', 'Resolved'))", conn);
                int pending = Convert.ToInt32(cmd5.ExecuteScalar());
                lblPending.Text = pending.ToString();
                lblPendingBadge.Text = pending + " pending";

                // in progress
                SqlCommand cmd6 = new SqlCommand(
                    "SELECT COUNT(DISTINCT cr.ConcernID) FROM ConcernResponses cr " +
                    "INNER JOIN (SELECT ConcernID, MAX(DateResponded) AS LastDate FROM ConcernResponses GROUP BY ConcernID) latest " +
                    "ON cr.ConcernID = latest.ConcernID AND cr.DateResponded = latest.LastDate " +
                    "WHERE cr.Status = 'In Progress'", conn);
                int inProgress = Convert.ToInt32(cmd6.ExecuteScalar());
                lblInProgress.Text = inProgress.ToString();

                // resolved all time
                SqlCommand cmd7 = new SqlCommand(
                    "SELECT COUNT(DISTINCT cr.ConcernID) FROM ConcernResponses cr " +
                    "INNER JOIN (SELECT ConcernID, MAX(DateResponded) AS LastDate FROM ConcernResponses GROUP BY ConcernID) latest " +
                    "ON cr.ConcernID = latest.ConcernID AND cr.DateResponded = latest.LastDate " +
                    "WHERE cr.Status = 'Resolved'", conn);
                int resolved = Convert.ToInt32(cmd7.ExecuteScalar());
                lblResolved.Text = resolved.ToString();

                lblConcernSub.Text = pending + " pending, " + inProgress + " in progress";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStats error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadWeeklySchedule()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                string query = "SELECT Phase, CollectionType, CollectionDate, Notes, Status " +
                               "FROM CollectionSchedule " +
                               "WHERE CollectionDate BETWEEN CAST(GETDATE() AS DATE) " +
                               "AND DATEADD(DAY, 7, CAST(GETDATE() AS DATE)) " +
                               "ORDER BY CollectionDate ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    rptWeeklySchedule.Visible = false;
                    lblNoSchedule.Visible = true;
                }
                else
                {
                    rptWeeklySchedule.DataSource = dt;
                    rptWeeklySchedule.DataBind();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadWeeklySchedule error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadRecentConcerns()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                string query = @"SELECT TOP 5
                                    c.ConcernID,
                                    u.FirstName + ' ' + u.LastName AS FullName,
                                    c.Category,
                                    c.Phase,
                                    ISNULL(cr.Status, 'Pending') AS CurrentStatus
                                FROM Concerns c
                                INNER JOIN Users u ON c.UserID = u.UserID
                                LEFT JOIN ConcernResponses cr ON cr.ResponseID = (
                                    SELECT TOP 1 ResponseID FROM ConcernResponses
                                    WHERE ConcernID = c.ConcernID
                                    ORDER BY DateResponded DESC
                                )
                                ORDER BY c.DateSubmitted DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    rptRecentConcerns.Visible = false;
                    lblNoConcerns.Visible = true;
                }
                else
                {
                    rptRecentConcerns.DataSource = dt;
                    rptRecentConcerns.DataBind();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadRecentConcerns error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // initials from full name e.g. "Maria Reyes" -> "MR"
        protected string GetInitials(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return "?";
            string[] parts = fullName.Trim().Split(' ');
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return parts[0].Substring(0, 1).ToUpper() + parts[parts.Length - 1].Substring(0, 1).ToUpper();
        }

        // css class for schedule card and pill
        protected string GetStatusClass(string status)
        {
            if (string.IsNullOrEmpty(status)) return "upcoming";
            if (status.ToLower() == "active") return "active";
            if (status.ToLower() == "rescheduled") return "rescheduled";
            return "upcoming";
        }

        // css class for concern card and pill
        protected string GetConcernStatusClass(string status)
        {
            if (string.IsNullOrEmpty(status)) return "pending";
            if (status.ToLower() == "in progress") return "progress";
            if (status.ToLower() == "resolved") return "resolved";
            return "pending";
        }
    }
}