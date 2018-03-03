using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Agile_Tool_Suite
{
    public partial class Master : System.Web.UI.MasterPage
    {
        string user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);

            if (IsPostBack)
            {
                Session["project"] = project.SelectedValue.ToString();
                string test = project.SelectedValue.ToString();
            }

            if (user == null)
            {
                Response.BufferOutput = true;
                Response.Redirect("Home.aspx", false);
            }
            else
            {
                Load_Page();
            }
        }

        protected void Load_Page()
        {
            userLabel.Text = SQL_Helpers.getUserName(user);

            List<ListItem>  projectItems = getProjects();

            project.Items.Clear();

            project.Items.AddRange(projectItems.ToArray());

            project.SelectedValue = (string)(Session["project"]);
        }

        List<ListItem> getProjects()
        {
            List<ListItem> projects = new List<ListItem>();
            List<string> projectIDs = new List<string>();

            MySql.Data.MySqlClient.MySqlConnection conn = SQL_Helpers.createConnection();
            conn.Open();

            string queryStr = "SELECT projectID FROM agiledb.userprojects WHERE userID=?id";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", user);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                projectIDs.Add(reader.GetString(reader.GetOrdinal("projectID")));
            }

            conn.Close();

            foreach(string proj in projectIDs)
            {
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "SELECT projectName FROM agiledb.project WHERE projectID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", proj);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    projects.Add(new ListItem(reader.GetString(reader.GetOrdinal("projectName")), proj.ToString()));
                }

                projects.Add(new ListItem("Test", "19"));

                if (Session["project"] == null)
                {
                    Session["project"] = projectIDs[0];
                }

                string test = (string)(Session["project"]);

                conn.Close();
            }

            return projects;
        }

        protected void LogoutMethod(object sender, EventArgs e)
        {
            Session["uname"] = null;
            Session["project"] = null;
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Home.aspx", false);
        }
    }
}