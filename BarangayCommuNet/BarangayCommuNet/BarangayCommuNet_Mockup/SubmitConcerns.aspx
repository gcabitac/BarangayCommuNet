<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubmitConcerns.aspx.cs" Inherits="BarangayCommuNet_Mockup.SubmitConcerns" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.body.classList.add('page-submitconcerns');
            console.log('Body classes:', document.body.className);
        });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="concern-wrapper">
        <div id="page-header">
            <h2>Submit a Concern</h2>
            <p>Barangay Mamatid Unit will get back to you as soon as possible.</p>
        </div>
        <div class="concern-card">

            <p class="form-section-title">Concern Details</p>

            <div class="form-group">
                <label>Concern Category</label>
                <asp:DropDownList ID="ddlConcernType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="">-- Select a category --</asp:ListItem>
                    <asp:ListItem Value="Missed Collection">Missed Collection</asp:ListItem>
                    <asp:ListItem Value="Illegal Dumping">Illegal Dumping</asp:ListItem>
                    <asp:ListItem Value="Noise Complaint">Noise Complaint</asp:ListItem>
                    <asp:ListItem Value="Others">Others</asp:ListItem>
                </asp:DropDownList>
                <p class="category-hint">Choose the category that describes your concern.</p>
            </div>

            <div class="form-group">
                <label>Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="5"
                    placeholder="Describe your concern in detail." />
            </div>

            <hr class="form-divider" />

            <p class="form-section-title">Additional Information</p>

            <div class="form-group">
                <label>Phase</label>
                <asp:DropDownList ID="ddlConcernPhase" runat="server" CssClass="form-control">
                    <asp:ListItem Value="">-- Select Phase --</asp:ListItem>
                    <asp:ListItem Value="1">1</asp:ListItem>
                    <asp:ListItem Value="2">2</asp:ListItem>
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                    <asp:ListItem Value="6">6</asp:ListItem>
                    <asp:ListItem Value="7">7</asp:ListItem>
                </asp:DropDownList>
                <p class="category-hint">Select your specific phase where the concern needs to be addressed..</p>
            </div>

            <div class="form-group">
                <label>Location <span class="optional">(Complete Address)</span></label>
                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"
                    placeholder="Block, Lot, Street, Subdivision" />
            </div>

            <div class="form-group">
                <label>Upload Photo <span class="optional">(Optional)</span></label>
                <div class="upload-area">
                    <span class="upload-label">Choose your file to upload</span>
                    <asp:FileUpload ID="fuPhoto" runat="server" accept="image/*" />
                </div>
            </div>


            <div class="form-buttons">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit Concern"
                    CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear Form"
                    CssClass="btn btn-secondary" OnClick="btnClear_Click" />
            </div>

            <asp:Label ID="lblMessage" runat="server" />
        </div>
    </div>
</asp:Content>