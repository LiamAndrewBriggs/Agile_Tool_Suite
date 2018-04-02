using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Agile_Tool_Suite
{
    public partial class SprintBoard : System.Web.UI.Page
    {
        //Session information
        private string user;
        private string project;
        private string sprint;

        //SQL information
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string queryStr;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);
            project = (string)(Session["project"]);

            PageSetUp();
        }

        protected void PageSetUp()
        {
            getActiveSprint();
            showSprintInformation();
        }


        protected void getActiveSprint()
        {
            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT activeSprint FROM agiledb.project WHERE projectID=?projid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?projid", project);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                sprint = reader.GetString(reader.GetOrdinal("activeSprint"));
            }

            conn.Close();

        } 

        private void showSprintInformation()
        {
            List<string> sprintStories = SQL_Helpers.getSprintStories(sprint);
            var css = "";

            int count = 0;

            foreach (string story in sprintStories)
            {
                HtmlGenericControl row = new HtmlGenericControl("div");
                row.Attributes.Add("class", "row");

                HtmlGenericControl storyColumn = new HtmlGenericControl("div");
                storyColumn.Attributes.Add("class", "col-sm-3");
                
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "SELECT storyName FROM agiledb.story WHERE storyID=?storyid";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?storyid", story);

                reader = cmd.ExecuteReader();

                HtmlGenericControl storyTitle = new HtmlGenericControl("h2");
                storyTitle.Attributes.Add("class", "title");

                while (reader.HasRows && reader.Read())
                {
                    storyTitle.InnerText = reader.GetString(reader.GetOrdinal("storyName"));
                }

                conn.Close();

                storyColumn.Controls.Add(storyTitle);
                row.Controls.Add(storyColumn);

                List<string> taskID = getStoryTasks(story, "toDo");

                HtmlGenericControl toDoColumn = new HtmlGenericControl("div");
                toDoColumn.Attributes.Add("class", "col-sm-3");

                HtmlGenericControl list = new HtmlGenericControl("ul");
                list.Attributes.Add("class", "connectedSortable" + count);
                list.Attributes.Add("id", "toDo" + count);

                HtmlGenericControl item;

                foreach (string task in taskID)
                {
                    item = new HtmlGenericControl("li");
                    item.Attributes.Add("data", task);

                    conn = SQL_Helpers.createConnection();
                    conn.Open();

                    queryStr = "SELECT taskName FROM agiledb.task WHERE taskID=?taskid";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?taskid", task);

                    reader = cmd.ExecuteReader();

                    while (reader.HasRows && reader.Read())
                    {
                        item.InnerText = reader.GetString(reader.GetOrdinal("taskName"));
                    }

                    conn.Close();
                    list.Controls.Add(item);
                }

                toDoColumn.Controls.Add(list);

                row.Controls.Add(toDoColumn);

                taskID = getStoryTasks(story, "inProgress");

                HtmlGenericControl inProgressColumn = new HtmlGenericControl("div");
                inProgressColumn.Attributes.Add("class", "col-sm-3");

                list = new HtmlGenericControl("ul");
                list.Attributes.Add("class", "connectedSortable" + count);
                list.Attributes.Add("id", "inProgress" + count);

                foreach (string task in taskID)
                {
                    item = new HtmlGenericControl("li");
                    item.Attributes.Add("data", task);

                    conn = SQL_Helpers.createConnection();
                    conn.Open();

                    queryStr = "SELECT taskName, userAssigned FROM agiledb.task WHERE taskID=?taskid";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?taskid", task);

                    reader = cmd.ExecuteReader();

                    HtmlGenericControl userTag = new HtmlGenericControl("a");

                    while (reader.HasRows && reader.Read())
                    {
                        userTag.Attributes.Add("class", "userName");

                        var names = SQL_Helpers.getUserName(reader.GetString(reader.GetOrdinal("userAssigned"))).Split(' ');
                        string firstName = names[0];
                        string lastName = names[1];


                        userTag.InnerText = firstName[0].ToString() + " " + lastName;
                        item.InnerText = reader.GetString(reader.GetOrdinal("taskName"));
                        item.Controls.Add(userTag);
                    }

                    conn.Close();

                    list.Controls.Add(item);
                }

                inProgressColumn.Controls.Add(list);

                row.Controls.Add(inProgressColumn);

                taskID = getStoryTasks(story, "done");

                HtmlGenericControl doneColumn = new HtmlGenericControl("div");
                doneColumn.Attributes.Add("class", "col-sm-3");

                list = new HtmlGenericControl("ul");
                list.Attributes.Add("class", "connectedSortable" + count);
                list.Attributes.Add("id", "done" + count);

                foreach (string task in taskID)
                {
                    item = new HtmlGenericControl("li");
                    item.Attributes.Add("data", task);

                    conn = SQL_Helpers.createConnection();
                    conn.Open();

                    queryStr = "SELECT taskName, userAssigned FROM agiledb.task WHERE taskID=?taskid";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?taskid", task);

                    reader = cmd.ExecuteReader();

                    HtmlGenericControl userTag = new HtmlGenericControl("a");

                    while (reader.HasRows && reader.Read())
                    {
                        userTag.Attributes.Add("class", "userName");

                        var names = SQL_Helpers.getUserName(reader.GetString(reader.GetOrdinal("userAssigned"))).Split(' ');
                        string firstName = names[0];
                        string lastName = names[1];


                        userTag.InnerText = firstName[0].ToString() + " " + lastName;
                        item.InnerText = reader.GetString(reader.GetOrdinal("taskName"));
                        item.Controls.Add(userTag);
                    }

                    conn.Close();
                    list.Controls.Add(item);
                }

                doneColumn.Controls.Add(list);

                row.Controls.Add(doneColumn);

                table.Controls.Add(row); 

                String scriptText = "";
                scriptText += "$(function() {";
                scriptText += "   $(\"#toDo" + count + ", #inProgress" + count + ", #done" + count + "\").sortable({" +
                    " connectWith: \".connectedSortable" + count + "\"," +
                    " change: function (evt, ui) { " +
                    " document.getElementById('" + selectedTaskID.ClientID +"').value = ui.item[\"0\"].attributes[\"0\"].value;" +
                    " document.getElementById('" + taskDestination.ClientID + "').value = evt.target.id;" +
                    " document.getElementById('" + saveBtn.ClientID + "').click();" +
                    " console.log(document.getElementById('" + taskDestination.ClientID + "').value);" +
                    "}" +
                    "}).disableSelection();";
                scriptText += "});";
                ClientScript.RegisterStartupScript(this.GetType(),
                   "connected" + count, scriptText, true);

                css += @"#toDo" + count + @", #inProgress" + count + @", #done" + count + @" {
                            width: 100%;
                            min-height: 20px;
                            list-style: none;
                            margin: 0;
                            padding: 5px 0 0 0;
                            float: left;
                            margin-right: 10px;
                            padding-bottom: 15px;
                        }

                        #toDo" + count + @" li, #inProgress" + count + @" li, #done" + count + @" li {
                            padding: 5px;
                            display: inline-block;
                            font-size: 1.2em;
                            border: 1px solid #808080;
                            width: 100%;
                            cursor: move;
                            cursor: grab;
                            cursor: -moz-grab;
                            cursor: -webkit-grab;
                        }

                        #toDo" + count + @" li:active, #inProgress" + count + @" li:active, #done" + count + @" li:active {
                            cursor: grabbing;
                            cursor: -moz-grabbing;
                            cursor: -webkit-grabbing;
                        }

                        #toDo" + count + @" li:hover, #inProgress" + count + @" li:hover, #done" + count + @" li:hover {
                            background-color: #bdc1bf;
                        }

                        
                        ";

                count++;
            }

            htmlCss.InnerHtml = css;
        }

        protected List<string> getStoryTasks(string storyID, string status)
        {
            List<string> tempTaskIDs = SQL_Helpers.getStoryTasks(storyID);
            List<string> taskIDs = new List<string>();

            foreach (string task in tempTaskIDs)
            {
                conn.Open();

                queryStr = "SELECT taskID FROM agiledb.task WHERE taskID=?taskid AND taskStatus=?status";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?taskid", task);
                cmd.Parameters.AddWithValue("?status", status);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    taskIDs.Add(reader.GetString(reader.GetOrdinal("taskID")));
                }

                conn.Close();
            }
            
            return taskIDs;
        }

        protected void saveSprintBoard(object sender, EventArgs e)
        {
            string taskID = selectedTaskID.Value;
            string destination = taskDestination.Value;

            if(destination.Contains("toDo"))
            {
                destination = "toDo";
            }
            else if (destination.Contains("inProgress"))
            {
                destination = "inProgress";

                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "UPDATE agiledb.task SET userAssigned = ?userid WHERE taskID=?taskid";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?userid", project);
                cmd.Parameters.AddWithValue("?taskid", taskID);

                cmd.ExecuteReader();
                conn.Close();
            }
            else if (destination.Contains("done"))
            {
                destination = "done";
            }

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "UPDATE agiledb.task SET taskStatus = ?status WHERE taskID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?status", destination);
            cmd.Parameters.AddWithValue("?id", taskID);

            cmd.ExecuteReader();
            conn.Close();

            SQL_Helpers.checkStatus(project, sprint);

            Response.Redirect(Request.RawUrl);
        }
    }
}