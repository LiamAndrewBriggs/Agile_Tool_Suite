<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="Agile_Tool_Suite.Projects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Project
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Theme.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header class="page_title">Projects</header>
    <br /> <br /> <br />
    <div>
        <header class="title">New Project</header>
        <p>Name of Project: </p>
        <asp:TextBox ID="ProjectName" placeholder="Project Name" class="info" runat="server" />
        <br /> <br />
        <p>Project Methodology</p>
        <asp:DropDownList ID="DdlMonths" class="info" runat="server">
            <asp:ListItem Enabled="true" Text="Select Methodology" Value="-1"></asp:ListItem>
            <asp:ListItem Text="Scrum" Value="1"></asp:ListItem>
            <asp:ListItem Text="Kanban" Value="2"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="DropDownError" Text="Please select a methodology" visible="false" class="error" runat="server" />
        <br />
        <asp:Button ID="createButton" Text="Create" runat="server" OnClick="CreateProject" class="button" />
        <br /> <br /> <br />
    </div>
    <div>
        <header class="title">Join A Project</header>
        <p> Join another project using the code your Manager/Scrum master will give you:</p>
        <br /> <br />
        <asp:TextBox ID="searchTextBox" placeholder="Project Code" runat="server" class="nu_entry" />
        <asp:Button ID="searchProject" Text="Search" runat="server" OnClick="Search_Project" class="button"/>
        <asp:Label ID="errorMessage" Text="" runat="server" class="errors"/>
    </div>
</asp:Content>
