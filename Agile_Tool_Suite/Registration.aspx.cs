using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Agile_Tool_Suite
{
    public partial class Registration : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string queryStr;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void registerEventMethod(object sender, EventArgs e)
        {
            RegisterUserWithSlowHash();
        }

        private void RegisterUserWithSlowHash()
        {
            bool methodStatus = true;
            emailError.Visible = false;
            usernameError.Visible = false;
            passwordError.Visible = false;

            if (!InputValidation.ValidateName(firstnameTextBox.Text))
            {
                methodStatus = false;
            }
            if (!InputValidation.ValidateName(surnameTextBox.Text))
            {
                methodStatus = false;
            }
            if (!InputValidation.ValidateUserInput(usernameTextBox.Text))
            {
                methodStatus = false;
            }
            if (!InputValidation.ValidateUserInput(passwordTextBox.Text))
            {
                methodStatus = false;
            }
            if (!InputValidation.ValidateEmail(emailTextBox.Text))
            {
                methodStatus = false;
            }
            if(!UserValidation.EmailCheck(emailTextBox.Text))
            {
                methodStatus = false;
                emailError.Visible = true;
            }
            if(!UserValidation.UsernameCheck(usernameTextBox.Text))
            {
                methodStatus = false;
                usernameError.Visible = true;
            }
            if(confirmPasswordTextBox.Text != passwordTextBox.Text)
            {
                methodStatus = false;
                passwordError.Visible = true;
            }

            if (methodStatus == true)
            {
                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "INSERT INTO agiledb.Users (firstName, lastName, email, userName, slowHashSalt) " +
                    "VALUES(?firstname, ?lastname, ?email, ?username, ?slowhashsalt)";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?firstname", firstnameTextBox.Text);
                cmd.Parameters.AddWithValue("?lastname", surnameTextBox.Text);
                cmd.Parameters.AddWithValue("?email", emailTextBox.Text);
                cmd.Parameters.AddWithValue("?username", usernameTextBox.Text);

                string saltHashReturned = PasswordStorage.CreateHash(passwordTextBox.Text);
                cmd.Parameters.AddWithValue("?slowhashsalt", saltHashReturned);

                cmd.ExecuteReader();
                conn.Close();

                conn.Open();

                queryStr = "SELECT userID FROM agiledb.users ORDER BY userID DESC LIMIT 1";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    Session["uname"] = reader.GetString(reader.GetOrdinal("userID"));
                }

                conn.Close();

                Response.BufferOutput = true;
                Response.Redirect("LoggedIn.aspx", false);
            }

        }

    }
}