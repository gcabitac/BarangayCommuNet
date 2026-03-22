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
    public partial class LandingPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"SELECT u.UserID, u.FirstName, u.LastName, r.RoleName 
                                 FROM Users u 
                                 INNER JOIN Roles r ON u.RoleID = r.RoleID 
                                 WHERE LTRIM(RTRIM(u.Email)) = LTRIM(RTRIM(@Email))
                                 AND LTRIM(RTRIM(u.PasswordHash)) = LTRIM(RTRIM(@PasswordHash))";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@PasswordHash", txtPassword.Text.Trim());

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Session["UserID"] = reader["UserID"].ToString();
                    Session["FirstName"] = reader["FirstName"].ToString();
                    Session["Username"] = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                    Session["Role"] = reader["RoleName"].ToString();

                    if (reader["RoleName"].ToString().Trim() == "Admin")
                        Response.Redirect("AdminDashboard.aspx");
                    else
                        Response.Redirect("ResidentDashboard.aspx");
                }
                else
                {
                    //lblMessage.Text = $"Email: '{txtEmail.Text.Trim()}' | Pass: '{txtPassword.Text.Trim()}'";
                    lblMessage.Visible = true;
                    lblMessage.Text = "Invalid Email or Password!";
                }
            }
        }
        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResetPassword.aspx");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            activeTab.Value = "register";

            if (string.IsNullOrWhiteSpace(txtRegFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtRegLastName.Text) ||
                string.IsNullOrWhiteSpace(txtRegEmail.Text) ||
                string.IsNullOrWhiteSpace(txtRegPassword.Text) ||
                ddlRole.SelectedValue == "")
            {
                lblRegMessage.Visible = true;
                lblRegMessage.Text = "Please fill in all required fields.";
                return;
            }

            if (txtRegPassword.Text != txtRegConfirm.Text)
            {
                lblRegMessage.Visible = true;
                lblRegMessage.Text = "Passwords do not match.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Email", txtRegEmail.Text.Trim());
                int exists = (int)checkCmd.ExecuteScalar();
                if (exists > 0)
                {
                    lblRegMessage.Visible = true;
                    lblRegMessage.Text = "Account already exists.";
                    return;
                }

                string query = @"INSERT INTO Users (FirstName, LastName, Email, PasswordHash, PhoneNum, RoleID, CreatedAt)
                         VALUES (@FirstName, @LastName, @Email, @Password, @PhoneNum, @RoleID, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FirstName", txtRegFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@LastName", txtRegLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtRegEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtRegPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@PhoneNum", string.IsNullOrWhiteSpace(txtRegPhone.Text) ? (object)DBNull.Value : txtRegPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@RoleID", ddlRole.SelectedValue);

                try
                {
                    cmd.ExecuteNonQuery();
                    lblRegMessage.Visible = true;
                    lblRegMessage.CssClass = "success-msg";
                    lblRegMessage.Text = "Account created! You can now log in.";
                }
                catch (Exception ex)
                {
                    lblRegMessage.Visible = true;
                    lblRegMessage.CssClass = "error-msg";
                    lblRegMessage.Text = "Error: " + ex.Message;
                }
            }

        }
    }
}
