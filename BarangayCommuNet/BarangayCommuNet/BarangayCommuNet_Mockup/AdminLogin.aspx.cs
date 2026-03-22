using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("LandingPage.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminDashboard.aspx");
        }
    }
}