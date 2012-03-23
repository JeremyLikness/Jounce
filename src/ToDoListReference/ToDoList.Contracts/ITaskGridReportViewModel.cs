using System;
using System.Collections.Generic;

namespace ToDoList.Contracts
{
    public interface ITaskGridReportViewModel
    {
        DateTime PrintDate { get; set; }
        string UserName { get; set; }
        int TotalPages { get; set; }
        int Page { get; set; }
        IEnumerable<IToDoItem> Tasks { get; set; }       
    }
}