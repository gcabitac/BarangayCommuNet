<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="AdminDashboard.aspx.cs" Inherits="BarangayCommuNet_Mockup.AdminDashboard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-admindashboard');
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="admindash-wrapper">

        <div id="page-header">
            <h2>Admin Dashboard</h2>
            <p>Operations overview for LGU Staff and Barangay Officials.</p>
        </div>

        <div class="admindash-card">

            <%-- ── HERO BANNER ── --%>
            <div class="j-cover">
                <div class="j-stat-hero">
                    <div class="j-hero-num">
                        <asp:Label ID="lblTotalConcerns" runat="server" Text="0" />
                    </div>
                    <div class="j-hero-lbl">Concerns Today</div>
                    <span class="j-hero-badge j-badge-warn">
                        <asp:Label ID="lblPendingBadge" runat="server" Text="0 pending" />
                    </span>
                </div>
                <div class="j-stat-hero">
                    <div class="j-hero-num">
                        <asp:Label ID="lblResolvedToday" runat="server" Text="0" />
                    </div>
                    <div class="j-hero-lbl">Resolved Today</div>
                    <span class="j-hero-badge j-badge-green">this month</span>
                </div>
                <div class="j-stat-hero">
                    <div class="j-hero-num">
                        <asp:Label ID="lblMissedCollections" runat="server" Text="0" />
                    </div>
                    <div class="j-hero-lbl">Missed Collections</div>
                    <span class="j-hero-badge j-badge-warn">needs attention</span>
                </div>
                <div class="j-stat-hero">
                    <div class="j-hero-num">
                        <asp:Label ID="lblAnnouncements" runat="server" Text="0" />
                    </div>
                    <div class="j-hero-lbl">Announcements</div>
                    <span class="j-hero-badge j-badge-blue">this month</span>
                </div>
            </div>

            <%-- ── THREE COLUMN BODY ── --%>
            <div class="j-body">

                <%-- col 1: phase schedule --%>
                <div class="j-col">
                    <div class="j-sec-label">Phase Schedule</div>

                    <asp:Repeater ID="rptWeeklySchedule" runat="server">
                        <HeaderTemplate>
                            <div class="j-sch-list">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class='<%# "j-sch-card j-sch-" + GetStatusClass(Eval("Status").ToString()) %>'>
                                <div class="j-sch-top">
                                    <span class="j-sch-zone">Phase <%# Eval("Phase") %></span>
                                    <span class='<%# "j-sch-pill j-sch-pill-" + GetStatusClass(Eval("Status").ToString()) %>'>
                                        <%# Eval("Status") %>
                                    </span>
                                </div>
                                <div class="j-sch-type"><%# Eval("CollectionType") %></div>
                                <div class="j-sch-date">
                                    <%# Convert.ToDateTime(Eval("CollectionDate")).ToString("MMM d") %>
                                    <%# Eval("Notes") != DBNull.Value && Eval("Notes").ToString() != "" ? " &middot; " + Eval("Notes").ToString() : "" %>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>

                    <asp:Label ID="lblNoSchedule" runat="server" Visible="false"
                        Text="No schedules found for this week."
                        CssClass="j-empty-msg" />
                </div>

                <%-- col 2: recent concerns --%>
                <div class="j-col">
                    <div class="j-sec-label">Recent Concerns</div>

                    <asp:Repeater ID="rptRecentConcerns" runat="server">
                        <HeaderTemplate>
                            <div class="j-concern-list">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class='<%# "j-concern-card j-concern-" + GetConcernStatusClass(Eval("CurrentStatus").ToString()) %>'>
                                <div class="j-cf-avatar">
                                    <%# GetInitials(Eval("FullName").ToString()) %>
                                </div>
                                <div class="j-cf-body">
                                    <div class="j-cf-name"><%# Eval("FullName") %></div>
                                    <div class="j-cf-cat">
                                        <%# Eval("Category") %> &middot; Phase <%# Eval("Phase") %>
                                    </div>
                                </div>
                                <span class='<%# "j-cf-pill j-cf-pill-" + GetConcernStatusClass(Eval("CurrentStatus").ToString()) %>'>
                                    <%# Eval("CurrentStatus") %>
                                </span>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>

                    <asp:Label ID="lblNoConcerns" runat="server" Visible="false"
                        Text="No recent concerns."
                        CssClass="j-empty-msg" />
                </div>

                <%-- col 3: quick actions + concern status --%>
                <div class="j-col j-col-last">

                    <div class="j-sec-label">Quick Actions</div>
                    <div class="j-action-list">
                        <a class="j-action-btn" href="ManageGarbageSchedule.aspx">
                            <div class="j-action-title">Manage Schedule</div>
                            <div class="j-action-sub">Add, edit, delete collections</div>
                        </a>
                        <a class="j-action-btn" href="AdminConcerns.aspx">
                            <div class="j-action-title">View Concerns</div>
                            <div class="j-action-sub">
                                <asp:Label ID="lblConcernSub" runat="server" Text="Pending and in progress" />
                            </div>
                        </a>
                        <a class="j-action-btn" href="AnnouncementManagementPage.aspx">
                            <div class="j-action-title">Post Announcement</div>
                            <div class="j-action-sub">Publish to all residents</div>
                        </a>
                        <a class="j-action-btn" href="LandingPage.aspx">
                            <div class="j-action-title">Public Site</div>
                            <div class="j-action-sub">View resident-facing pages</div>
                        </a>
                    </div>

                    <div class="j-sec-label" style="margin-top: 18px;">Concern Status</div>
                    <div class="j-status-list">
                        <div class="j-status-row">
                            <span class="j-sp j-sp-pending">Pending</span>
                            <asp:Label ID="lblPending" runat="server" Text="0" CssClass="j-status-num" />
                        </div>
                        <div class="j-status-row">
                            <span class="j-sp j-sp-progress">In Progress</span>
                            <asp:Label ID="lblInProgress" runat="server" Text="0" CssClass="j-status-num" />
                        </div>
                        <div class="j-status-row">
                            <span class="j-sp j-sp-resolved">Resolved</span>
                            <asp:Label ID="lblResolved" runat="server" Text="0" CssClass="j-status-num" />
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>

</asp:Content>