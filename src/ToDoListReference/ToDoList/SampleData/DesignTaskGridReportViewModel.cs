using System;
using System.Collections.Generic;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.SampleData
{
#if DEBUG
    public class DesignTaskGridReportViewModel : ITaskGridReportViewModel 
    {
        public DesignTaskGridReportViewModel()
        {
            UserName = "Design User";
            TotalPages = 5;
            Page = 3;
            var tasks = new List<IToDoItem>();
            var random = new Random();
            for (var x = 0; x < 48; x++)
            {
                var task = new ToDoItemOverride();
                if (random.NextDouble() < 0.5)
                {
                    task.IsComplete = false;
                    task.DueDate = DateTime.Now
                        .AddHours(-13 + 48 * random.NextDouble());
                }
                else
                {
                    task.IsComplete = true;
                    task.CompletedDate = DateTime.Now
                        .AddHours(-72*random.NextDouble());
                }
                task.Title = "Task " + (x + 1);
                task.Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum eget ante augue, in interdum arcu. Suspendisse potenti. Ut aliquam tellus ac arcu porttitor id laoreet neque imperdiet. Aliquam imperdiet massa in ante malesuada eleifend. Aenean semper elementum placerat. In hac habitasse platea dictumst. Nulla ut suscipit nulla. Quisque quam nibh, vehicula vitae viverra in, dapibus a metus. Fusce ut porttitor odio. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae;";
                tasks.Add(task);
            }
            Tasks = tasks;
        }

        public DateTime PrintDate { get; set; }

        public string UserName { get; set; }

        public int TotalPages { get; set; }

        public int Page { get; set; }

        public IEnumerable<IToDoItem> Tasks { get; set; }
    }
#endif
}