<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="BarangayCommuNet_Mockup.LandingPage" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="auth-section">

        <asp:HiddenField ID="activeTab" runat="server" Value="login" />

        <div id="auth-branding">
            <img src="Images/CommuNetLogoLight.png" alt="Logo" />
            <h2>BARANGAY COMMUNET</h2>
            <p>Efficiency in Motion, Community in Mind.</p>
            <div class="branding-feature">
                <div class="branding-dot"></div>
                <span>Submit concerns online</span>
            </div>
            <div class="branding-feature">
                <div class="branding-dot"></div>
                <span>Track your concerns</span>
            </div>
            <div class="branding-feature">
                <div class="branding-dot"></div>
                <span>Stay updated with announcements</span>
            </div>
            <div class="branding-feature">
                <div class="branding-dot"></div>
                <span>Access collection schedules</span>
            </div>
        </div>

        <div id="auth-forms">
            <div id="auth-tabs">
                <button class="auth-tab active" onclick="showTab('login'); return false;">Login</button>
                <button class="auth-tab" onclick="showTab('register'); return false;">Register</button>
            </div>

            <div id="login-form" class="auth-form">
                <h3>Welcome Back</h3>
                <p class="auth-subtitle">Sign in to your account to continue.</p>
                <div class="form-group">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your email" />
                </div>
                <div class="form-group">
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password" />
                </div>
                <asp:Label ID="lblMessage" runat="server" CssClass="error-msg" Visible="false" /><br />
                <asp:LinkButton ID="lnkForgotPassword" runat="server" OnClick="lnkForgotPassword_Click">Forgot Password</asp:LinkButton>
                <br /><br />
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-full" OnClick="btnLogin_Click" UseSubmitBehavior="true" />
                
            </div>

            <div id="register-form" class="auth-form" style="display:none;">
                <h3>Create Account</h3>
                <p class="auth-subtitle">Complete the following information.</p>
                <div class="form-row">
                    <div class="form-group">
                        <label>First Name</label>
                        <asp:TextBox ID="txtRegFirstName" runat="server" CssClass="form-control"/>
                    </div>
                    <div class="form-group">
                        <label>Last Name</label>
                        <asp:TextBox ID="txtRegLastName" runat="server" CssClass="form-control"/>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Email</label>
                        <asp:TextBox ID="txtRegEmail" runat="server" CssClass="form-control"/>
                    </div>
                    <div class="form-group">
                        <label>Phone Number</label>
                        <asp:TextBox ID="txtRegPhone" runat="server" CssClass="form-control" placeholder="09xxxxxxxxx" />
                    </div>
                </div>
                <div class="form-group">
                    <label>Role</label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- Select Role --</asp:ListItem>
                        <asp:ListItem Value="1">Admin</asp:ListItem>
                        <asp:ListItem Value="2">Resident</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Password</label>
                        <asp:TextBox ID="txtRegPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Choose a password" />
                    </div>
                    <div class="form-group">
                        <label>Confirm Password</label>
                        <asp:TextBox ID="txtRegConfirm" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm password" />
                    </div>
                </div>
                <asp:Label ID="lblRegMessage" runat="server" CssClass="error-msg" Visible="false" />
                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary btn-full" OnClick="btnRegister_Click" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showTab(tab) {
            document.getElementById('login-form').style.display = tab === 'login' ? 'block' : 'none';
            document.getElementById('register-form').style.display = tab === 'register' ? 'block' : 'none';
            var tabs = document.querySelectorAll('.auth-tab');
            tabs.forEach(function (t) { t.classList.remove('active'); });
            event.target.classList.add('active');
            document.getElementById('<%= activeTab.ClientID %>').value = tab;
        }

        window.addEventListener('DOMContentLoaded', function () {
            var hiddenField = document.getElementById('<%= activeTab.ClientID %>');
            var currentTab = hiddenField ? hiddenField.value : 'login';
            if (currentTab === 'register') {
                document.getElementById('login-form').style.display = 'none';
                document.getElementById('register-form').style.display = 'block';
                document.querySelectorAll('.auth-tab')[1].classList.add('active');
                document.querySelectorAll('.auth-tab')[0].classList.remove('active');
            }
        });
    </script>
    <script>document.body.classList.add('page-landing');</script>
</asp:Content>