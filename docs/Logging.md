# Logging

Jounce ships with a default logger that simply writes to the attached debugger. 

For more sophisticated debugging (for example, displaying events in the application or sending them to the service via a service for logging) you can implement our own logger. Simply implement the interface ILogger and export the interface. Be sure to only do this once - Jounce currently supports only one non-default logger (but you can certainly import other loggers as you like in your own implementation).

{code:c#}
[Export(typeof(ILogger))](Export(typeof(ILogger)))
public MyLogger : ILogger
{
}
{code:c#}

The logger can have a minimum severity set (the typical Verbose, Information, Warning, Error, and Critical are canned). It should accept a log with a message, an Exception, and a string with arguments to perform string formatting. The "source" is typically the type that called the log (i.e. GetType().FullName) and this is how Jounce logs messages, however, you are welcome to use your own convention.