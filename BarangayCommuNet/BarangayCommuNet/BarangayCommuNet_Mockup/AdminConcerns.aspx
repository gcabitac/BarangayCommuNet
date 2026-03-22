<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminConcerns.aspx.cs" Inherits="BarangayCommuNet_Mockup.AdminConcerns" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-adminconcerns');
        });

        function filterConcerns() {
            var searchInput = document.getElementById("searchInput").value.toLowerCase();
            var catFilter = document.getElementById("catFilter").value.toLowerCase();
            var rows = document.querySelectorAll("#<%= gvConcerns.ClientID %> tr:not(:first-child)");

            rows.forEach(function (row) {
                var resident = row.cells[0].innerText.toLowerCase();
                var category = row.cells[1].innerText.toLowerCase();

                var matchesSearch = resident.includes(searchInput);
                var matchesCategory = (catFilter === "all" || category.includes(catFilter));

                row.style.display = (matchesSearch && matchesCategory) ? "" : "none";
            });
        }

        function clearFilters() {
            document.getElementById("searchInput").value = "";
            document.getElementById("catFilter").value = "all";
            filterConcerns();
        }
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="adminconcerns-wrapper">

        <div id="page-header">
            <h2>Admin Concern Management</h2>
            <p>View, filter, and manage resident concerns submitted to the barangay.</p>
        </div>

        <div class="adminconcerns-card">

            <%-- ── FILTER BAR ── --%>
            <div class="ac-filter-row">
                <div class="ac-sec-label">Filter Concerns</div>
                <div class="ac-filter-inputs">
                    <input type="text" id="searchInput"
                        placeholder="Search by resident name..."
                        oninput="filterConcerns()"
                        class="ms-form-inp ac-search" />
                    <select id="catFilter" onchange="filterConcerns()" class="ms-form-inp ac-select">
                        <option value="all">All Categories</option>
                        <option value="missed collection">Missed Collection</option>
                        <option value="illegal dumping">Illegal Dumping</option>
                        <option value="noise complaint">Noise Complaint</option>
                        <option value="others">Others</option>
                    </select>
                    <button class="ms-btn-save" onclick="clearFilters(); return false;">Clear Filters</button>
                </div>
            </div>

            <%-- ── TABLE ── --%>
            <div class="ac-table-section">
                <div class="ac-sec-label">Resident Concerns</div>

                <asp:GridView ID="gvConcerns" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="ms-table"
                    DataKeyNames="ConcernID"
                    OnRowCommand="gvConcerns_RowCommand"
                    OnRowDataBound="gvConcerns_RowDataBound"
                    EmptyDataText="No resident concerns found."
                    EmptyDataRowStyle-CssClass="ms-empty"
                    GridLines="None">
                    <HeaderStyle CssClass="ms-table-head" />
                    <Columns>
                        <asp:BoundField DataField="FullName"       HeaderText="Resident Name" />
                        <asp:BoundField DataField="Category"       HeaderText="Category" />
                        <asp:BoundField DataField="Description"    HeaderText="Details"
                            ItemStyle-Width="280px" ItemStyle-Wrap="true" />

                        <asp:TemplateField HeaderText="Photo">
                            <ItemTemplate>
                                <asp:Image ID="imgConcern" runat="server"
                                    ImageUrl='<%# GetPhotoUrl(Eval("PhotoPath")) %>'
                                    Visible='<%# Eval("PhotoPath") != DBNull.Value && Eval("PhotoPath").ToString() != "" %>'
                                    style="max-width:80px; max-height:80px; border-radius:6px; cursor:pointer; object-fit:cover;"
                                    onclick='<%# "window.open(\"" + ResolveUrl("~/" + Eval("PhotoPath").ToString()) + "\", \"_blank\")" %>' />
                                <asp:Label runat="server"
                                    Text="No photo"
                                    Visible='<%# Eval("PhotoPath") == DBNull.Value || Eval("PhotoPath").ToString() == "" %>'
                                    style="color:#aaa; font-size:12px;" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="DateSubmitted"  HeaderText="Date Submitted"
                            DataFormatString="{0:MMM dd, yyyy}" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="ms-form-inp-sm" style="font-weight:bold;">
                                    <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                    <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                                    <asp:ListItem Value="Resolved">Resolved</asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hfCurrentStatus" runat="server" Value='<%# Eval("CurrentStatus") %>' />

                                <%-- Stores the resident's UserID so we can target the notification --%>
                                <asp:HiddenField ID="hfResidentUserID" runat="server" Value='<%# Eval("UserID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Admin Response">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAdminResponse" runat="server"
                                    CssClass="ms-form-inp"
                                    Text='<%# Eval("LatestResponse") %>'
                                    placeholder="Enter response..." />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnSave" runat="server"
                                    Text="Save"
                                    CommandName="SaveResponse"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    CssClass="ms-btn-add" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblAdminMsg" runat="server" CssClass="ms-msg" style="margin-top:16px;" />
            </div>

        </div>
    </div>

</asp:Content>