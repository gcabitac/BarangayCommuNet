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
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
           if(string.IsNullOrWhiteSpace(txtResetUsername.Text)||
                string.IsNullOrWhiteSpace(txtNewPassword.Text)||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                lblMessage.Text = "All fields are required.";
                return;

            }

           if(txtNewPassword.Text != txtConfirmPassword.Text)
            {
                lblMessage.Text = "Passwords do not match";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Users SET PasswordHash=@PasswordHash WHERE Email=@Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PasswordHash", txtNewPassword.Text);
                cmd.Parameters.AddWithValue("@Email", txtResetUsername.Text);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if(rows>0)
                {
                    lblMessage.Text = "Password reset successfully";
                }
                else
                {
                    lblMessage.Text = "Email not found.";
                }
            }
        }
        
    }
}