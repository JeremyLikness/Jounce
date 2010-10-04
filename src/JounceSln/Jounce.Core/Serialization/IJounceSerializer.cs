namespace Jounce.Core.Serialization
{
    /// <summary>
    ///     Interface for class to perform serialization
    /// </summary>
    public interface IJounceSerializer
    {
        /// <summary>
        ///     Serialize an object that can be serialized
        /// </summary>
        /// <param name="src">The source object</param>
        void Serialize(object src);

        /// <summary>
        ///     Serialize with folder
        /// </summary>
        /// <param name="src">The source object</param>
        /// <param name="folder">The folder</param>
        void Serialize(object src, string folder);

        /// <summary>
        ///     Deserialize the object
        /// </summary>
        object Deserialize<T>();

        /// <summary>
        ///     Deserialize with folder
        /// </summary>
        /// <param name="folder">The folder to deserialize from</param>
        void Deserialize<T>(string folder);
    }
}