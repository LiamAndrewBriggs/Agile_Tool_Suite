<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Agile_Tool_Suite.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <link href="~/Css/Home.css" rel="stylesheet" type="text/css"/>
    <link href="Css/Theme.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" class="username" runat="server" autocomplete="off">
    <div class="loginForm">
        <header class="title">Login</header>
        <p>UserName</p>
        <asp:TextBox ID="usernameTextBox" placeholder="Username" runat="server" class="entry" />
        <p>Password</p>
        <asp:TextBox ID="passwordTextBox" placeholder="Password" runat="server" type="password" class="entry" />
        <asp:Label ID="errorMessage" Text="" runat="server" class="errors"/>
        <br />
        <br />
        <asp:Button ID="submitButton" Text="Log in" runat="server" OnClick="LogInMethod" class="button"/>
        <br />
        <br />
        <a href="Registration.aspx">Sign Up</a>
    
    </div>
        </form>
    
</body>
</html>
