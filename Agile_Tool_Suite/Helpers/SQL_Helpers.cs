using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agile_Tool_Suite
{
    public class SQL_Helpers
    {
        public static MySql.Data.MySqlClient.MySqlConnection createConnection()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connString);

            return conn;

        }

        public static String getUserName(string user)
        {
            String name = "";

            MySql.Data.MySqlClient.MySqlConnection conn = createConnection();
            conn.Open();

            string queryStr = "SELECT firstName, lastName FROM agiledb.users WHERE userID=?id";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", user);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                name = reader.GetString(reader.GetOrdinal("firstName")) + " "
                    + reader.GetString(reader.GetOrdinal("lastName"));
            }

            conn.Close();

            return name;
        }

        public static String getEmail(string user)
        {
            String email = "";

            MySql.Data.MySqlClient.MySqlConnection conn = createConnection();
            conn.Open();

            string queryStr = "SELECT email FROM agiledb.users WHERE userID=?id";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", user);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                email = reader.GetString(reader.GetOrdinal("email"));
            }

            conn.Close();

            return email;
        }

        public static void checkStatus(string project, string sprint)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySql.Data.MySqlClient.MySqlCommand cmd;
            MySql.Data.MySqlClient.MySqlDataReader reader;
            string queryStr = "";

            List<string> storyIDs = getSprintStories(sprint);
            List<string> taskIDs = new List<string>();

            bool sprintCompleted = true;
            bool storyCompleted = true;
            
            foreach (string story in storyIDs)
            {
                taskIDs = getStoryTasks(story);

                foreach(string task in taskIDs)
                {
                    string response = "";

                    conn = createConnection();
                    conn.Open();

                    queryStr = "SELECT taskStatus FROM agiledb.task WHERE taskID=?taskid";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?taskid", task);

                    reader = cmd.ExecuteReader();

                    while (reader.HasRows && reader.Read())
                    {
                        response = reader.GetString(reader.GetOrdinal("taskStatus"));
                    }

                    conn.Close();

                    if(response.Equals("done"))
                    {
                        storyCompleted = true;
                    }
                    else
                    {
                        storyCompleted = false;
                    }
                }

                if(storyCompleted == true)
                {
                    conn = SQL_Helpers.createConnection();
                    conn.Open();

                    queryStr = "UPDATE agiledb.story SET storyStatus = ?status WHERE storyID=?id";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?status", "done");
                    cmd.Parameters.AddWithValue("?id", story);

                    cmd.ExecuteReader();
                    conn.Close();
                }
                else
                {
                    sprintCompleted = false;
                }

            }

            if(sprintCompleted == true)
            {
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "UPDATE agiledb.sprints SET sprintStatus = ?status WHERE sprintID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?status", "done");
                cmd.Parameters.AddWithValue("?id", sprint);

                cmd.ExecuteReader();
                conn.Close();
            }
        }

        public static List<string> getSprintStories(string sprint)
        {
            List<string> storyIDs = new List<string>();

            MySql.Data.MySqlClient.MySqlConnection conn = SQL_Helpers.createConnection();
            conn.Open();

            string queryStr = "SELECT storiesID FROM agiledb.sprintstories WHERE sprintID=?sprintid";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?sprintid", sprint);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                storyIDs.Add(reader.GetString(reader.GetOrdinal("storiesID")));
            }

            conn.Close();

            return storyIDs;
        }

        public static List<string> getStoryTasks(string storyID)
        {
            List<string> tempTaskIDs = new List<string>();

            MySql.Data.MySqlClient.MySqlConnection conn = SQL_Helpers.createConnection();
            conn.Open();

            string queryStr = "SELECT taskID FROM agiledb.storytasks WHERE storyID=?storyid";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?storyid", storyID);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                tempTaskIDs.Add(reader.GetString(reader.GetOrdinal("taskID")));
            }

            conn.Close();

            return tempTaskIDs;
        }

    }
}