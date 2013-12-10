namespace Jounce.Core.Event
{
    /// <summary>
    ///     Helps encapsulate actions taken against entities
    /// </summary>
    public enum EntityCommand : short
    {
        /// <summary>
        /// An add has been issued
        /// </summary>
        Add,
        /// <summary>
        /// An update has been issued
        /// </summary>
        Update,
        /// <summary>
        /// A delete has been issued
        /// </summary>
        Delete,
        /// <summary>
        /// An entity has been selected
        /// </summary>
        Select,
        /// <summary>
        /// An entity has received focus
        /// </summary>
        Focus,
        /// <summary>
        /// An entity has been queried
        /// </summary>
        Query
    }
}