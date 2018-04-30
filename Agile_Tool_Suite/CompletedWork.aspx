<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CompletedWork.aspx.cs" Inherits="Agile_Tool_Suite.CompletedWork" %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Completed Work
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Backlog.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="titles">Burndown Chart </h2>

    <asp:Chart ID="BurndownChart" runat="server" Height="500px" Width="1300px">
    </asp:Chart>

    <div id="backlog">
        <h2 class="titles">Completed Backlog Stories</h2>
        <div id="backlogList" class="backlogList" runat="server"> </div>
    </div>
</asp:Content>
