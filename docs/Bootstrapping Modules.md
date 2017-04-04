# Bootstrapping Modules

If you have any code that should run once a module is loaded, simply implement and export the module initializer interface. This works for the main XAP file as well, and will be called when the application is initializing.

Here is the interface:

{code:c#}
public interface IModuleInitializer
{
    bool Initialized { get; }
    void Initialize();
}
{code:c#}

Jounce will automatically call any exported class that implements this interface and has _Initialized_ set to false any time a XAP file is loaded. Here is an example that shows a message once the XAP has been loaded:

{code:c#}
[Export(typeof(IModuleInitializer))](Export(typeof(IModuleInitializer)))
public class ModuleInit : IModuleInitializer 
{
    public bool Initialized { get; set; }
        
    public void Initialize()
    {
        JounceHelper.ExecuteOnUI(()=>MessageBox.Show("Dynamic module loaded."));
        Initialized = true;
    }
}
{code:c#}

Note if you don't set initialized to true, the code will get called anytime a new XAP file is dynamically loaded.