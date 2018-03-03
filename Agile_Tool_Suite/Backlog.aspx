<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Backlog.aspx.cs" Inherits="Agile_Tool_Suite.Backlog" validateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Backlog
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Backlog.css" rel="stylesheet" />
    <link href="Css/Theme.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="wrapper">
        <div id="mainContent">
            <div id="sprints">
                <asp:Button ID="createSprint" Text="Create Sprint" runat="server" OnClick="createSprintButton" class="button" />
                <h2 class="titles">Sprints</h2>
                <div class="panel-group" id="sprintAccordian" runat="server"> </div>
            </div>
            <div id="backlog">
                <button class="button" id="createBacklogItem">Create Backlog Item</button>
                <h2 class="titles">Backlog Stories</h2>
                <div id="backlogList" class="backlogList" runat="server"> 

                     
                    <%--<ul id="sortable2" class="connectedSortable">
                      <li class="ui-state-highlight">Item 1</li>
                      <li class="ui-state-highlight">Item 2</li>
                      <li class="ui-state-highlight">Item 3</li>
                      <li class="ui-state-highlight">Item 4</li>
                      <li class="ui-state-highlight">Item 5</li>
                    </ul>--%>
                </div>
            </div>

            <asp:HiddenField ID="backlogItemhf" runat="server" Value="null" />
            <asp:HiddenField ID="backlogOrderhf" runat="server" Value="null" />
            <asp:Button ID = "viewbtn" runat = "server" OnClick = "viewBacklogItem" style = "display:none" />
            <asp:Button ID = "destroySessionData" runat = "server" OnClick = "destroySession" style = "display:none" />
            
        </div>

        <div id="backlog-wrapper">
            <div class="container-fluid" id ="backlogContainer">
                    <div class="row">
                        <div class="col-lg-12">
                            <header class="title">Story</header>

                            <asp:TextBox ID="backlogItemName" TextMode="multiline" width="100%" Rows="2" style='margin-top:10px; margin-bottom:10px; resize: none; overflow:hidden; font-size:20px; ' 
                                runat="server" onkeydown="setHeight(this);" Placeholder="As a *insert role here* I'd like to *insert action to be completed*"/>
                            
                            <p class="backlogLabel">Status:</p>
                            <asp:DropDownList ID="backlogItemStatus" class="info" runat="server"> 
                                 <asp:ListItem Text="To Do" Value="toDo"></asp:ListItem>
                                 <asp:ListItem Text="In Progress" Value="inProgress"></asp:ListItem>
                                 <asp:ListItem Text="Done" Value="done"></asp:ListItem>
                             </asp:DropDownList>
                            <br /><br />
                            <p>Story Points:</p>
                            <asp:DropDownList ID="backlogItemStoryPoints" class="info" runat="server"> 
                                <asp:ListItem Text="1 - Extra Small" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2 - Small" Value="2"></asp:ListItem>
                                <asp:ListItem Text="4 - Medium" Value="4"></asp:ListItem>
                                <asp:ListItem Text="8 - Large" Value="8"></asp:ListItem>
                                <asp:ListItem Text="16 - Extra Large" Value="16"></asp:ListItem>
                             </asp:DropDownList>
                            
                            <asp:TextBox ID="backlogItemDescription" TextMode="multiline" width="100%" Rows="3" style='margin-top:10px; margin-bottom:10px; resize: none; overflow:hidden; font-size:15px; ' 
                                runat="server" onkeydown="setHeight(this);" Placeholder="Describe what this story item should accomplish in more detail" />

                            <button class="button" id="closeBacklog">Close</button>
                            <asp:Button ID="registerBacklogButton" Text="Save" runat="server" OnClick="createBacklogItem" class="button" />
                         </div>
                    </div>
            </div>
        </div>
    </div>

    <script>
        $("#createBacklogItem, #registerBacklogButton, #storyInfo").click(function (e) {
            e.preventDefault();

            if (this.getAttribute("id") == 'storyInfo') {
                var id = this.getAttribute("data-id");
                document.getElementById('<%=backlogItemhf.ClientID%>').value = id;
                setOrder();
                $('#<%= viewbtn.ClientID %>').click();
            }
            else {
                $("#wrapper").toggleClass("createDisplaced", true);
            }
            
        });

        $("#closeBacklog").click(function (e) {
            e.preventDefault();

            setOrder();

            $("#wrapper").toggleClass("createDisplaced");
            document.getElementById('<%=backlogItemName.ClientID%>').value = "";
            document.getElementById('<%=backlogItemStatus.ClientID%>').value = "";
            document.getElementById('<%=backlogItemStoryPoints.ClientID%>').value = "";
            document.getElementById('<%=backlogItemDescription.ClientID%>').value = "";
            $('#<%= destroySessionData.ClientID %>').click();

        });


        $(function () {
            $("#backlogSort, #sortable2").sortable({
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
