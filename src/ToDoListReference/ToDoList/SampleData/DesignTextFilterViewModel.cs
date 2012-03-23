using ToDoList.Contracts;

namespace ToDoList.SampleData
{
#if DEBUG
    public class DesignTextFilterViewModel : ITextFilterViewModel
    {
        public string TextFilter
        {
            get { return "Designer Search"; }
        }
    }
#endif
}