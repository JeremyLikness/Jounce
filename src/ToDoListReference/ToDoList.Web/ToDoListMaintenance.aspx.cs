using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.UI;
using ToDoList.Contracts;
using ToDoList.Model;
using ToDoList.SterlingDatabase;

namespace ToDoList.Web
{
    public partial class ToDoListMaintenance : Page
    {
        [Import]
        public IRepository Repository { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Click += Button1_Click;

            if (Repository == null)
            {
                MEFBootstrapper.SatisfyImports(this);
            }

            GridView1.DataSource = new List<IToDoItem>(Repository.Query());
            GridView1.DataBind();
        }

        private class ToDoItemSample : ToDoItem
        {
            public void MarkCompleteNoMessage()
            {
                IsComplete = true;
                CompletedDate = DateTime.Now;
            }
        }

        void Button1_Click(object sender, EventArgs e)
        {
            var random = new Random();
            for(var x = 0; x < 10; x++)
            {
                var offset = x - 5;
                var todo = new AuditableToDoItem
                                {
                                    Description = 
                                    string.Format("Sample task {0}", 
                                    x + 1),
                                    DueDate = DateTime.Now
                                        .AddDays(offset),
                                    Title = string.Format("Todo Task {0}", 
                                    x + 1)
                                };
                if (random.NextDouble() < 0.3)
                {
                    todo.IsComplete = true;
                    todo.CompletedDate = DateTime.Now;
                }
                Repository.Save(todo);
            }
            Response.Redirect("ToDoListMaintenance.aspx");
        }
    }
}