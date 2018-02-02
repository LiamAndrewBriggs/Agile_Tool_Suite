<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Agile_Tool_Suite.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <link href="Css/Registration.css" rel="stylesheet" />
    <link href="Css/Theme.css" rel="stylesheet" />
</head>
<body>

    <form class="form1" id="form1" runat="server" autocomplete="off">

     
        <div class="registration_form">
            <header class="title">Registration</header>
            <p>Full Name</p>
            <asp:TextBox ID="firstnameTextBox" placeholder="First Name" class="info" runat="server" />
            <asp:TextBox ID="surnameTextBox" placeholder="Last Name" class="info" runat="server" />
            <p>Contact Information</p>
            <asp:TextBox ID="emailTextBox" placeholder="Email" class="info" runat="server" />
            <asp:Label ID="emailError" Text="Email already registered to an account" visible="false" class="error" runat="server" />
            <p>Username</p>
            <asp:TextBox ID="usernameTextBox" placeholder="Username" class="info" runat="server" />
            <asp:Label ID="usernameError" Text="Username already registered to an account" visible="false" class="error" runat="server" />
            <p>Password</p>
            <asp:TextBox ID="passwordTextBox" placeholder="Password" class="info" type="password" runat ="server" />
            <br />
            <br />
            <asp:TextBox ID="confirmPasswordTextBox" placeholder="Confirm Password" class="info" type="password" runat ="server" />
            <asp:Label ID="passwordError" Text="Passwords do not match" visible="false" class="error" runat="server" />
            <br />
            <br />
            <asp:Button ID="registerButton" Text="Register" runat="server" OnClick="registerEventMethod" class="button" />
         </div>
    </form>
</body>
</html>
