using System.Windows;
using System.Windows.Controls;

namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Fluent region manager
    /// </summary>
    public interface IFluentRegionManager
    {
        void ExportViewToRegion(string viewName, string regionTag);
        void ExportViewToRegion<T>(string regionTag) where T : UserControl;
        void RegisterRegion(UIElement region, string regionTag);
    }
}