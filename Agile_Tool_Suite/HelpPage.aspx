<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="HelpPage.aspx.cs" Inherits="Agile_Tool_Suite.HelpPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Help
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Help.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="titles">Agile Help</h2>

    <div class="panel-group" id="accordion">
      <div class="panel panel-default">
        <div class="panel-heading">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">
            Agile</a>
          </h4>
        </div>
        <div id="collapse1" class="panel-collapse collapse in">
          <div class="panel-body">
          <div>To ensure your project is being run in an Agile way you must follow the following principles:</div>
          
          
          <p class="text">   <br /> <br /> Our highest priority is to satisfy the customer
                through early and continuous delivery
                of valuable software. <br /> <br />

                Welcome changing requirements, even late in 
                development. Agile processes harness change for 
                the customer's competitive advantage. <br /> <br />

                Deliver working software frequently, from a 
                couple of weeks to a couple of months, with a 
                preference to the shorter timescale. <br /> <br />

                Business people and developers must work 
                together daily throughout the project. <br /> <br />

                Build projects around motivated individuals. 
                Give them the environment and support they need, 
                and trust them to get the job done. <br /> <br />

                The most efficient and effective method of 
                conveying information to and within a development 
                team is face-to-face conversation. <br /> <br />

                Working software is the primary measure of progress. <br /> <br />

                Agile processes promote sustainable development. 
                The sponsors, developers, and users should be able 
                to maintain a constant pace indefinitely. <br /> <br />

                Continuous attention to technical excellence 
                and good design enhances agility. <br /> <br />

                Simplicity--the art of maximizing the amount 
                of work not done--is essential. <br /> <br />

                The best architectures, requirements, and designs 
                emerge from self-organizing teams. <br /> <br />

                At regular intervals, the team reflects on how 
                to become more effective, then tunes and adjusts 
                its behavior accordingly.</p>  
        

            <div> For more information please visit:  <a href="http://agilemanifesto.org/ "> http://agilemanifesto.org/ </a> </div>
            </div>
        </div>
      </div>
      <div class="panel panel-default">
        <div class="panel-heading">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">
            Scrum</a>
          </h4>
        </div>
        <div id="collapse2" class="panel-collapse collapse">
          <div class="panel-body">

              <div>For a complete guide to scrum please read : <a href="https://www.scrumguides.org/docs/scrumguide/v2017/2017-Scrum-Guide-US.pdf">https://www.scrumguides.org/docs/scrumguide/v2017/2017-Scrum-Guide-US.pdf</a></div>

              <br />

              <div>Here are a few helpful buzz words to help you use the program: </div>

              <p class="text"> 
                  <br /> <br />

                  Scrum has self-organizing teams: there are three main roles which are the Product Owner, 
                  Scrum Master and Development Team(s).

                  <br /> <br />

                  Product Owner: the role in Scrum accountable for maximizing the value of a product, 
                  primarily by incrementally managing and expressing business and functional expectations
                  for a product to the Development Team(s).

                  <br /> <br />

                  Scrum Master: the role within a Scrum Team accountable for guiding, coaching, teaching
                  and assisting a Scrum Team and its environments in a proper understanding and use of Scrum.

                  <br /> <br />

                  Development Team: the role within a Scrum Team accountable for managing, organizing and doing 
                  all development work required to create a releasable Increment of product every Sprint.

                  <br /> <br />

                  Product Backlog: an ordered list of the work to be done in order to create, maintain 
                  and sustain a product. Managed by the Product Owner.

                  <br /> <br />

                  Sprint: Time-boxed event of 30 days, or less, that serves as a container for the 
                  other Scrum events and activities. Sprints are done consecutively, without intermediate gaps.

                  <br /> <br />

                  Stories: A short expression of the purpose of a Sprint, often a business problem that is addressed. 
                  
                  <br /> <br />

                  Story Tasks: The steps that need to be taken in order to solve the Story problem and achieve its goal.

                  <br /> <br />

                  Story Points: A Story Point is a subjective unit of estimation used by Agile teams to estimate
                  Stories. Story points represent the amount of effort required to implement a user story.

                  <br /> <br />

                  Scrum Board: A physical board to visualize information for and by the Scrum Team, 
                  often used to manage Sprint Backlog. Scrum boards are an optional implementation 
                  within Scrum to make information visible. 
                  
                  <br /> <br />

                  Burn-down Chart: a chart which shows the amount of work which is thought to remain 
                  in a backlog. Time is shown on the horizontal axis and work remaining on the vertical 
                  axis. As time progresses and items are drawn from the backlog and completed, a plot line 
                  showing work remaining may be expected to fall. The amount of work may be assessed in any 
                  of several ways such as user story points or task hours. 

                  <br /> <br />

              </p>

              <div> Credit: <a href="https://www.scrum.org/resources/scrum-glossary"> https://www.scrum.org/resources/scrum-glossary </a> </div>
          </div>
        </div>
      </div>
      <div class="panel panel-default">
        <div class="panel-heading">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">
            Kanban</a>
          </h4>
        </div>
        <div id="collapse3" class="panel-collapse collapse">
          <div class="panel-body">

              <div>For a complete guide to Kanban please read : <a href="https://www.scrumguides.org/docs/scrumguide/v2017/2017-Scrum-Guide-US.pdf">https://www.scrumguides.org/docs/scrumguide/v2017/2017-Scrum-Guide-US.pdf</a></div>

              <br />

              <div>To help you get to grips with Kanban here are the basic things to know:</div>
            
              <p class="text">
    
                  <br /> <br />
                  
                  Board: The term Kanban board refers to one entire workflow - a set of lanes that together, 
                  represent a process. Teams might choose to use one shared Kanban board to manage their work, 
                  or they might use multiple connected boards to optimize each board for a specific workflow 
                  (for example, a marketing team might have one board to manage content creation, while another 
                  manages campaigns).

                  <br /> <br />

                  Swimlanes: The stages that your work has to go through before being classed as 'complete'. 

                  <br /> <br />


              </p>
              


            <div> Credit: <a href="https://leankit.com/learn/kanban/kanban-glossary/"> https://leankit.com/learn/kanban/kanban-glossary/ </a> </div>
          </div>
        </div>
      </div>


    </div>
</asp:Content>
