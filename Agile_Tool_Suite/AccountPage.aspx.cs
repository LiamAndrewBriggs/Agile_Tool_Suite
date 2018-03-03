using System;
using System.Text.RegularExpressions;

namespace Agile_Tool_Suite
{
    public partial class AccountPage : System.Web.UI.Page
    {
        private string firstName = " ";
        private string surname = " ";
        private string email = " ";

        private string user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);
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
            string name = SQL_Helpers.getUserName(user);

            firstName = name.Substring(0, name.IndexOf(' '));
            surname = Regex.Match(name, "[^ ]* (.*)").Groups[1].Value;
            email = SQL_Helpers.getEmail(user);

            firstnameTextBox.Text = firstName;
            surnameTextBox.Text = surname;
            emailTextBox.Text = email;
            originalPasswordTextBox.Text = "";
            passwordTextBox.Text = "";
            confirmPasswordTextBox.Text = "";
        }

        protected void registerEventMethod(object sender, EventArgs e)
        {
            bool methodStatus = true;
            emailError.Visible = false;
            passwordError.Visible = false;
            originalpasswordError.Visible = false;

            string queryStr = " ";
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySql.Data.MySqlClient.MySqlCommand cmd;

            if (firstnameTextBox.Enabled == true)
            {
                if (!InputValidation.ValidateName(firstnameTextBox.Text))
                {
                    methodStatus = false;
                }
                if (!InputValidation.ValidateName(surnameTextBox.Text))
                {
                    methodStatus = false;
                }

                if (methodStatus)
                {
                    conn = SQL_Helpers.createConnection();
                    conn.Open();

                    queryStr = "UPDATE agiledb.users SET firstName = ?firstname,  lastName = ?lastname WHERE userID=?id";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?firstname", firstnameTextBox.Text);
                    cmd.Parameters.AddWithValue("?lastname", surnameTextBox.Text);
                    cmd.Parameters.AddWithValue("?id", user);

                    cmd.ExecuteReader();
                    conn.Close();

                    firstnameTextBox.Enabled = false;
                    surnameTextBox.Enabled = false;
                }

                methodStatus = true;
            }
            if (emailTextBox.Enabled == true)
            {
                if (!InputValidation.ValidateEmail(emailTextBox.Text))
                {
                    methodStatus = false;
                }
                if (!UserValidation.EmailCheck(emailTextBox.Text))
                {
                    methodStatus = false;
                    emailError.Visible = true;
                }

                if (methodStatus)
                {
                    conn = SQL_Helpers.createConnection();
                    conn.Open();

                    queryStr = "UPDATE agiledb.Users SET email = ?email WHERE userID=?id";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?email", firstnameTextBox.Text);
                    cmd.Parameters.AddWithValue("?id", user);

                    cmd.ExecuteReader();
                    conn.Close();

                    emailTextBox.Enabled = false;
                }

                methodStatus = true;
            }
            if (passwordTextBox.Enabled == true)
            {
                String saltHash = " ";

                conn = SQL_Helpers.createConnection();
                conn.Open();

                queryStr = "SELECT slowHashSalt FROM agiledb.Users WHERE userID=?id";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", user);

                MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    saltHash = reader.GetString(reader.GetOrdinal("slowHashSalt"));
                }

                bool validUser = PasswordStorage.VerifyPassword(originalPasswordTextBox.Text, saltHash);

                conn.Close();

                if (validUser)
                {
                    if (!InputValidation.ValidateUserInput(passwordTextBox.Text))
                    {
                        methodStatus = false;
                    }
                    if (confirmPasswordTextBox.Text != passwordTextBox.Text)
                    {
                        methodStatus = false;
                        passwordError.Visible = true;
                    }

                    if (methodStatus)
                    {
                        conn = SQL_Helpers.createConnection();
                        conn.Open();

                        queryStr = "UPDATE agiledb.Users SET slowHashSalt = ?slowHashSalt WHERE userID=?id";

                        cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                        cmd.Parameters.AddWithValue("?id", firstnameTextBox.Text);
                        string saltHashReturned = PasswordStorage.CreateHash(passwordTextBox.Text);
                        cmd.Parameters.AddWithValue("?slowhashsalt", saltHashReturned);

                        cmd.ExecuteReader();
                        conn.Close();

                        originalPasswordTextBox.Enabled = false;
                        passwordTextBox.Enabled = false;
                        confirmPasswordTextBox.Enabled = false;
                    }
                }
                else
                {
                    originalpasswordError.Visible = true;
                }

            }
        }

        protected void nameEdit(object sender, EventArgs e)
        {
            firstnameTextBox.Enabled = true;
            surnameTextBox.Enabled = true;
        }

        protected void emailEdit(object sender, EventArgs e)
        {
            emailTextBox.Enabled = true;
        }

        protected void passwordEdit(object sender, EventArgs e)
        {
            originalPasswordTextBox.Enabled = true;
            passwordTextBox.Enabled = true;
            confirmPasswordTextBox.Enabled = true;
        }
    }
}