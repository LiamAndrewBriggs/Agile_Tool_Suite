<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AccountPage.aspx.cs" Inherits="Agile_Tool_Suite.AccountPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Account.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="registration_form">
            <header class="page_title">Your Information</header>
            <br /> <br /> <br />
            <p>Full Name</p>
            <br /> <br />
            <asp:TextBox ID="firstnameTextBox" placeholder="First Name" class="info" runat="server" Enabled="false" />
            <asp:TextBox ID="surnameTextBox" placeholder="Last Name" class="info" runat="server" Enabled="false" />
            <asp:Button ID="nameSwitch" Text="Edit" runat="server" OnClick="nameEdit" class="set_button" />
            <br /> <br />
            <p>Contact Information</p>
            <br /> <br />
            <asp:TextBox ID="emailTextBox" placeholder="Email" class="info" runat="server" Enabled="false" />
            <asp:Button ID="emailSwitch" Text="Edit" runat="server" OnClick="emailEdit" class="set_button" />
            <asp:Label ID="emailError" Text="Email already registered to an account" visible="false" class="error" runat="server" />
            <br /> <br />
            <p>Password</p>
            <br /> <br />
            <asp:TextBox ID="originalPasswordTextBox" placeholder="Original Password" class="info" type="password" runat ="server" Enabled="false" />
            <asp:Button ID="passwordSwitch" Text="Edit" runat="server" OnClick="passwordEdit" class="set_button" />
            <asp:Label ID="originalpasswordError" Text="Incorrect password" visible="false" class="error" runat="server" />
            <br /> <br />
            <asp:TextBox ID="passwordTextBox" placeholder="Password" class="info" type="password" runat ="server" Enabled="false" />
            <asp:TextBox ID="confirmPasswordTextBox" placeholder="Confirm Password" class="info" type="password" runat ="server" Enabled="false" />
            <asp:Label ID="passwordError" Text="Passwords do not match" visible="false" class="error" runat="server" />
            <br /> <br />
            <asp:Button ID="registerButton" Text="Update" runat="server" OnClick="registerEventMethod" class="button" />
         </div>
</asp:Content>
