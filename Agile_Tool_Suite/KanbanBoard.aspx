<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="KanbanBoard.aspx.cs" Inherits="Agile_Tool_Suite.KanbanBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Kanban Board
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Boards.css" rel="stylesheet" />
    <style type="text/css" runat="server" id="kanbanhtmlCss"></style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="table" runat="server">    </div>


    <asp:HiddenField ID="selectedTaskID" runat="server" Value="null" />
    <asp:HiddenField ID="taskDestination" runat="server" Value="null" />
    <asp:Button ID = "saveBtn" runat = "server" OnClick = "saveKanbanBoard" style = "display:none" />
</asp:Content>
