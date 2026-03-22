<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Collection.aspx.cs" Inherits="BarangayCommuNet_Mockup.Collection" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-collection');
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="collection-wrapper">

        <div id="page-header">
            <h2>Garbage Collection Schedule</h2>
            <p>View upcoming and active collection schedules for your phase.</p>
        </div>

        <div class="collection-card">

            <%-- ── TOP: filters ── --%>
            <div class="col-filter-row">
                <div class="col-sec-label">Filter Schedules</div>
                <div class="col-filter-inputs">
                    <div class="col-form-group">
                        <label class="ms-form-lbl">Phase</label>
                        <asp:DropDownList ID="ddlPhaseFilter" runat="server"
                            CssClass="ms-form-inp"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ApplyFilters">
                            <asp:ListItem Text="All Phases" Value="" />
                            <asp:ListItem Text="Phase 1"    Value="1" />
                            <asp:ListItem Text="Phase 2"    Value="2" />
                            <asp:ListItem Text="Phase 3"    Value="3" />
                            <asp:ListItem Text="Phase 5"    Value="5" />
                            <asp:ListItem Text="Phase 6"    Value="6" />
                            <asp:ListItem Text="Phase 7"    Value="7" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-form-group">
                        <label class="ms-form-lbl">Month</label>
                        <asp:DropDownList ID="ddlMonthFilter" runat="server"
                            CssClass="ms-form-inp"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ApplyFilters">
                            <asp:ListItem Text="All Months"  Value="" />
                            <asp:ListItem Text="January"     Value="1" />
                            <asp:ListItem Text="February"    Value="2" />
                            <asp:ListItem Text="March"       Value="3" />
                            <asp:ListItem Text="April"       Value="4" />
                            <asp:ListItem Text="May"         Value="5" />
                            <asp:ListItem Text="June"        Value="6" />
                            <asp:ListItem Text="July"        Value="7" />
                            <asp:ListItem Text="August"      Value="8" />
                            <asp:ListItem Text="September"   Value="9" />
                            <asp:ListItem Text="October"     Value="10" />
                            <asp:ListItem Text="November"    Value="11" />
                            <asp:ListItem Text="December"    Value="12" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-form-group">
                        <label class="ms-form-lbl">Collection Type</label>
                        <asp:DropDownList ID="ddlTypeFilter" runat="server"
                            CssClass="ms-form-inp"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ApplyFilters">
                            <asp:ListItem Text="All Types"          Value="" />
                            <asp:ListItem Text="Biodegradable"      Value="Biodegradable" />
                            <asp:ListItem Text="Non-Biodegradable"  Value="Non-Biodegradable" />
                            <asp:ListItem Text="Special Collection" Value="Special Collection" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-form-group">
                        <label class="ms-form-lbl">Status</label>
                        <asp:DropDownList ID="ddlStatusFilter" runat="server"
                            CssClass="ms-form-inp"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ApplyFilters">
                            <asp:ListItem Text="All Statuses" Value="" />
                            <asp:ListItem Text="Active"       Value="Active" />
                            <asp:ListItem Text="Upcoming"     Value="Upcoming" />
                            <asp:ListItem Text="Rescheduled"  Value="Rescheduled" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-form-group">
                        <label class="ms-form-lbl">&nbsp;</label>
                        <asp:Button ID="btnClearFilters" runat="server"
                            Text="Clear Filters"
                            CssClass="ms-btn-save"
                            OnClick="btnClearFilters_Click" />
                    </div>
                </div>
            </div>

            <%-- ── BOTTOM: calendar | schedules ── --%>
            <div class="col-bottom">

                <div class="col-cal-col">
                    <div class="col-sec-label">Calendar</div>
                    <asp:Calendar ID="calSchedule" runat="server"
                        OnDayRender="calSchedule_DayRender"
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
                </div>

                <div class="col-table-col">
                    <div class="col-sec-label">Schedules</div>
                    <asp:GridView ID="gvSchedule" runat="server"
                        CssClass="ms-table"
                        AutoGenerateColumns="false"
                        DataKeyNames="ScheduleID"
                        GridLines="None"
                        EmptyDataText="No schedules found."
                        EmptyDataRowStyle-CssClass="ms-empty">
                        <HeaderStyle CssClass="ms-table-head" />
                        <Columns>
                            <asp:BoundField DataField="CollectionDate" HeaderText="Date"
                                DataFormatString="{0:MMMM dd, yyyy}" HtmlEncode="false" />
                            <asp:BoundField DataField="Phase"          HeaderText="Phase" />
                            <asp:BoundField DataField="CollectionType" HeaderText="Type" />
                            <asp:BoundField DataField="Notes"          HeaderText="Notes" />
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='<%# "ms-pill ms-pill-" + GetStatusClass(Eval("Status").ToString()) %>'>
                                        <%# Eval("Status") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>

        </div>
    </div>

</asp:Content>