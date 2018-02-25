using System;

namespace Agile_Tool_Suite
{
    public partial class NewUser : System.Web.UI.Page
    {
        string user;
        string name;

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
                addUserToProject();
            }
        }

        protected void addUserToProject()
        {
            userLabel.Text = "Welcome! " + SQL_Helpers.getUserName(user);
        }

        protected void Search_Project(object sender, EventArgs e)
        {

        }

        protected void Create_Project(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("Create_Project.aspx", false);
        }

        protected void LogoutMethod(object sender, EventArgs e)
        {
            Session["uname"] = null;
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Home.aspx", false);
        }
    }
}