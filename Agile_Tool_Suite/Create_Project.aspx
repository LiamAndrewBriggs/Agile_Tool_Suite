<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create_Project.aspx.cs" Inherits="Agile_Tool_Suite.Create_Project" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create project</title>
    <link href="Css/Create_Project.css" rel="stylesheet" />
    <link href="Css/Theme.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">

    <div class="username">
       <asp:Button ID="LogoutButton" Text="Log out" runat="server" OnClick="LogoutMethod" class="button"/> 
    </div>

        <div class="container">
            <header class="title">New Project</header>

            <p>Name of Project</p>
            <asp:TextBox ID="ProjectName" placeholder="Project Name" class="info" runat="server" />
            <p>Project Methodology</p>
                <asp:DropDownList ID="DdlMonths" class="info" runat="server">
                <asp:ListItem Enabled="true" Text="Select Methodology" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Scrum" Value="1"></asp:ListItem>
                <asp:ListItem Text="Kanban" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="DropDownError" Text="Please select a methodology" visible="false" class="error" runat="server" />
            <br />
            <asp:Button ID="createButton" Text="Create" runat="server" OnClick="CreateProject" class="button" />
        </div>
    </form>
</body>
</html>
