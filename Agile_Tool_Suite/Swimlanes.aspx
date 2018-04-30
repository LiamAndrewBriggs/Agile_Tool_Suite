<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Swimlanes.aspx.cs" Inherits="Agile_Tool_Suite.Swimlanes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Swimlanes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Backlog.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="wrapper">
        <div id="mainContent">
            <div class="backlog">
                <h2 class="titles">Swimlanes</h2>
                <div id="laneList" class="backlogList" runat="server"> </div>
                 <asp:Panel runat="server" DefaultButton="Button1">
                    <asp:TextBox ID="newSwimlane" runat="server" class="taskEntryBox" placeholder="Enter new swimlane (maximum 12) in the order you wish to see them and press enter to save"></asp:TextBox>    
                    <asp:Button ID="Button1" runat="server" style="display:none" OnClick="createSwimlane" />
                </asp:Panel>
                <br /><br />
            </div>

            <div class="backlog">
                <h2 class="titles">Tasks</h2>
                <div id="taskList" class="backlogList" runat="server"> </div>
                 <asp:Panel runat="server" DefaultButton="Button2">
                    <asp:TextBox ID="newTask" runat="server" class="taskEntryBox" placeholder="Enter new task"></asp:TextBox>    
                    <asp:Button ID="Button2" runat="server" style="display:none" OnClick="createTask" />
                </asp:Panel>
                <br /><br />
            </div>

        </div>
     </div>


    

    <script>
        
    </script>
</asp:Content>
