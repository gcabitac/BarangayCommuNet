<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    CodeBehind="AdminLogin.aspx.cs" Inherits="BarangayCommuNet_Mockup.AdminLogin" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="landing-hero">
        <div id="hero-logo">
            <img src="Images/CommuNetLogo.png" alt="Logo" id="logo" />
        </div>
    </div>

    <div id="auth-section">
        <div id="admin-login-wrapper" class="admin-theme">
            <div id="login-form" class="auth-form">
                <h3>Admin Login</h3>
                <div class="form-group">
                    <label>Username</label>
                    <asp:TextBox ID="txtLoginUsername" runat="server" CssClass="form-control" placeholder="Enter username" />
                </div>
                <div class="form-group">
                    <label>Password</label>
                    <asp:TextBox ID="txtLoginPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter password" />
                </div>
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                <div class="form-group">
                    <asp:HyperLink ID="lnkForgotPassword" runat="server" NavigateUrl="~/ResetPassword.aspx" CssClass="link">
                        Forgot Password?
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
