using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Agile_Tool_Suite
{
    public partial class Home : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        String queryStr;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LogInMethod(object sender, EventArgs e)
        {
            if (checkAgainstWhiteList(usernameTextBox.Text) &&
                checkAgainstWhiteList(passwordTextBox.Text))
            {
                LogInWithHash();
            }
            else
            {
                passwordTextBox.Text = "Does not pass White List test";
            }

        }

        private void LogInWithHash()
        {
            List<String> saltHashList = null;
            List<String> userList = null;

            try
            {
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "SELECT slowHashSalt, userID, firstName, lastName FROM AgileDB.Users WHERE username=?uname";

                cmd =  new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?uname", usernameTextBox.Text);

                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    if(saltHashList == null)
                    {
                        saltHashList = new List<string>();
                        userList = new List<string>();
                    }

                    String saltHashes = reader.GetString(reader.GetOrdinal("slowHashSalt"));
                    saltHashList.Add(saltHashes);

                    String userID = reader.GetString(reader.GetOrdinal("userID"));

                    userList.Add(userID);
                }
                reader.Close();

                if (saltHashList != null)
                {
                    for (int i = 0; i <saltHashList.Count; i++)
                    {
                        queryStr = "";
                        bool validUser = PasswordStorage.VerifyPassword(passwordTextBox.Text, saltHashList[i]);
                        if(validUser == true)
                        {
                            Session["uname"] = userList[i];
                            Response.BufferOutput = true;
                            Response.Redirect("LoggedIn.aspx", false);
                        }
                        else
                        {
                            passwordTextBox.Text = "Wrong Password";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                passwordTextBox.Text = e.ToString();
            }
        }

        private bool checkAgainstWhiteList(String input)
        {
            var regExpression = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]*$");

            if (regExpression.IsMatch(input))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}