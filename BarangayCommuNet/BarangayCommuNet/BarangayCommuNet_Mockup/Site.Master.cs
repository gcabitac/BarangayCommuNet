using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarangayCommuNet_Mockup
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string script = @"
        function toggleDropdown() {
            var dropdown = document.getElementById('dropdown');
            if (dropdown.className === 'dropdown-hidden') {
                dropdown.className = 'dropdown-visible';
            } else {
                dropdown.className = 'dropdown-hidden';
            }
        }

        document.addEventListener('click', function(e) {
            if (e.target.id !== 'profile-icon') {
                document.getElementById('dropdown').className = 'dropdown-hidden';
            }
        });
    ";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "dropdownScript", script, true);
        }
    }
}