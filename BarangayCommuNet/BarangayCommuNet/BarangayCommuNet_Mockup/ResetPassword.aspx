<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="BarangayCommuNet_Mockup.ResetPassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="admin-login-wrapper" class="admin-theme">
        <h3>Reset Your Password</h3>
        <div class="form-group">
            <label>Email</label>
            <asp:TextBox ID="txtResetUsername" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label>New Password</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" />
        </div>
        <div class="form-group">
            <label>Confirm Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
        </div>
        <asp:Button ID="btnReset" runat="server" Text="Reset Password" CssClass="btn btn-primary" OnClick="btnReset_Click" />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>