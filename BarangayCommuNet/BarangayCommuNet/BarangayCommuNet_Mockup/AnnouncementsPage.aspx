<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="AnnouncementsPage.aspx.cs" Inherits="BarangayCommuNet_Mockup.AnnouncementsPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-announcements');
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="announcements-wrapper">

        <div id="page-header">
            <h2>Announcements</h2>
            <p>Browse the latest news, advisories, and updates from the Barangay LGU.</p>
        </div>

        <div class="announcements-card">

            <!-- FILTER BAR  -->
            <div class="filter-bar">
                <input type="text" id="searchInput" placeholder="Search announcements..." oninput="filterRows()" />
                <select id="catFilter" onchange="filterRows()">
                    <option value="all">All Categories</option>
                    <option value="garbage collection">Garbage Collection</option>
                    <option value="health and safety">Health and Safety</option>
                    <option value="events">Events</option>
                    <option value="advisory">Advisory</option>
                    <option value="general">General</option>
                </select>
            </div>

            <!-- ANNOUNCEMENT LIST -->
            <div class="ann-list" id="annList">

                <asp:Repeater ID="rptAnnouncements" runat="server">
                    <ItemTemplate>
                        <div class="ann-row"
                             data-category='<%# Eval("Category").ToString().ToLower() %>'
                             data-title='<%# Eval("Title").ToString().ToLower() %>'>

                            <!-- DATE -->
                            <div class="ann-date-col">
                                <div class="ann-date-day">
                                    <%# Convert.ToDateTime(Eval("PublishedAt")).ToString("dd") %>
                                </div>
                                <div class="ann-date-mon">
                                    <%# Convert.ToDateTime(Eval("PublishedAt")).ToString("MMM") %>
                                </div>
                            </div>

                            <!-- CONTENT -->
                            <div class="ann-content-col">
                                <span class='<%# "ann-badge ann-badge-" + GetBadgeClass(Eval("Category").ToString()) %>'>
                                    <%# Eval("Category") %>
                                </span>

                                <div class="ann-row-title"><%# Eval("Title") %></div>

                                <div class="ann-row-excerpt">
                                    <%# TruncateText(Eval("Content").ToString(), 160) %>
                                </div>

                                <div class="ann-row-meta">
                                    <span><%# Convert.ToDateTime(Eval("PublishedAt")).ToString("MMMM dd, yyyy") %></span>

                                    <a class="ann-read-link"
                                       href='<%# "AnnouncementDetails.aspx?id=" + Eval("AnnouncementID") %>'>
                                        Read full post &rsaquo;
                                    </a>
                                </div>
                            </div>

                            <!-- IMAGE -->
                            <div class='<%# "ann-img-col ann-img-" + GetBadgeClass(Eval("Category").ToString()) %>'>
                                <asp:Literal
                                    ID="litImage"
                                    runat="server"
                                    Text='<%# BuildImageOrPlaceholder(Eval("ImagePath"), Eval("Category").ToString()) %>'>
                                </asp:Literal>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <!-- EMPTY STATE -->
                <div class="ann-empty" id="annEmpty" style="display:none;">
                    <p>No announcements match your search.</p>
                    <p>Try a different keyword or select a different category.</p>
                </div>

            </div>
        </div>
    </div>

    <!-- FILTER SCRIPT -->
    <script type="text/javascript">
        function filterRows() {
            var keyword = document.getElementById('searchInput').value.toLowerCase().trim();
            var category = document.getElementById('catFilter').value.toLowerCase();
            var rows = document.querySelectorAll('.ann-row');
            var visible = 0;

            rows.forEach(function (row) {
                var rowCat = row.getAttribute('data-category');
                var rowTitle = row.getAttribute('data-title');

                var matchCat = (category === 'all' || rowCat === category);
                var matchSearch = (keyword === '' || rowTitle.indexOf(keyword) !== -1);

                if (matchCat && matchSearch) {
                    row.style.display = 'grid';
                    visible++;
                } else {
                    row.style.display = 'none';
                }
            });

            document.getElementById('annEmpty').style.display = (visible === 0) ? 'block' : 'none';
        }
    </script>

</asp:Content>