using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace Agile_Tool_Suite
{
    public partial class CompletedWork : System.Web.UI.Page
    {
        //Session information
        private string user;
        private string project;

        //Variables
        private string backlog;
        private string sprintLength;
        private string projectStartDate;
        private string projectEndDate;
        private int totalPoints;

        //SQL information
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string queryStr;

        private int backlogSize;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);
            project = (string)(Session["project"]);

            getBacklogInformation();
            getBacklog();
            getBurnDown();
        }

        private void getBacklogInformation()
        {
            conn = SQL_Helpers.createConnection();
            conn.Open();

            queryStr = "SELECT backlogID, sprintLength, projectStartDate, projectEndDate FROM agiledb.project WHERE projectID=?projid";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?projid", project);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                backlog = reader.GetString(reader.GetOrdinal("backlogID"));
                sprintLength = reader.GetString(reader.GetOrdinal("sprintLength"));
                projectStartDate = reader.GetString(reader.GetOrdinal("projectStartDate"));
                projectEndDate = reader.GetString(reader.GetOrdinal("projectEndDate"));
            }
        }

        private void getBurnDown()
        {
            DateTime startDate = Convert.ToDateTime(projectStartDate);
            DateTime endDate = Convert.ToDateTime(projectEndDate);

            DateTime start =  startDate.Date.AddDays(- ((((int)startDate.DayOfWeek) + 6) % 7));
            DateTime end = endDate.Date.AddDays(-((((int)endDate.DayOfWeek) + 6) % 7));

            int days = (int)(end - start).TotalDays;
            int weeks = (days / 7) + 1;

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("X", typeof(int)), new DataColumn("Y", typeof(int)) });
            dt.Rows.Add(0, totalPoints);
            dt.Rows.Add(weeks, 0);

            BurndownChart.ChartAreas.Add("chtArea");
            BurndownChart.ChartAreas[0].AxisX.Title = "Sprints";
            BurndownChart.ChartAreas[0].AxisX.Minimum = 0;
            BurndownChart.ChartAreas[0].AxisX.Maximum = weeks;
            BurndownChart.ChartAreas[0].AxisY.Title = "Story Points";

            BurndownChart.Legends.Add("Predicted");
            BurndownChart.Series.Add("Predicted");
            BurndownChart.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            BurndownChart.Series[0].Points.DataBindXY(dt.DefaultView, "X", dt.DefaultView, "Y");

            BurndownChart.Series[0].BorderWidth = 3;
            BurndownChart.Series[0].Color = Color.Green;

            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("X", typeof(int)), new DataColumn("Y", typeof(int)) });
            dt.Rows.Add(0, totalPoints);

            List<int> storiesCompleted = getSprints();
            int count = 1;

            foreach (int points in storiesCompleted)
            {
                totalPoints = totalPoints - points;
                dt.Rows.Add(count, totalPoints);
                count++;
            }
            
            BurndownChart.Legends.Add("Actual");
            BurndownChart.Series.Add("Actual");
            BurndownChart.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            BurndownChart.Series[1].Points.DataBindXY(dt.DefaultView, "X", dt.DefaultView, "Y");

            BurndownChart.Series[1].BorderWidth = 3;
            BurndownChart.Series[1].Color = Color.Red;
        }

        private List<int> getSprints()
        {
            List<int> sprints = new List<int>();

            List<string> sprintIDs = getSprintIDs();

            foreach (string sprint in sprintIDs)
            {
                string status = "";
                string points = "";

                conn.Open();

                queryStr = "SELECT sprintStatus, pointsCompleted FROM agiledb.sprints WHERE sprintID=?sprintid";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?sprintid", sprint);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    status = reader.GetString(reader.GetOrdinal("sprintStatus"));
                    points = reader.GetString(reader.GetOrdinal("pointsCompleted"));
                }

                conn.Close();

                if (status.Equals("done"))
                {
                    sprints.Add(Convert.ToInt32(points));
                }
            }

            return sprints;
        }

        private List<string> getSprintIDs()
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
                cmd.Parameters.AddWithValue("?status", "done");

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

                conn.Open();

                queryStr = "SELECT storyPoints FROM agiledb.story WHERE storyID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", storyID);
                cmd.Parameters.AddWithValue("?status", "done");

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    totalPoints = totalPoints + Convert.ToInt32(reader.GetString(reader.GetOrdinal("storyPoints")));
                }

                conn.Close();
            }

            backlogList.Controls.Add(list);
        }
    }
}