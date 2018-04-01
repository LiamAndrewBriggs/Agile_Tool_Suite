<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SprintBoard.aspx.cs" Inherits="Agile_Tool_Suite.SprintBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
        Sprint Board
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Boards.css" rel="stylesheet" />
    <style type="text/css" runat="server" id="htmlCss"></style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="table" runat="server">
        <div class="row">
          <div class="col-sm-3"><h1 class="titles"> Story </h1></div>
          <div class="col-sm-3"><h1 class="titles"> To Do </h1></div>
          <div class="col-sm-3"><h1 class="titles"> Doing </h1></div>
          <div class="col-sm-3"><h1 class="titles"> Done </h1></div>
        </div>
    </div>

    <script>
       
     </script>

</asp:Content>
