using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BarangayCommuNet_Mockup
{
    public static class NotificationHelper
    {
        // 1. FOR SCHEDULE UPDATES (broadcast to all — no UserID)
        public static void AddScheduleNotification(string phase, string type, string status)
        {
            string title = $"Schedule Update: {phase}";
            string desc = $"{type} collection is now marked as '{status}'. Check the full schedule for details.";
            SaveToDb(title, desc, "Schedule");
        }

        // 2. FOR CONCERNS (targeted to the resident who submitted it)
        public static void AddConcernNotification(string ticketId, string newStatus, string category = "", int residentUserId = 0)
        {
            string title = $"Concern Update #{ticketId}";
            string desc = string.IsNullOrEmpty(category)
                ? $"The status of your reported concern has been updated to: {newStatus}."
                : $"Your concern about \"{category}\" has been updated to: {newStatus}.";

            // Pass residentUserId so the notification is only visible to that resident
            SaveToDb(title, desc, "Concern", residentUserId);
        }

        // 3. FOR ANNOUNCEMENTS (broadcast to all — no UserID)
        public static void AddAnnouncementNotification(string headline)
        {
            string title = "New Announcement";
            string desc = $"A new post was published: \"{headline}\". Click to read more.";
            SaveToDb(title, desc, "Announcement");
        }

        // PRIVATE MASTER METHOD
        // userId = 0 means broadcast (NULL in DB), userId > 0 means targeted to that resident
        private static void SaveToDb(string title, string description, string category, int userId = 0)
        {
            string connString = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    INSERT INTO Notifications (Title, Description, Category, UserID, CreatedAt) 
                    VALUES (@T, @D, @C, @UserID, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@T", title);
                cmd.Parameters.AddWithValue("@D", description);
                cmd.Parameters.AddWithValue("@C", category);

                // NULL = visible to everyone, specific ID = visible only to that resident
                if (userId > 0)
                    cmd.Parameters.AddWithValue("@UserID", userId);
                else
                    cmd.Parameters.AddWithValue("@UserID", DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}