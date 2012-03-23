using System;
using System.Threading;
using System.Windows.Markup;

namespace ToDoList.Behaviors
{
    public class LocalizeExtension : MarkupExtension
    {
        public string Resource { get; set; }

        public bool AsLabel { get; set; }

        public string Default { get; set; }
        
        public override string ToString()
        {
            return Default ?? string.Empty;
        }
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {            
            var value = Resources.ResourceManager
                            .GetString(Resource, Thread.CurrentThread.CurrentUICulture)
                        ?? Default
                        ?? string.Empty;
            return AsLabel
                        ? string.Format("{0}:", value.Trim())
                        : value.Trim();
        }
    }
}