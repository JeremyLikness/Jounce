# Dynamic XAP Loading

The simplest way to dynamically load a module in Jounce is to bind a view to the XAP file it lives in (see: [Binding Views to Xap Files](Binding-Views-to-Xap-Files)) and then navigate to the view.

Sometimes you require more control and may need to explicitly load the XAP file. 

Use the deployment service for this: 

{code:c#}
[Import](Import)
public IDeploymentService Deployment { get; set; }
{code:c#}

The service provides two methods: one to load a XAP file, and another that will load the XAP file and then call back with an exception if an exception occurred. Calling the service is straightforward, as demonstrated in the calculator quickstart:

{code:c#}
Deployment.RequestXap("DyanmicXapCalculatorAdvanced.xap");
{code:c#}

If you have any code that should run once a module is loaded, read [Bootstrapping Modules](Bootstrapping-Modules).