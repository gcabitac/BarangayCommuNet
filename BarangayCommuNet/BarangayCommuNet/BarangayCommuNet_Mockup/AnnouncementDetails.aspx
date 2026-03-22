<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="AnnouncementDetails.aspx.cs" Inherits="BarangayCommuNet_Mockup.AnnouncementDetails" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-announcementdetails');
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="announcement-wrapper">

        <div id="page-header">
            <h2>Announcement Details</h2>
            <p>Full view of the selected barangay announcement.</p>
        </div>

        <asp:Panel ID="pnlAnnouncement" runat="server">
            <div class="announcement-card">
                <span class="cat-tag">
                    <asp:Literal ID="litCategory" runat="server" />
                </span>
                <h2 class="article-title">
                    <asp:Literal ID="litTitle" runat="server" />
                </h2>
                <div class="article-meta">
                    Posted: <asp:Literal ID="litDate" runat="server" /> &nbsp;|&nbsp; Barangay LGU
                </div>
                <asp:Image ID="imgAnnouncement" runat="server" CssClass="article-image" Visible="false" />
                <div class="article-body">
                    <asp:Literal ID="litContent" runat="server" />
                </div>
                <div class="form-buttons" style="margin-top: 28px;">
                    <a href="AnnouncementsPage.aspx" class="btn btn-primary">
                        &larr; Back to Announcements
                    </a>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
            <div class="announcement-card">
                <p style="font-size:14px; color:#999; margin-bottom:16px;">
                    This announcement could not be found or is no longer available.
                </p>
                <a href="AnnouncementsPage.aspx" class="btn btn-primary">Back to Announcements</a>
            </div>
        </asp:Panel>

        <div class="announcement-card">
            <h3 class="related-title">Other Announcements</h3>
            <asp:Repeater ID="rptRelated" runat="server">
                <HeaderTemplate><ul class="related-list"></HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <a href='<%# "AnnouncementDetails.aspx?id=" + Eval("AnnouncementID") %>'>
                            <%# Eval("Title") %>
                        </a>
                        <span>
                            <%# Convert.ToDateTime(Eval("PublishedAt")).ToString("MMMM dd, yyyy") %>
                        </span>
                    </li>
                </ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
        </div>

    </div>

</asp:Content>