using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class SubmitConcerns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Resident")
            {
                Response.Redirect("LandingPage.aspx");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginAlert",
                    "alert('You need to login first!'); window.location='LandingPage.aspx';", true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["BrgyCommuNetDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string photoPath = null;
                if (fuPhoto.HasFile)
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                    string ext = Path.GetExtension(fuPhoto.FileName).ToLower();

                    if (!allowedExtensions.Contains(ext))
                    {
                        lblMessage.Text = "Only image files (jpg, png) are allowed.";
                        return;
                    }

                    string uploadFolder = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fuPhoto.FileName);
                    string savePath = Path.Combine(uploadFolder, fileName);
                    fuPhoto.SaveAs(savePath);
                    photoPath = "Uploads/" + fileName;
                }

                string query = @"INSERT INTO Concerns (UserID, Category, Description, PhotoPath, Location, DateSubmitted, Phase)
                                 VALUES (@UserID, @Category, @Description, @PhotoPath, @Location, GETDATE(), @Phase)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"].ToString());
                cmd.Parameters.AddWithValue("@Category", ddlConcernType.SelectedValue);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@PhotoPath", (object)photoPath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Location", txtLocation.Text);
                cmd.Parameters.AddWithValue("@Phase", ddlConcernPhase.SelectedValue);

                try
                {
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Concern submitted successfully!";
                    lblMessage.CssClass = "success-msg";
                    clearForm();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "error-msg";
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void clearForm()
        {
            ddlConcernType.SelectedIndex = 0;
            txtDescription.Text = "";
            txtLocation.Text = "";
        }
    }
}