using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
            HtmlGenericControl panel = new HtmlGenericControl("div");
            panel.Attributes.Add("class", "panel panel-default");

            HtmlGenericControl panelHeading = new HtmlGenericControl("div");
            panelHeading.Attributes.Add("class", "panel-heading");

            HtmlGenericControl heading = new HtmlGenericControl("h4");
            heading.Attributes.Add("class", "panel-title");

            HtmlGenericControl dataToggle = new HtmlGenericControl("a");
            dataToggle.Attributes.Add("data-toggle", "collapse");
            dataToggle.Attributes.Add("data-parent", "#sprintAccordian");
            dataToggle.Attributes.Add("href", "#collapse1");
            dataToggle.InnerText = "Sprint 1";

            heading.Controls.Add(dataToggle);
            panelHeading.Controls.Add(heading);
            panel.Controls.Add(panelHeading);


            HtmlGenericControl content = new HtmlGenericControl("div");
            content.Attributes.Add("id", "collapse1");
            content.Attributes.Add("class", "panel-collapse collapse in");

            HtmlGenericControl contentBody = new HtmlGenericControl("div");
            contentBody.Attributes.Add("class", "panel-body");
            contentBody.InnerText = "Sprint 1";

            content.Controls.Add(contentBody);
            panel.Controls.Add(content);
            sprintAccordian.Controls.Add(panel);
        }

        protected void createSprintButton(object sender, EventArgs e)
        {

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

                queryStr = "SELECT storyName FROM agiledb.story WHERE storyID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", storyID);

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
                    "VALUES(?name, ?status, ?points, ?detail)";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?name", backlogItemName.Text);
                cmd.Parameters.AddWithValue("?status", backlogItemStatus.SelectedValue);
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
            }

            conn.Close();
            
            Session["storyInfo"] = storyID;

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
    }
}