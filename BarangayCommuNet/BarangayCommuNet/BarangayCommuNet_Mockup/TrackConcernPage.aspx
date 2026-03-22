<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackConcernPage.aspx.cs" Inherits="BarangayCommuNet_Mockup.TrackConcernPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-trackconcern');
            console.log('Body classes:', document.body.className);
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="trackconcern-wrapper">

        <div id="page-header">
            <h2>Track Your Concerns</h2>
            <p>View the status and responses from the barangay regarding your submitted concerns.</p>
        </div>

        <div class="concern-card">

            <div class="tc-info">

                <div class="tc-info-box">
                    <span class="tc-info-title">Submitted Concerns</span>
                </div>

                <div class="tc-info-box">
                    <span class="tc-info-title">Status Tags</span>
                    <span class="tc-badge tc-pending">Pending</span>
                    <span class="tc-badge tc-progress">In Progress</span>
                    <span class="tc-badge tc-resolved">Resolved</span>
                </div>

            </div>

            <div class="tc-table-wrap">
                <asp:GridView ID="gvConcerns"
                    runat="server"
                    CssClass="tc-table"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    EmptyDataText="No concerns submitted yet.">

                    <Columns>
                        <asp:BoundField DataField="ConcernID" HeaderText="ID" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# "tc-badge tc-" + Eval("Status").ToString().Replace(" ", "").ToLower() %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DateSubmitted"
                            HeaderText="Date Submitted"
                            DataFormatString="{0:MMMM dd, yyyy}" />
                        <asp:BoundField DataField="AdminResponse" HeaderText="Admin Response" />
                    </Columns>

                </asp:GridView>
            </div>

        </div>
    </div>

</asp:Content>