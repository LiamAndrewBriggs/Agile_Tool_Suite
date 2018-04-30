using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Agile_Tool_Suite
{
    public partial class KanbanBoard : System.Web.UI.Page
    {
        //Session information
        private string project;

        //SQL information
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string queryStr;

        //variables
        private int columns = 0;
        private List<string> columnNames = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            project = (string)(Session["project"]);

            createBoard();
            getTasks();
        }

        private void createBoard()
        {
            List<string> swimlanes = new List<string>();

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT swimlaneID FROM agiledb.projectswimlanes WHERE projectID=?projectid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?projectid", project);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                swimlanes.Add(reader.GetString(reader.GetOrdinal("swimlaneID")));
            }

            conn.Close();

            HtmlGenericControl row = new HtmlGenericControl("div");
            row.Attributes.Add("class", "row");

            if (project != null)
            {
                columns = 12 / swimlanes.Count;
            }

            foreach (string swimLane in swimlanes)
            {
                HtmlGenericControl storyColumn = new HtmlGenericControl("div");
                storyColumn.Attributes.Add("class", "col-sm-" + columns);

                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "SELECT swimlaneName FROM agiledb.swimlanes WHERE swimlaneID=?swimlaneid";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?swimlaneid", swimLane);

                reader = cmd.ExecuteReader();

                HtmlGenericControl storyTitle = new HtmlGenericControl("h1");
                storyTitle.Attributes.Add("class", "title");

                while (reader.HasRows && reader.Read())
                {
                    storyTitle.InnerText = reader.GetString(reader.GetOrdinal("swimlaneName"));
                    columnNames.Add(reader.GetString(reader.GetOrdinal("swimlaneName")));
                }

                conn.Close();

                storyColumn.Controls.Add(storyTitle);
                row.Controls.Add(storyColumn);
            }

            table.Controls.Add(row);
        }

        private void getTasks()
        {
            List<string> kanbanTasks = new List<string>();

            HtmlGenericControl row = new HtmlGenericControl("div");
            row.Attributes.Add("class", "row");

            foreach (string title in columnNames)
            {
                HtmlGenericControl storyColumn = new HtmlGenericControl("div");
                storyColumn.Attributes.Add("class", "col-sm-" + columns);

                HtmlGenericControl list = new HtmlGenericControl("ul");
                list.Attributes.Add("class", "connectedSortable");

                string newTitle = Regex.Replace(title, @"\s+", "_");

                list.Attributes.Add("id", newTitle);

                List<string> taskID = getTasks(title);

                HtmlGenericControl item;

                foreach (string task in taskID)
                {
                    item = new HtmlGenericControl("li");
                    item.Attributes.Add("data", task);

                    item.Attributes.Add("data", task);

                    conn = SQL_Helpers.createConnection();
                    conn.Open();

                    queryStr = "SELECT taskName FROM agiledb.kanbantask WHERE kanbantaskID=?taskid";

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

                storyColumn.Controls.Add(list);
                row.Controls.Add(storyColumn);
            }

            table.Controls.Add(row);

            string script = "";
            string css1 = "";
            string css2 = "";
            string css3 = "";
            string css4 = "";

            bool firstTime = true;

            foreach (string title in columnNames)
            {
                string newTitle = Regex.Replace(title, @"\s+", "_");

                if (firstTime == false)
                {
                    script += ", #" + newTitle;
                    css1 += ", #" + newTitle;
                    css2 += ", #" + newTitle + " li";
                    css3 += ", #" + newTitle + " li:active";
                    css4 += ", #" + newTitle + " li:hover";
                }
                else
                {
                    script = "#" + newTitle;
                    css1 = "#" + newTitle;
                    css2 = "#" + newTitle + " li";
                    css3 = "#" + newTitle + " li:active";
                    css4 = "#" + newTitle + " li:hover";

                    firstTime = false;
                }
            }

            var css = "";
            String scriptText = "";
            scriptText += "$(function() {";
            scriptText += "   $(\"" + script + "\").sortable({" +
                " connectWith: \".connectedSortable\"," +
                " change: function (evt, ui) { " +
                " document.getElementById('" + selectedTaskID.ClientID + "').value = ui.item[\"0\"].attributes[\"0\"].value;" +
                " document.getElementById('" + taskDestination.ClientID + "').value = evt.target.id;" +
                " document.getElementById('" + saveBtn.ClientID + "').click();" +
                " console.log(document.getElementById('" + taskDestination.ClientID + "').value);" +
                "}" +
                "}).disableSelection();";
            scriptText += "});";
            ClientScript.RegisterStartupScript(this.GetType(),
               "connected", scriptText, true);

            css += css1 + @" {
                            width: 100%;
                            min-height: 20px;
                            list-style: none;
                            margin: 0;
                            padding: 5px 0 0 0;
                            float: left;
                            margin-right: 10px;
                            padding-bottom: 15px;
                        }"

                        + css2 + @" {
                            padding: 5px;
                            display: inline-block;
                            font-size: 1.2em;
                            border: 1px solid #808080;
                            width: 100%;
                            cursor: move;
                            cursor: grab;
                            cursor: -moz-grab;
                            cursor: -webkit-grab;
                        }"

                        + css3 + @" {
                            cursor: grabbing;
                            cursor: -moz-grabbing;
                            cursor: -webkit-grabbing;
                        }"

                        + css4 + @" {
                            background-color: #bdc1bf;
                        }

                        
                        ";

            kanbanhtmlCss.InnerHtml = css;
        }

        private List<string> getTasks(string colomn)
        {
            List<string> tempTasks = new List<string>();
            List<string> toUpdate = new List<string>();
            List<string> tasks = new List<string>();

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT taskID FROM agiledb.kanbanprojecttasks WHERE projectID=?projectid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?projectid", project);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                tempTasks.Add(reader.GetString(reader.GetOrdinal("taskID")));
            }

            conn.Close();

            foreach (string task in tempTasks)
            {
                conn.Open();

                queryStr = "SELECT swimlanePosition FROM agiledb.kanbantask WHERE kanbantaskID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", task);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    if (reader.GetString(reader.GetOrdinal("swimlanePosition")).Equals(colomn))
                    {
                        tasks.Add(task);
                    }
                    else if (reader.GetString(reader.GetOrdinal("swimlanePosition")).Equals("ToGiveName"))
                    {
                        tasks.Add(task);
                        toUpdate.Add(task);
                    }
                }

                conn.Close();
            }

            foreach (string task in toUpdate)
            {
                conn.Open();

                queryStr = "UPDATE agiledb.kanbantask SET swimlanePosition = ?pos WHERE kanbantaskID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?pos", colomn);
                cmd.Parameters.AddWithValue("?id", task);

                cmd.ExecuteReader();
                conn.Close();
            }


            return tasks;
        }

        protected void saveKanbanBoard(object sender, EventArgs e)
        {
            string taskID = selectedTaskID.Value;
            string destination = taskDestination.Value;

            destination = destination.Replace("_", " ");

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "UPDATE agiledb.kanbantask SET  swimlanePosition=?status WHERE kanbantaskID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?status", destination);
            cmd.Parameters.AddWithValue("?id", taskID);

            cmd.ExecuteReader();
            conn.Close();

        }
    }
}