<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="ManageGarbageSchedule.aspx.cs" Inherits="BarangayCommuNet_Mockup.ManageGarbageSchedule" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-manageschedule');
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="managesched-wrapper">

        <div id="page-header">
            <h2>Manage Garbage Collection Schedule</h2>
            <p>Add, edit, or delete collection schedules by phase.</p>
        </div>

        <div class="managesched-card">

            <%-- ── TOP: calendar beside form ── --%>
            <div class="ms-top">

                <%-- calendar --%>
                <div class="ms-cal-col">
                    <div class="ms-sec-label">Select Date</div>
                    <asp:Calendar ID="calSchedule" runat="server"
                        OnDayRender="calSchedule_DayRender"
                        OnSelectionChanged="calSchedule_SelectionChanged"
                        CssClass="ms-calendar"
                        TitleStyle-CssClass="ms-cal-title"
                        DayHeaderStyle-CssClass="ms-cal-dayheader"
                        DayStyle-CssClass="ms-cal-day"
                        SelectedDayStyle-CssClass="ms-cal-selected"
                        TodayDayStyle-CssClass="ms-cal-today"
                        OtherMonthDayStyle-CssClass="ms-cal-other"
                        NextPrevStyle-CssClass="ms-cal-nav"
                        ShowGridLines="false">
                    </asp:Calendar>
                    <asp:Label ID="lblSelectedDate" runat="server"
                        Text="No date selected."
                        CssClass="ms-selected-date-msg" />
                </div>

                <%-- form --%>
                <div class="ms-form-col">
                    <div class="ms-sec-label">Schedule Details</div>

                    <div class="ms-form-grid">
                        <div class="ms-form-group">
                            <label class="ms-form-lbl">Phase</label>
                            <asp:TextBox ID="txtPhase" runat="server"
                                CssClass="ms-form-inp"
                                placeholder="Phase 1" />
                        </div>
                        <div class="ms-form-group">
                            <label class="ms-form-lbl">Collection Type</label>
                            <asp:DropDownList ID="ddlCollectionType" runat="server" CssClass="ms-form-inp">
                                <asp:ListItem Text="Biodegradable"     Value="Biodegradable" />
                                <asp:ListItem Text="Non-Biodegradable" Value="Non-Biodegradable" />
                                <asp:ListItem Text="Special Collection" Value="Special Collection" />
                            </asp:DropDownList>
                        </div>
                        <div class="ms-form-group">
                            <label class="ms-form-lbl">Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="ms-form-inp">
                                <asp:ListItem Text="Active"      Value="Active" />
                                <asp:ListItem Text="Upcoming"    Value="Upcoming" />
                                <asp:ListItem Text="Rescheduled" Value="Rescheduled" />
                            </asp:DropDownList>
                        </div>
                        <div class="ms-form-group">
                            <label class="ms-form-lbl">Notes</label>
                            <asp:TextBox ID="txtNotes" runat="server"
                                CssClass="ms-form-inp"
                                placeholder="Morning Collection" />
                        </div>
                    </div>

                    <div class="ms-btn-row">
                        <asp:Button ID="btnAdd" runat="server"
                            Text="Add Collection Day"
                            CssClass="ms-btn-add"
                            OnClick="btnAdd_Click" />
                        <asp:Button ID="btnSave" runat="server"
                            Text="Save Changes"
                            CssClass="ms-btn-save"
                            OnClick="btnSave_Click" />
                    </div>

                    <asp:Label ID="lblMessage" runat="server" CssClass="ms-msg" />
                </div>

            </div>

            <%-- ── BOTTOM: full-width table ── --%>
            <div class="ms-bottom">
                <div class="ms-sec-label">Existing Schedules</div>

                <asp:GridView ID="gvSchedule" runat="server"
                    CssClass="ms-table"
                    AutoGenerateColumns="false"
                    DataKeyNames="ScheduleID"
                    GridLines="None"
                    OnRowEditing="gvSchedule_RowEditing"
                    OnRowUpdating="gvSchedule_RowUpdating"
                    OnRowDeleting="gvSchedule_RowDeleting"
                    OnRowCancelingEdit="gvSchedule_RowCancelingEdit"
                    EmptyDataText="No schedules found."
                    EmptyDataRowStyle-CssClass="ms-empty">
                    <HeaderStyle CssClass="ms-table-head" />
                    <Columns>
                        <asp:BoundField DataField="ScheduleID"    HeaderText="ID"    ReadOnly="true" />
                        <asp:BoundField DataField="CollectionDate" HeaderText="Date"
                            DataFormatString="{0:dd MMM yyyy}" HtmlEncode="false" />
                        <asp:BoundField DataField="Phase"          HeaderText="Phase" />
                        <asp:BoundField DataField="CollectionType" HeaderText="Type" />
                        <asp:BoundField DataField="Notes"          HeaderText="Notes" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# "ms-pill ms-pill-" + GetStatusClass(Eval("Status").ToString()) %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="ms-form-inp-sm">
                                    <asp:ListItem Text="Active"      Value="Active" />
                                    <asp:ListItem Text="Upcoming"    Value="Upcoming" />
                                    <asp:ListItem Text="Rescheduled" Value="Rescheduled" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField
                            ShowEditButton="true"
                            ShowDeleteButton="true"
                            EditText="Edit"
                            DeleteText="Delete"
                            UpdateText="Update"
                            CancelText="Cancel"
                            ButtonType="Link"
                            ControlStyle-CssClass="ms-tbl-act" />
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>

</asp:Content>