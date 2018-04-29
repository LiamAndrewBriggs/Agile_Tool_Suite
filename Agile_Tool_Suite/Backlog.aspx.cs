using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Agile_Tool_Suite
{
    public partial class Backlog : System.Web.UI.Page
    {
        //Session information
        private string user;
        private string project;
        private string backlog;
        private string story;
        private string currentSprint = "0";

        //SQL information
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string queryStr;

        //local variables 
        private string editStory = "false";
        private string editStoryID;
        private int backlogSize;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);
            project = (string)(Session["project"]);
            story = (string)(Session["storyInfo"]);

            if (story == null)
            {
                editStory = "false";
            }
            else
            {
                editStory = "true";
                editStoryID = story;
            }


            if (user == null)
            {
                Response.BufferOutput = true;
                Response.Redirect("Home.aspx", false);
            }
            else
            {
                PageSetUp();
            }
        }

        protected void PageSetUp()
        {
            getBacklogID();
            getSprints();
            getBacklog();

        }

        private void getBacklogID()
        {
            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT backlogID FROM agiledb.project WHERE projectID=?projid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?projid", project);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                backlog = reader.GetString(reader.GetOrdinal("backlogID"));
            }
        }

        private void getSprints()
        {
            List<string> sprintIDs = getSprintIDs();

            string activeSprint = "0", startDate = "0", sprintLength = "0";

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT activeSprint FROM agiledb.project WHERE projectID=?projid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?projid", project);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                activeSprint = reader.GetString(reader.GetOrdinal("activeSprint"));
            }

            conn.Close();

            if (activeSprint.Equals("0"))
            {
                startSprintHide.Visible = true;
            }
            else
            {
                startSprintHide.Visible = false;

                conn.Open();

                queryStr = "SELECT sprintStartDate, sprintLength FROM agiledb.sprints WHERE sprintID=?sprintid";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?sprintid", activeSprint);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    startDate = reader.GetString(reader.GetOrdinal("sprintStartDate"));
                    sprintLength = reader.GetString(reader.GetOrdinal("sprintLength"));
                }

                conn.Close();
            }

            int count = 1;

            foreach (string sprint in sprintIDs)
            {
                string status = "";

                conn.Open();

                queryStr = "SELECT sprintStatus FROM agiledb.sprints WHERE sprintID=?sprintid";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?sprintid", sprint);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    status = reader.GetString(reader.GetOrdinal("sprintStatus"));
                }

                conn.Close();

                if (!status.Equals("done"))
                {
                    HtmlGenericControl panel = new HtmlGenericControl("div");
                    panel.Attributes.Add("class", "panel panel-default");

                    HtmlGenericControl panelHeading = new HtmlGenericControl("div");
                    panelHeading.Attributes.Add("class", "panel-heading");

                    HtmlGenericControl heading = new HtmlGenericControl("h4");
                    heading.Attributes.Add("class", "panel-title");

                    HtmlGenericControl dataToggle = new HtmlGenericControl("a");
                    dataToggle.Attributes.Add("data-toggle", "collapse");
                    dataToggle.Attributes.Add("data-parent", "#sprintAccordian");
                    dataToggle.Attributes.Add("href", "#collapse" + count);
                    dataToggle.InnerText = "Sprint " + count;

                    int days = Convert.ToInt32(sprintLength) * 7;

                    DateTime systemStartDate = DateTime.Now;
                    DateTime currentDate = DateTime.Now;
                    DateTime endDate = systemStartDate.AddDays(days);

                    int daysLeft = 0;

                    if (activeSprint.Equals(sprint))
                    {
                        systemStartDate = Convert.ToDateTime(startDate);
                        currentSprint = sprint;
                        double difference = (currentDate - systemStartDate).TotalDays;

                        daysLeft = days - Convert.ToInt32(Math.Ceiling(difference));

                        if (daysLeft < 0)
                        {
                            panelHeading.Attributes.Add("style", "background-color: #ff0000!important");
                        }
                        else
                        {
                            panelHeading.Attributes.Add("style", "background-color: #4CAF50!important");
                        }
                    }

                    sprintList.Items.Insert(count - 1, new ListItem("Sprint " + count, sprint));

                    heading.Controls.Add(dataToggle);
                    panelHeading.Controls.Add(heading);
                    panel.Controls.Add(panelHeading);

                    HtmlGenericControl content = new HtmlGenericControl("div");
                    content.Attributes.Add("id", "collapse" + count);
                    content.Attributes.Add("class", "panel-collapse collapse out");

                    HtmlGenericControl contentBody = new HtmlGenericControl("div");
                    contentBody.Attributes.Add("class", "panel-body");

                    if (activeSprint.Equals(sprint))
                    {
                        bool completed = isSprintFinished(sprint);

                        HtmlGenericControl timeLeft = new HtmlGenericControl("p");

                        if (daysLeft < 0)
                        {
                            timeLeft.InnerText = "This sprint ended on: " + endDate.ToString("d/M/yyyy") + " please conduct the retrospective and planning then click the complete sprint button to begin the next one";
                        }
                        else if (completed)
                        {
                            timeLeft.InnerText = "All stories completed please conduct the retrospective and planning then click the complete sprint button to begin the next one";
                        }
                        else
                        {
                            timeLeft.InnerText = "You started this sprint on: " + startDate + " you have " + daysLeft + " days till the sprint ends on: " + endDate.ToString("d/M/yyyy");
                        }


                        HtmlGenericControl breakTag = new HtmlGenericControl("br");

                        HtmlGenericControl header = new HtmlGenericControl("p");
                        header.InnerText = "Stories: ";

                        contentBody.Controls.Add(timeLeft);
                        contentBody.Controls.Add(breakTag);
                        contentBody.Controls.Add(breakTag);
                        contentBody.Controls.Add(header);
                    }
                    else
                    {
                        contentBody.InnerText = "Stories: ";
                    }

                    List<string> storyIDs = getStoryIDs(sprint);

                    HtmlGenericControl list = new HtmlGenericControl("ul");
                    //list.Attributes.Add("class", "connectedSortable");
                    list.Attributes.Add("id", "sprintView");

                    foreach (string story in storyIDs)
                    {
                        conn.Open();

                        queryStr = "SELECT storyName FROM agiledb.story WHERE storyID=?id";

                        cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                        cmd.Parameters.AddWithValue("?id", story);

                        reader = cmd.ExecuteReader();

                        while (reader.HasRows && reader.Read())
                        {
                            HtmlGenericControl item = new HtmlGenericControl("li");
                            item.Attributes.Add("class", "ui-state-default");
                            item.Attributes.Add("id", "storyInfo");
                            item.Attributes.Add("data-id", story);
                            item.InnerText = reader.GetString(reader.GetOrdinal("storyName"));

                            list.Controls.Add(item);
                        }

                        conn.Close();
                    }

                    contentBody.Controls.Add(list);
                    content.Controls.Add(contentBody);
                    panel.Controls.Add(content);
                    sprintAccordian.Controls.Add(panel);

                    count++;
                }
            }
        }

        protected bool isSprintFinished(String sprint)
        {
            List<string> stories = getStoryIDs(sprint);
            List<string> tasks = new List<string>();

            foreach (string story in stories)
            {
                conn.Open();

                queryStr = "SELECT taskID FROM agiledb.storytasks WHERE storyID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", story);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    tasks.Add(reader.GetString(reader.GetOrdinal("taskID")));
                }

                conn.Close();

                foreach (string task in tasks)
                {
                    string status = "";

                    conn.Open();

                    queryStr = "SELECT taskStatus FROM agiledb.task WHERE taskID=?id";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?id", task);

                    reader = cmd.ExecuteReader();

                    while (reader.HasRows && reader.Read())
                    {
                        status = reader.GetString(reader.GetOrdinal("taskStatus"));
                    }

                    if(!status.Equals("done"))
                    {
                        return false;
                    }

                    conn.Close();
                }

                conn.Open();

                queryStr = "UPDATE agiledb.story SET storyStatus = ?status WHERE storyID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?status", "done");
                cmd.Parameters.AddWithValue("?id", story);

                cmd.ExecuteReader();
                conn.Close();

            }
            
            return true;
        }


        protected List<string> getSprintIDs()
        {
            List<string> sprintIDs = new List<string>();

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT sprintID FROM agiledb.projectsprints WHERE projectID=?projid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?projid", project);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                sprintIDs.Add(reader.GetString(reader.GetOrdinal("sprintID")));
            }

            conn.Close();

            return sprintIDs;
        }

        protected List<string> getStoryIDs(string sprintID)
        {
            List<string> storyIDs = new List<string>();

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT storiesID FROM agiledb.sprintstories WHERE sprintID=?sprintid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?sprintid", sprintID);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                storyIDs.Add(reader.GetString(reader.GetOrdinal("storiesID")));
            }

            conn.Close();

            return storyIDs;
        }

        protected void createSprintButton(object sender, EventArgs e)
        {
            string list = sprintStorieshf.Value;

            List<string> matched = new List<string>();
            int indexStart = 0, indexEnd = 0;
            bool exit = false;
            string startString = "data-id=\"";
            string endString = "\">";
            while (!exit)
            {
                indexStart = list.IndexOf(startString);
                indexEnd = list.IndexOf(endString);
                if (indexStart != -1 && indexEnd != -1)
                {
                    matched.Add(list.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length));
                    list = list.Substring(indexEnd + endString.Length);
                }
                else
                    exit = true;
            }

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "INSERT INTO agiledb.sprints (sprintLength) " +
                "VALUES(?length)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?length", SprintLength.SelectedValue);

            cmd.ExecuteReader();
            conn.Close();

            conn.Open();

            string sprintID = "";

            queryStr = "SELECT sprintID FROM agiledb.sprints ORDER BY sprintID DESC LIMIT 1";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                sprintID = reader.GetString(reader.GetOrdinal("sprintID"));
            }

            conn.Close();

            conn.Open();

            queryStr = "INSERT INTO agiledb.projectsprints (projectID, sprintID) " +
                "VALUES(?project, ?sprint)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?project", project);
            cmd.Parameters.AddWithValue("?sprint", sprintID);

            cmd.ExecuteReader();
            conn.Close();

            foreach (string id in matched)
            {
                string storyID = id.Replace("\" style=\"", "");

                conn.Open();

                queryStr = "INSERT INTO agiledb.sprintstories (sprintID, storiesID) " +
                    "VALUES(?sprint, ?story)";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?sprint", sprintID);
                cmd.Parameters.AddWithValue("?story", storyID);

                cmd.ExecuteReader();
                conn.Close();

                conn.Open();

                queryStr = "UPDATE agiledb.story SET storyStatus = ?status WHERE storyID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?status", "InProgress");
                cmd.Parameters.AddWithValue("?id", storyID);

                cmd.ExecuteReader();
                conn.Close();
            }

            Response.Redirect(Request.RawUrl);
        }


        private void getBacklog()
        {
            HtmlGenericControl list = new HtmlGenericControl("ul");
            list.Attributes.Add("class", "connectedSortable");
            list.Attributes.Add("id", "backlogSort");

            conn = SQL_Helpers.createConnection();
            conn.Open();

            List<string> backlogStories = new List<string>();

            queryStr = "SELECT storyID FROM agiledb.backlogstories WHERE backlogID=?id ORDER BY backlogPriority ASC";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", backlog);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                backlogStories.Add(reader.GetString(reader.GetOrdinal("storyID")));
            }

            conn.Close();

            backlogSize = backlogStories.Count;

            foreach (string storyID in backlogStories)
            {
                conn.Open();

                queryStr = "SELECT storyName FROM agiledb.story WHERE storyID=?id AND storyStatus=?status";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", storyID);
                cmd.Parameters.AddWithValue("?status", "toDo");

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    HtmlGenericControl item = new HtmlGenericControl("li");
                    item.Attributes.Add("class", "ui-state-default");
                    item.Attributes.Add("id", "storyInfo");
                    item.Attributes.Add("data-id", storyID);
                    item.InnerText = reader.GetString(reader.GetOrdinal("storyName"));

                    list.Controls.Add(item);
                }

                conn.Close();
            }

            backlogList.Controls.Add(list);
        }

        protected void createBacklogItem(object sender, EventArgs e)
        {
            if (editStory == "false")
            {
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "INSERT INTO agiledb.story (storyName, storyStatus, storyPoints, storyDetail) " +
                    "VALUES(?name, ?status, ?points, ?detail, ?story)";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?name", backlogItemName.Text);
                cmd.Parameters.AddWithValue("?status", "toDo");
                cmd.Parameters.AddWithValue("?points", backlogItemStoryPoints.SelectedValue);
                cmd.Parameters.AddWithValue("?detail", backlogItemDescription.Text);

                cmd.ExecuteReader();
                conn.Close();

                conn.Open();

                string storyID = "";

                queryStr = "SELECT storyID FROM agiledb.story ORDER BY storyID DESC LIMIT 1";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    storyID = reader.GetString(reader.GetOrdinal("storyID"));
                }

                conn.Close();
                conn.Open();

                queryStr = "INSERT INTO agiledb.backlogstories(backlogID, storyID, backlogPriority) VALUES(?backlog, ?story, ?priority)";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?backlog", backlog);
                cmd.Parameters.AddWithValue("?story", storyID);
                cmd.Parameters.AddWithValue("?priority", backlogSize);
                reader = cmd.ExecuteReader();

                conn.Close();

                backlogItemName.Text = "";
                backlogItemStatus.SelectedValue = "reset";
                backlogItemStoryPoints.SelectedValue = "reset";
                backlogItemDescription.Text = "";

                updateBacklogOrder();
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "UPDATE agiledb.story SET storyName = ?name, storyStatus = ?status, storyPoints = ?points, storyDetail = ?detail " +
                    "WHERE storyID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?name", backlogItemName.Text);
                cmd.Parameters.AddWithValue("?status", backlogItemStatus.SelectedValue);
                cmd.Parameters.AddWithValue("?points", backlogItemStoryPoints.SelectedValue);
                cmd.Parameters.AddWithValue("?detail", backlogItemDescription.Text);
                cmd.Parameters.AddWithValue("?id", editStoryID);

                cmd.ExecuteReader();
                conn.Close();

                Session["storyInfo"] = null;
                updateBacklogOrder();
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void viewBacklogItem(object sender, EventArgs e)
        {
            string storyID = backlogItemhf.Value;
            List<string> storyInfo = new List<string>();

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT * FROM agiledb.story WHERE storyID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", storyID);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                backlogItemName.Text = reader.GetString(reader.GetOrdinal("storyName"));
                backlogItemStatus.SelectedValue = reader.GetString(reader.GetOrdinal("storyStatus"));
                backlogItemStoryPoints.SelectedValue = reader.GetString(reader.GetOrdinal("storyPoints"));
                backlogItemDescription.Text = reader.GetString(reader.GetOrdinal("storyDetail"));


                if (reader.GetString(reader.GetOrdinal("storyStatus")).Equals("toDo"))
                {
                    registerBacklogButton.Visible = true;
                    backlogItemStoryPoints.Enabled = true;
                    backlogItemStatus.Enabled = false;
                    backlogItemName.Enabled = true;
                    backlogItemDescription.Enabled = true;
                }
                else
                {
                    registerBacklogButton.Visible = false;
                    backlogItemStoryPoints.Enabled = false;
                    backlogItemStatus.Enabled = true;
                    backlogItemName.Enabled = false;
                    backlogItemDescription.Enabled = false;
                }
            }

            conn.Close();
            
            Session["storyInfo"] = storyID;

            getTasks();

            updateBacklogOrder();
        }

        protected void destroySession(object sender, EventArgs e)
        {
            Session["storyInfo"] = null;
            updateBacklogOrder();
            Response.Redirect(Request.RawUrl);
        }

        private void updateBacklogOrder()
        {
            string list = backlogOrderhf.Value;

            List<string> matched = new List<string>();
            int indexStart = 0, indexEnd = 0;
            bool exit = false;
            string startString = "data-id=\"";
            string endString = "\">";
            while (!exit)
            {
                indexStart = list.IndexOf(startString);
                indexEnd = list.IndexOf(endString);
                if (indexStart != -1 && indexEnd != -1)
                {
                    matched.Add(list.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length));
                    list = list.Substring(indexEnd + endString.Length);
                }
                else
                    exit = true;
            }

            int count = 1;
            
            foreach (string id in matched)
            {
                string storyID = id.Replace("\" style=\"", "");

                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "UPDATE agiledb.backlogstories SET backlogPriority = ?priority WHERE storyID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?priority", count);
                cmd.Parameters.AddWithValue("?id", storyID);

                cmd.ExecuteReader();
                conn.Close();
                count++;
            }


        }

        protected void createTask(object sender, EventArgs e)
        {
            string storyID = backlogItemhf.Value;

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "INSERT INTO agiledb.task (taskName) VALUES(?name)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?name", newTask.Text);
            
            cmd.ExecuteReader();
            conn.Close();

            conn.Open();

            string taskID = "";

            queryStr = "SELECT taskID FROM agiledb.task ORDER BY taskID DESC LIMIT 1";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                taskID = reader.GetString(reader.GetOrdinal("taskID"));
            }

            conn.Close();
            conn.Open();

            queryStr = "INSERT INTO agiledb.storytasks(storyID, taskID) VALUES(?story, ?task)";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?story", storyID);
            cmd.Parameters.AddWithValue("?task", taskID);
            reader = cmd.ExecuteReader();

            conn.Close();

            newTask.Text = "";

            getTasks();
        }

        private void getTasks()
        {
            tasklist.InnerHtml = null;
            string storyID = backlogItemhf.Value;

            HtmlGenericControl list = new HtmlGenericControl("ul");
            list.Attributes.Add("id", "storyList");

            conn = SQL_Helpers.createConnection();
            conn.Open();

            List<string> storyTasks = new List<string>();

            queryStr = "SELECT taskID FROM agiledb.storytasks WHERE storyID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", storyID);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                storyTasks.Add(reader.GetString(reader.GetOrdinal("taskID")));
            }

            conn.Close();

            foreach (string taskID in storyTasks)
            {
                conn.Open();

                queryStr = "SELECT taskName FROM agiledb.task WHERE taskID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", taskID);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    HtmlGenericControl item = new HtmlGenericControl("li");
                    item.Attributes.Add("id", "taskInfo");
                    item.Attributes.Add("data-id", taskID);
                    item.InnerText = reader.GetString(reader.GetOrdinal("taskName"));

                    list.Controls.Add(item);
                }

                conn.Close();
            }

            tasklist.Controls.Add(list);
        }

        protected void startSprintButton(object sender, EventArgs e)
        {
            string sprintID = sprintList.SelectedValue;

            string date = DateTime.Now.ToString("d/M/yyyy");

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "UPDATE agiledb.sprints SET sprintStartDate = ?date, sprintStatus= ?status WHERE sprintID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?date", date);
            cmd.Parameters.AddWithValue("?status", "In Progress");
            cmd.Parameters.AddWithValue("?id", sprintID);

            cmd.ExecuteReader();
            conn.Close();

            conn.Open();

            queryStr = "UPDATE agiledb.project SET activeSprint = ?sprint WHERE projectID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?sprint", sprintID);
            cmd.Parameters.AddWithValue("?id", project);

            cmd.ExecuteReader();
            conn.Close();

            Response.Redirect(Request.RawUrl);
        }

        protected void endSprintButton(object sender, EventArgs e)
        {
           if(currentSprint.Equals("0"))
           {
                string alert = "No active sprint";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + alert + "');", true);
           }
           else
           {
                conn.Open();

                queryStr = "UPDATE agiledb.sprints SET sprintStatus = ?status WHERE sprintID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?status", "done");
                cmd.Parameters.AddWithValue("?id", currentSprint);

                cmd.ExecuteReader();

                conn.Close();

                conn.Open();

                queryStr = "UPDATE agiledb.project SET activeSprint = ?sprint WHERE projectID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?sprint", 0);
                cmd.Parameters.AddWithValue("?id", project);

                cmd.ExecuteReader();

                conn.Close();

                List<string> nonCompleted = getStoryIDs(currentSprint);
                string result = "";

                foreach(string story in nonCompleted)
                {
                    conn.Open();

                    queryStr = "SELECT storyStatus FROM agiledb.story WHERE storyID=?id";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?id", story);
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows && reader.Read())
                    {
                        result = reader.GetString(reader.GetOrdinal("storyStatus"));
                    }

                    conn.Close();

                    if(!result.Equals("done"))
                    {
                        conn.Open();

                        queryStr = "UPDATE agiledb.story SET storyStatus = ?status WHERE storyID=?id";

                        cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                        cmd.Parameters.AddWithValue("?status", "toDo");
                        cmd.Parameters.AddWithValue("?id", story);

                        cmd.ExecuteReader();
                        conn.Close();

                        List<string> tasks = new List<string>();

                        conn.Open();

                        queryStr = "SELECT taskID FROM agiledb.storytasks WHERE storyID=?id";

                        cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                        cmd.Parameters.AddWithValue("?id", story);

                        reader = cmd.ExecuteReader();

                        while (reader.HasRows && reader.Read())
                        {
                            tasks.Add(reader.GetString(reader.GetOrdinal("taskID")));
                        }

                        conn.Close();

                    }
                }

                Response.Redirect(Request.RawUrl);
            }
            
        }

        protected void viewTaskItem(object sender, EventArgs e)
        {
            string name ="", status = "";

            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT * FROM agiledb.task WHERE taskID=?id";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", taskIDhf.Value);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                name =  reader.GetString(reader.GetOrdinal("taskName"));
                status = reader.GetString(reader.GetOrdinal("taskStatus"));
            }

            taskName.Text = name;
            taskStatus.SelectedValue = status;

            getTasks();
        }

        protected void saveTaskItem(object sender, EventArgs e)
        {
            if(taskIDhf.Value.Equals("null"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert" + UniqueID, "alert('Please select a task');", true);
                getTasks();
            }
            else
            {
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "UPDATE agiledb.task SET taskName = ?name, taskStatus = ?status WHERE taskID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?name", taskName.Text);
                cmd.Parameters.AddWithValue("?status", taskStatus.SelectedValue);
                cmd.Parameters.AddWithValue("?id", taskIDhf.Value);
                reader = cmd.ExecuteReader();

                conn.Close();

                getTasks();
            }
        }
    }
}