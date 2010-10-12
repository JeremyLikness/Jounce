using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Core.Application;
using Jounce.Framework;

namespace RegionManagementDynamic
{
    /// <summary>
    ///     This is an optional class. If you implement IModuleInitializer and export it 
    ///     in your module, Jounce will automatically call this once the XAP is loaded. It
    ///     is useful for performing whatever set up your module may need, and if you
    ///     don't need it, you don't have to include it.
    /// </summary>
    [Export(typeof(IModuleInitializer))]
    public class ModuleInit : IModuleInitializer 
    {
        public bool Initialized { get; set; }
        
        public void Initialize()
        {
            JounceHelper.ExecuteOnUI(()=>MessageBox.Show("Dynamic module loaded."));
            Initialized = true;
        }
    }
}