<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResidentDashboard.aspx.cs" Inherits="BarangayCommuNet_Mockup.ResidentDashboard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-residentdashboard');
        });
    </script>

    <style>
        /* ── Unread notification: bold title + accent left border ── */
        .rd-notif-unread {
            border-left: 3px solid #2563eb;
            background-color: #eff6ff;
        }

        /* ── Unread dot badge ── */
        .rd-notif-unread-dot {
            display: inline-block;
            width: 8px;
            height: 8px;
            border-radius: 50%;
            background-color: #2563eb;
            margin-left: 6px;
            vertical-align: middle;
        }

        /* ── Read notification: muted appearance ── */
        .rd-notif-read {
            border-left: 3px solid transparent;
            opacity: 0.75;
        }

        /* ── Unread title is bolder ── */
        .rd-notif-unread .rd-notif-title {
            font-weight: 700;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="resdash-wrapper">

        <div id="page-header">
            <h2>Resident Dashboard</h2>
            <p>Welcome to Barangay CommuNet — your hub for updates and services.</p>
        </div>

        <div class="resdash-card">

            <%-- ── WELCOME BANNER ── --%>
            <div class="rd-cover">
                <div class="rd-welcome-hero">
                    <div class="rd-welcome-greeting">Good day, <asp:Label ID="lblUserName" runat="server" Text="Guest" />!</div>
                    <div class="rd-welcome-sub">Track garbage collection schedules and stay updated with announcements.</div>
                </div>
            </div>

            <%-- ── BODY: 2-col layout ── --%>
            <div class="rd-body">

                <%-- LEFT: main content ── --%>
                <div class="rd-main">

                    <%-- Today's collection status --%>
                    <div class="rd-sec-label">Today's Garbage Collection Status</div>
                    <div class="rd-stat-row">
                        <asp:Repeater ID="rptCollectionStatus" runat="server">
                            <ItemTemplate>
                                <%# GetStatusClass(Eval("Status").ToString()) %>
                                    <div class="rd-stat-num"><%# Eval("Status") %></div>
                                    <div class="rd-stat-lbl"><%# Eval("Phase") %> — <%# Eval("CollectionType") %></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <%-- Quick actions --%>
                    <div class="rd-sec-label" style="margin-top: 22px;">Quick Actions</div>
                    <div class="rd-action-list">
                        <a class="j-action-btn" href="Collection.aspx">
                            <div class="j-action-title">View Full Schedule</div>
                            <div class="j-action-sub">See all upcoming collection dates</div>
                        </a>
                        <a class="j-action-btn" href="SubmitConcerns.aspx">
                            <div class="j-action-title">Submit a Concern</div>
                            <div class="j-action-sub">Report missed pickups or issues</div>
                        </a>
                        <a class="j-action-btn" href="TrackConcernPage.aspx">
                            <div class="j-action-title">Track My Concern</div>
                            <div class="j-action-sub">Check the status of your report</div>
                        </a>
                    </div>

                </div>

                <%-- RIGHT: notifications sidebar ── --%>
                <div class="rd-sidebar">
                    <div class="rd-sec-label">Notifications</div>
                    <div class="rd-notif-list">
                        <asp:Repeater ID="rptNotifications" runat="server">
                            <ItemTemplate>
                                <%--
                                    onclick: fire-and-forget fetch to mark this notification
                                    as read before navigating to the target page.
                                --%>
                                <a class='<%# GetNotifItemClass(Eval("IsRead")) %>'
                                   href='<%# GetCategoryUrl(Eval("Category").ToString()) %>'
                                   onclick="markNotifRead(<%# Eval("NotificationID") %>, this); return true;">

                                    <div style="margin-bottom: 5px; display: flex; align-items: center;">
                                        <span class="rd-notif-badge"><%# Eval("Category") %></span>
                                        <%# Convert.ToBoolean(Eval("IsRead")) ? "" : "<span class='rd-notif-unread-dot'></span>" %>
                                    </div>

                                    <div class="rd-notif-title"><%# Eval("Title") %></div>
                                    <div class="rd-notif-desc"><%# Eval("Description") %></div>
                                    <div class="rd-notif-time">
                                        <%# GetRelativeTime(Convert.ToDateTime(Eval("CreatedAt"))) %>
                                    </div>
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <script>
        /**
         * Sends a fire-and-forget request to mark the notification as read.
         * The browser will still navigate normally via the href.
         * @param {number} notifId  - NotificationID from the database
         * @param {HTMLElement} el  - The anchor element that was clicked
         */
        function markNotifRead(notifId, el) {
            // Optimistically update the UI immediately
            el.classList.remove('rd-notif-unread');
            el.classList.add('rd-notif-read');
            var dot = el.querySelector('.rd-notif-unread-dot');
            if (dot) dot.remove();

            // Persist to the database via the HTTP handler
            fetch('MarkNotifRead.ashx?notifId=' + notifId, { method: 'POST' })
                .catch(function () { /* silently ignore network errors */ });
        }
    </script>

</asp:Content>