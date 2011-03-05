namespace Jounce.Core.Event
{
    /// <summary>
    ///     Helps encapsulate actions taken against entities
    /// </summary>
    public enum EntityCommand : short
    {
        Add,
        Update,
        Delete,
        Select,
        Focus,
        Query
    }
}