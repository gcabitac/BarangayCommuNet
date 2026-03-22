using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class ManageGarbageSchedule : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("LandingPage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                string query = "SELECT ScheduleID, CollectionDate, Phase, CollectionType, Notes, Status " +
                               "FROM CollectionSchedule ORDER BY CollectionDate ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvSchedule.DataSource = dt;
                gvSchedule.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("BindGrid error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        protected void calSchedule_SelectionChanged(object sender, EventArgs e)
        {
            lblSelectedDate.Text = "Selected: " + calSchedule.SelectedDate.ToString("MMMM dd, yyyy");
            lblMessage.Text = "";
        }

        protected void calSchedule_DayRender(object sender, DayRenderEventArgs e)
        {
            // reserved for future day highlighting logic
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (calSchedule.SelectedDate == DateTime.MinValue)
            {
                lblMessage.CssClass = "ms-msg ms-msg-error";
                lblMessage.Text = "Please select a date on the calendar first.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhase.Text))
            {
                lblMessage.CssClass = "ms-msg ms-msg-error";
                lblMessage.Text = "Phase is required.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                string query = "INSERT INTO CollectionSchedule (CreatedBy, CollectionDate, CollectionType, Phase, Notes, Status) " +
                               "VALUES (@CreatedBy, @CollectionDate, @CollectionType, @Phase, @Notes, @Status)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserID"]);
                cmd.Parameters.AddWithValue("@CollectionDate", calSchedule.SelectedDate);
                cmd.Parameters.AddWithValue("@CollectionType", ddlCollectionType.SelectedValue);
                cmd.Parameters.AddWithValue("@Phase", txtPhase.Text.Trim());
                cmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();

                NotificationHelper.AddScheduleNotification(
                    txtPhase.Text.Trim(),
                    ddlCollectionType.SelectedValue,
                    ddlStatus.SelectedValue
                );

                lblMessage.CssClass = "ms-msg ms-msg-success";
                lblMessage.Text = "Schedule added for " + calSchedule.SelectedDate.ToString("MMMM dd, yyyy") + ".";

                txtPhase.Text = "";
                txtNotes.Text = "";
                ddlCollectionType.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;

                BindGrid();
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "ms-msg ms-msg-error";
                lblMessage.Text = "Something went wrong. Please try again.";
                System.Diagnostics.Debug.WriteLine("btnAdd error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            BindGrid();
            lblMessage.CssClass = "ms-msg ms-msg-success";
            lblMessage.Text = "All changes saved successfully.";
        }

        protected void gvSchedule_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSchedule.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvSchedule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSchedule.EditIndex = -1;
            BindGrid();
        }

        protected void gvSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int scheduleID = Convert.ToInt32(gvSchedule.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvSchedule.Rows[e.RowIndex];

            // Columns: 0=ScheduleID(readonly), 1=CollectionDate, 2=Phase, 3=CollectionType, 4=Notes, 5=Status, 6=Commands
            string date = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string phase = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string type = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string notes = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();

            DropDownList ddlEditStatus = (DropDownList)row.FindControl("ddlEditStatus");
            string status = ddlEditStatus != null ? ddlEditStatus.SelectedValue : "Upcoming";

            // Parse date safely
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
            {
                lblMessage.CssClass = "ms-msg ms-msg-error";
                lblMessage.Text = "Invalid date format. Please use a valid date.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                string query = "UPDATE CollectionSchedule SET CollectionDate=@Date, Phase=@Phase, " +
                               "CollectionType=@Type, Notes=@Notes, Status=@Status, UpdatedAt=GETDATE() " +
                               "WHERE ScheduleID=@ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Date", parsedDate);
                cmd.Parameters.AddWithValue("@Phase", phase);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Notes", notes);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ID", scheduleID);

                conn.Open();
                cmd.ExecuteNonQuery();

                NotificationHelper.AddScheduleNotification(phase, type, status);

                gvSchedule.EditIndex = -1;
                BindGrid();

                lblMessage.CssClass = "ms-msg ms-msg-success";
                lblMessage.Text = "Schedule updated successfully.";
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "ms-msg ms-msg-error";
                lblMessage.Text = "Update failed. Please try again.";
                System.Diagnostics.Debug.WriteLine("RowUpdating error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        protected void gvSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int scheduleID = Convert.ToInt32(gvSchedule.DataKeys[e.RowIndex].Value);

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM CollectionSchedule WHERE ScheduleID=@ID", conn);
                cmd.Parameters.AddWithValue("@ID", scheduleID);

                conn.Open();
                cmd.ExecuteNonQuery();

                BindGrid();

                lblMessage.CssClass = "ms-msg ms-msg-success";
                lblMessage.Text = "Schedule entry deleted.";
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "ms-msg ms-msg-error";
                lblMessage.Text = "Delete failed. Please try again.";
                System.Diagnostics.Debug.WriteLine("RowDeleting error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        protected string GetStatusClass(string status)
        {
            if (string.IsNullOrEmpty(status)) return "upcoming";
            if (status.ToLower() == "active") return "active";
            if (status.ToLower() == "rescheduled") return "rescheduled";
            return "upcoming";
        }
    }
}