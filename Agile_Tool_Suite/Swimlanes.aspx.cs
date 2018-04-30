using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;


namespace Agile_Tool_Suite
{
    public partial class Swimlanes : System.Web.UI.Page
    {
        //SQL information
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string queryStr;

        private string project;

        protected void Page_Load(object sender, EventArgs e)
        {
            project = (string)(Session["project"]);

            getSwimlanes();
            getTasks();
        }

        protected void createSwimlane(object sender, EventArgs e)
        {
            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "INSERT INTO agiledb.swimlanes (swimlaneName) VALUES(?name)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?name", newSwimlane.Text);

            cmd.ExecuteReader();
            conn.Close();

            conn.Open();

            string swimlaneID = "";

            queryStr = "SELECT swimlaneID FROM agiledb.swimlanes ORDER BY swimlaneID DESC LIMIT 1";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                swimlaneID = reader.GetString(reader.GetOrdinal("swimlaneID"));
            }

            conn.Close();
            conn.Open();

            queryStr = "INSERT INTO agiledb.projectswimlanes(projectID, swimlaneID) VALUES(?project, ?swimlane)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?project", project);
            cmd.Parameters.AddWithValue("?swimlane", swimlaneID);
            reader = cmd.ExecuteReader();

            conn.Close();

            newSwimlane.Text = "";

            getSwimlanes();
        }

        private void getSwimlanes()
        {
            laneList.InnerHtml = null;
            
            HtmlGenericControl list = new HtmlGenericControl("ul");
            list.Attributes.Add("id", "storyList");

            conn = SQL_Helpers.createConnection();
            conn.Open();

            List<string> swimlanes = new List<string>();

            queryStr = "SELECT swimlaneID FROM agiledb.projectswimlanes WHERE projectID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", project);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                swimlanes.Add(reader.GetString(reader.GetOrdinal("swimlaneID")));
            }

            conn.Close();

            foreach (string swimlane in swimlanes)
            {
                conn.Open();

                queryStr = "SELECT swimlaneName FROM agiledb.swimlanes WHERE swimlaneID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", swimlane);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    HtmlGenericControl item = new HtmlGenericControl("li");
                    item.Attributes.Add("id", "taskInfo");
                    item.InnerText = reader.GetString(reader.GetOrdinal("swimlaneName"));

                    list.Controls.Add(item);
                }

                conn.Close();
            }

            laneList.Controls.Add(list);
        }

        protected void createTask(object sender, EventArgs e)
        {
            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "INSERT INTO agiledb.kanbantask (taskName, swimlanePosition) VALUES(?name, ?position)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?name", newTask.Text);
            cmd.Parameters.AddWithValue("?position", "ToGiveName");

            cmd.ExecuteReader();
            conn.Close();

            conn.Open();

            string kanbantaskID = "";

            queryStr = "SELECT kanbantaskID FROM agiledb.kanbantask ORDER BY kanbantaskID DESC LIMIT 1";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                kanbantaskID = reader.GetString(reader.GetOrdinal("kanbantaskID"));
            }

            conn.Close();
            conn.Open();

            queryStr = "INSERT INTO agiledb.kanbanprojecttasks(projectID, taskID) VALUES(?project, ?task)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?project", project);
            cmd.Parameters.AddWithValue("?task", kanbantaskID);
            reader = cmd.ExecuteReader();

            conn.Close();

            newTask.Text = "";

            getTasks();
        }

        private void getTasks()
        {
            taskList.InnerHtml = null;

            HtmlGenericControl list = new HtmlGenericControl("ul");
            list.Attributes.Add("id", "storyList");

            conn = SQL_Helpers.createConnection();
            conn.Open();

            List<string> tasks = new List<string>();

            queryStr = "SELECT taskID FROM agiledb.kanbanprojecttasks WHERE projectID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", project);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                tasks.Add(reader.GetString(reader.GetOrdinal("taskID")));
            }

            conn.Close();

            foreach (string task in tasks)
            {
                conn.Open();

                queryStr = "SELECT taskName FROM agiledb.kanbantask WHERE kanbantaskID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", task);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    HtmlGenericControl item = new HtmlGenericControl("li");
                    item.Attributes.Add("id", "taskInfo");
                    item.InnerText = reader.GetString(reader.GetOrdinal("taskName"));

                    list.Controls.Add(item);
                }

                conn.Close();
            }

            taskList.Controls.Add(list);
        }

    }
}