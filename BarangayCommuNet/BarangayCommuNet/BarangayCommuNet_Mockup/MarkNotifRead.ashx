<%@ WebHandler Language="C#" Class="MarkNotifRead" %>

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

public class MarkNotifRead : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        // Must be logged in
        if (context.Session["UserID"] == null)
        {
            context.Response.StatusCode = 401;
            return;
        }

        // Parse notifId from query string
        int notifId;
        if (!int.TryParse(context.Request.QueryString["notifId"], out notifId))
        {
            context.Response.StatusCode = 400;
            return;
        }

        int userId = Convert.ToInt32(context.Session["UserID"]);
        string connString = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            // Safe upsert: insert only if this user hasn't already read this notification
            string query = @"
                IF NOT EXISTS (
                    SELECT 1 FROM NotificationReads
                    WHERE UserID = @UserID AND NotificationID = @NotifID
                )
                INSERT INTO NotificationReads (UserID, NotificationID)
                VALUES (@UserID, @NotifID)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@NotifID", notifId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        context.Response.StatusCode = 200;
    }

    // Handler can be reused across requests
    public bool IsReusable { get { return true; } }
}
