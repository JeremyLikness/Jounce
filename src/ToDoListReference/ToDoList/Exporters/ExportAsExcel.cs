using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices.Automation;
using ToDoList.Contracts;

namespace ToDoList.Exporters
{
    [Export(typeof(IExportAs))]
    public class ExportAsExcel : IExportAs 
    {
        const string EXCEL = "Excel.Application";

        public void Export(IEnumerable<IToDoItem> items)
        {
            dynamic excel;

            try
            {
                excel = AutomationFactory.GetObject(EXCEL);
            }
            catch
            {
                excel = AutomationFactory.CreateObject(EXCEL);
            }

            excel.Visible = true;
            var workbook = excel.Workbooks;
            workbook.Add();

            var sheet = excel.ActiveSheet;

            var row = 1;
            foreach (var item in items)
            {
                var cell = sheet.Cells(row, 1);
                cell.Value = item.Title;
                cell.ColumnWidth = 25;

                cell = sheet.Cells(row, 2);
                cell.Value = item.Description;
                cell.ColumnWidth = 50;

                cell = sheet.Cells(row, 3);
                cell.Value = item.DueDate;
                cell.ColumnWidth = 20;

                cell = sheet.Cells(row, 4);
                cell.Value = item.IsComplete;
                cell.ColumnWidth = 25;

                cell = sheet.Cells(row, 5);
                cell.Value = item.CompletedDate;
                cell.ColumnWidth = 25;

                row++;
            }
        }
    }
}