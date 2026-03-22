using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class Collection : Page
    {
        private HashSet<DateTime> _collectionDates = new HashSet<DateTime>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadSchedule();

            LoadCalendarDates();
        }

        protected void ApplyFilters(object sender, EventArgs e)
        {
            LoadSchedule();
        }

        protected void btnClearFilters_Click(object sender, EventArgs e)
        {
            ddlPhaseFilter.SelectedIndex = 0;
            ddlTypeFilter.SelectedIndex = 0;
            ddlStatusFilter.SelectedIndex = 0;
            LoadSchedule();
        }

        // Highlight days that have a scheduled collection
        protected void calSchedule_DayRender(object sender, DayRenderEventArgs e)
        {
            if (_collectionDates.Contains(e.Day.Date))
            {
                e.Cell.BackColor = Color.FromArgb(0xD8, 0xF3, 0xDC);
                e.Cell.Font.Bold = true;
            }
        }

        // Load collection dates for the calendar's visible month
        private void LoadCalendarDates()
        {
            DateTime visibleMonth = calSchedule.VisibleDate == DateTime.MinValue
                ? DateTime.Today
                : calSchedule.VisibleDate;

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

            string sql = @"
                SELECT CollectionDate
                FROM   CollectionSchedule
                WHERE  MONTH(CollectionDate) = @Month
                  AND  YEAR(CollectionDate)  = @Year";

            _collectionDates.Clear();

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Month", visibleMonth.Month);
                cmd.Parameters.AddWithValue("@Year", visibleMonth.Year);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        _collectionDates.Add(Convert.ToDateTime(rdr["CollectionDate"]).Date);
                }
            }
        }

        // Load the schedule table based on filters
        private void LoadSchedule()
        {
            string phase = ddlPhaseFilter.SelectedValue;
            string month = ddlMonthFilter.SelectedValue;
            string type = ddlTypeFilter.SelectedValue;
            string status = ddlStatusFilter.SelectedValue;

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT ScheduleID, CollectionDate, Phase, CollectionType, Notes, Status
                FROM   CollectionSchedule
                WHERE  1 = 1");

            if (!string.IsNullOrEmpty(phase))
                sql.Append(" AND Phase = @Phase");
            if (!string.IsNullOrEmpty(month))
                sql.Append(" AND MONTH(CollectionDate) = @Month");
            if (!string.IsNullOrEmpty(type))
                sql.Append(" AND CollectionType = @Type");
            if (!string.IsNullOrEmpty(status))
                sql.Append(" AND Status = @Status");

            sql.Append(" ORDER BY CollectionDate ASC");

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
            {
                if (!string.IsNullOrEmpty(phase))
                    cmd.Parameters.AddWithValue("@Phase", phase);
                if (!string.IsNullOrEmpty(month))
                    cmd.Parameters.AddWithValue("@Month", int.Parse(month));
                if (!string.IsNullOrEmpty(type))
                    cmd.Parameters.AddWithValue("@Type", type);
                if (!string.IsNullOrEmpty(status))
                    cmd.Parameters.AddWithValue("@Status", status);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
        }

        protected string GetStatusClass(string status)
        {
            switch (status?.ToLower())
            {
                case "active": return "active";
                case "upcoming": return "upcoming";
                case "rescheduled": return "rescheduled";
                default: return "upcoming";
            }
        }
    }
}