<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AnnouncementManagementPage.aspx.cs" Inherits="BarangayCommuNet_Mockup.AnnouncementManagementPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-manageannouncements');
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="manageannounce-wrapper">

        <div id="page-header">
            <h2>Announcement Management</h2>
            <p>Create, update, and delete barangay announcements.</p>
        </div>

        <div class="manageannounce-card">

            <div class="ma-top">
                <div class="ma-form-col">
                    <div class="ma-sec-label">Create / Edit Announcement</div>

                    <asp:HiddenField ID="hfAnnouncementID" runat="server" />

                    <div class="ma-form-grid">
                        <div class="ma-form-group ma-span-2">
                            <label class="ma-form-lbl">Title</label>
                            <asp:TextBox ID="txtTitle" runat="server"
                                CssClass="ma-form-inp"
                                placeholder="Enter announcement title" />
                        </div>
                        <div class="ma-form-group">
                            <label class="ma-form-lbl">Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="ma-form-inp">
                                <asp:ListItem Value="General">General</asp:ListItem>
                                <asp:ListItem Value="Health and Safety">Health and Safety</asp:ListItem>
                                <asp:ListItem Value="Garbage Collection">Garbage Collection</asp:ListItem>
                                <asp:ListItem Value="Events">Events</asp:ListItem>
                                <asp:ListItem Value="Advisory">Advisory</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="ma-form-group">
                            <label class="ma-form-lbl">Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="ma-form-inp">
                                <asp:ListItem Value="Published">Published</asp:ListItem>
                                <asp:ListItem Value="Draft">Draft</asp:ListItem>
                                <asp:ListItem Value="Archived">Archived</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="ma-form-group ma-span-2">
                            <label class="ma-form-lbl">Content</label>
                            <asp:TextBox ID="txtContent" runat="server"
                                CssClass="ma-form-inp ma-form-textarea"
                                TextMode="MultiLine" Rows="5"
                                placeholder="Write the announcement content here." />
                        </div>
                    </div>

                    <div class="ma-btn-row">
                        <asp:Button ID="btnPost" runat="server"
                            Text="Publish Announcement"
                            CssClass="ma-btn-add"
                            OnClick="btnPost_Click" />
                        <asp:Button ID="btnClear" runat="server"
                            Text="Clear"
                            CssClass="ma-btn-save"
                            OnClick="btnClear_Click" />
                    </div>

                    <asp:Label ID="lblMessage" runat="server" CssClass="ma-msg" />
                </div>
            </div>

            <div class="ma-bottom">
                <div class="ma-sec-label">Existing Announcements</div>

                <asp:GridView ID="gvAnnouncements" runat="server"
                    CssClass="ma-table"
                    AutoGenerateColumns="false"
                    DataKeyNames="AnnouncementID"
                    GridLines="None"
                    OnRowDeleting="gvAnnouncements_RowDeleting"
                    OnRowCommand="gvAnnouncements_RowCommand"
                    EmptyDataText="No announcements found."
                    EmptyDataRowStyle-CssClass="ma-empty">
                    <HeaderStyle CssClass="ma-table-head" />
                    <Columns>
                        <asp:BoundField DataField="AnnouncementID" HeaderText="ID" ReadOnly="true" />
                        <asp:BoundField DataField="Title"          HeaderText="Title" />
                        <asp:BoundField DataField="Category"       HeaderText="Category" />
                        <asp:BoundField DataField="PublishedAt"    HeaderText="Posted On"
                            DataFormatString="{0:dd MMM yyyy}" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# "ma-pill ma-pill-" + GetStatusClass(Eval("Status").ToString()) %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandName="EditAnnouncement"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    CssClass="ma-tbl-act ma-tbl-edit">Edit</asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="Delete"
                                    CssClass="ma-tbl-act ma-tbl-delete"
                                    OnClientClick="return confirm('Delete this announcement?');">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>

</asp:Content>