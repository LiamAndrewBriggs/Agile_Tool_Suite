<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewUser.aspx.cs" Inherits="Agile_Tool_Suite.NewUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New User</title>
    <link href="Css/New_User.css" rel="stylesheet" />
    <link href="Css/Theme.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">

        <div class="username">
            <asp:Button ID="LogoutButton" Text="Log out" runat="server" OnClick="LogoutMethod" class="button"/> 
        </div>

        <div class="container">
            <div class="center">
                <asp:Label ID="userLabel" Text="No User" runat="server" class="title"/>
            </div>
            <p> Looks like you don't belong to a project</p>
            <p> Join a project using the code your Manager/Scrum master will give you</p>
            <asp:TextBox ID="searchTextBox" placeholder="Project Code" runat="server" class="nu_entry" />
            <asp:Button ID="searchProject" Text="Search" runat="server" OnClick="Search_Project" class="button"/>
            <p> Or create a project! </p>
            <asp:Button ID="createProject" Text="Create" runat="server" OnClick="Create_Project" class="button"/> 
        </div>
    
    </form>
</body>
</html>
