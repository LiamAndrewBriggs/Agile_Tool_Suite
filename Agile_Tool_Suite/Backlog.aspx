<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Backlog.aspx.cs" Inherits="Agile_Tool_Suite.Backlog" validateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Backlog
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Backlog.css" rel="stylesheet" />
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="wrapper">
        <div id="mainContent">
            <div id="sprints">
                <asp:Button id="endSprint" runat = "server" class="button" OnClick = "endSprintButton" Text="Complete Sprint" />
                <button class="button" id="createSprint">Create Sprint</button>
                <h2 class="titles">Sprints</h2>
                <div id="startSprintHide" runat="server">
                    <p style="margin-right:15px; color:#4CAF50; font-size:large">Choose the next sprint to start: </p>
                    <asp:DropDownList ID="sprintList" class="info" runat="server">  </asp:DropDownList>
                    <asp:Button ID="startSprint" runat = "server" OnClick = "startSprintButton" Text="Start Sprint" />
                </div>
                <div class="panel-group" id="sprintAccordian" runat="server"> </div>
                <div id ="sprintInput" class="container-fluid">
                    <h3 class="titles">Create Sprint</h3>
                    <button class="button" id="saveSprint">Save Sprint</button>                    
                    <div class="container-fluid">
                        <p>Add Backlog Story To Sprint (drag story from the backlog):</p>
                        <ul id="sprintCreate" class="connectedSortable"> </ul>
                    </div>
                </div>
            </div>

            <div id="backlog">
                <button class="button" id="createBacklogItem">Create Backlog Item</button>
                <h2 class="titles">Backlog Stories</h2>
                <div id="backlogList" class="backlogList" runat="server"> </div>
            </div>

            <asp:HiddenField ID="backlogItemhf" runat="server" Value="null" />
            <asp:HiddenField ID="backlogOrderhf" runat="server" Value="null" />
            <asp:HiddenField ID="taskIDhf" runat="server" Value="null" />
            <asp:HiddenField ID="sprintStorieshf" runat="server" Value="null" />
            <asp:Button ID = "viewbtn" runat = "server" OnClick = "viewBacklogItem" style = "display:none" />
            <asp:Button ID = "destroySessionData" runat = "server" OnClick = "destroySession" style = "display:none" />
            <asp:Button ID = "getTaskInfo" runat = "server" OnClick = "viewTaskItem" style = "display:none" />
            <asp:Button ID = "createSprintButtons" runat = "server" OnClick = "createSprintButton" style = "display:none" />
            
        </div>
     </div>

        <div id="backlog-wrapper">
            <div class="container-fluid" id ="backlogContainer">
                    <div class="row">
                        <div class="col-lg-12">
                            <header class="title">Story</header>

                            <asp:TextBox ID="backlogItemName" TextMode="multiline" width="100%" Rows="2" style='margin-top:10px; margin-bottom:10px; resize: none; overflow:hidden; font-size:20px; ' 
                                runat="server" onkeydown="setHeight(this);" Placeholder="As a *insert role here* I'd like to *insert action to be completed*"/>
                            
                            <p style="margin-right:15px;">Status:</p>
                            <asp:DropDownList ID="backlogItemStatus" class="info" runat="server"> 
                                 <asp:ListItem Text="To Do" Value="toDo"></asp:ListItem>
                                 <asp:ListItem Text="In Progress" Value="inProgress"></asp:ListItem>
                                 <asp:ListItem Text="Done" Value="done"></asp:ListItem>
                             </asp:DropDownList>
                            <br /><br />
                            <p style="margin-right:15px;">Story Points:</p>
                            <asp:DropDownList ID="backlogItemStoryPoints" class="info" runat="server"> 
                                <asp:ListItem Text="1 - Extra Small" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2 - Small" Value="2"></asp:ListItem>
                                <asp:ListItem Text="4 - Medium" Value="4"></asp:ListItem>
                                <asp:ListItem Text="8 - Large" Value="8"></asp:ListItem>
                                <asp:ListItem Text="16 - Extra Large" Value="16"></asp:ListItem>
                             </asp:DropDownList>
                            
                            <asp:TextBox ID="backlogItemDescription" TextMode="multiline" width="100%" Rows="3" style='margin-top:10px; margin-bottom:10px; resize: none; overflow:hidden; font-size:15px; ' 
                                runat="server" onkeydown="setHeight(this);" Placeholder="Describe what this story item should accomplish in more detail" />

                            <div id="visable">
                                <p>Tasks:</p>
                                <div id="tasklist" class="backlogList" runat="server"> </div>
                                <asp:Panel runat="server" DefaultButton="Button1">
                                   <asp:TextBox ID="newTask" runat="server" class="taskEntryBox" placeholder="Enter new task and press enter to save"></asp:TextBox>    
                                   <asp:Button ID="Button1" runat="server" style="display:none" OnClick="createTask" />
                                </asp:Panel>
                                <br /><br />
                            </div>

                            <button class="button" id="closeBacklog">Close</button>
                            <asp:Button ID="registerBacklogButton" Text="Save Story" runat="server" OnClick="createBacklogItem" class="button" />
                         </div>
                    </div>
                    <div id="taskvisable">
                        <div class="row" id="taskView">
                            <div class="col-lg-12">
                             <header class="title">Story Tasks</header>

                                    <asp:TextBox ID="taskName" runat="server" style="width: 100%" Text="Please selected a task to view details"/>

                                    <div id="space">
                                        <p class="backlogLabel">Status:</p>
                                        <asp:DropDownList ID="taskStatus" class="info" runat="server"> 
                                             <asp:ListItem Text="To Do" Value="toDo"></asp:ListItem>
                                             <asp:ListItem Text="In Progress" Value="inProgress"></asp:ListItem>
                                             <asp:ListItem Text="Done" Value="done"></asp:ListItem>
                                         </asp:DropDownList>
                                    </div>

                                    <asp:Button ID="saveStoryButton" Text="Save Task" runat="server" OnClick="saveTaskItem" class="button" style="margin-bottom: 20px" />
                            </div>
                            </div>
                        </div>
                    </div>
        </div>
    

    <script>
        $("#createBacklogItem, #registerBacklogButton, #storyInfo, #taskInfo").click(function (e) {
            e.preventDefault();

            document.getElementById('<%=taskName.ClientID%>').value = "";
            
            if (this.getAttribute("id") == 'storyInfo') {
                var id = this.getAttribute("data-id");
                document.getElementById('<%=backlogItemhf.ClientID%>').value = id;
                document.getElementById('<%=taskIDhf.ClientID%>').value = "null";
                setOrder();
                $('#<%= viewbtn.ClientID %>').click();
            }
            else if (this.getAttribute("id") == 'taskInfo') {
                var id = this.getAttribute("data-id");
                document.getElementById('<%=taskIDhf.ClientID%>').value = id;
                $('#<%= getTaskInfo.ClientID %>').click();
            }
            else {
                $("#wrapper").toggleClass("createDisplaced", true);
                $('#visable').hide();
                $('#taskvisable').hide();
            }
            
        });

        $("#createSprint").click(function (e) {
            e.preventDefault();

            $('#sprintInput').show();
        });

        $("#saveSprint").click(function (e) {
            e.preventDefault();
            $('#sprintInput').hide();

            document.getElementById('<%=sprintStorieshf.ClientID%>').value = $("#sprintCreate").html();
            $('#<%= createSprintButtons.ClientID %>').click();
        });

        $("#closeBacklog").click(function (e) {
            e.preventDefault();

            setOrder();

            $('#taskvisable').hide();
            $("#wrapper").toggleClass("createDisplaced");
            document.getElementById('<%=backlogItemName.ClientID%>').value = "";
            document.getElementById('<%=backlogItemStatus.ClientID%>').value = "";
            document.getElementById('<%=backlogItemStoryPoints.ClientID%>').value = "";
            document.getElementById('<%=backlogItemDescription.ClientID%>').value = "";
            document.getElementById('<%=taskName.ClientID%>').value = "";
            $('#<%= destroySessionData.ClientID %>').click();

        });


        $(function () {
            $("#backlogSort, #sprintCreate").sortable({
                connectWith: ".connectedSortable"
                }).disableSelection();
            });

        function setHeight(txtdesc) {
            txtdesc.style.height = txtdesc.scrollHeight + "px";
        }

        function setOrder() {
            document.getElementById('<%=backlogOrderhf.ClientID%>').value = $("#backlogSort").html();
        }

        function codeAddress() {
            if (document.getElementById('<%=backlogItemName.ClientID%>').value != '') {
                $("#wrapper").toggleClass("createDisplaced", true);
            }
        }
        window.onload = codeAddress;
    </script>
</asp:Content>
