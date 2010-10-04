using System;
using System.ComponentModel.Composition;
using Jounce.Core.Serialization;

namespace Jounce.Framework.Services
{
    /// <summary>
    ///     The serializer
    /// </summary>
    [Export(typeof(IJounceSerializer))]
    public class Serializer : IJounceSerializer 
    {
        /// <summary>
        ///     Serialize an object that can be serialized
        /// </summary>
        /// <param name="src">The source object</param>
        public void Serialize(object src)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Serialize with folder
        /// </summary>
        /// <param name="src">The source object</param>
        /// <param name="folder">The folder</param>
        public void Serialize(object src, string folder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Deserialize the object
        /// </summary>
        public object Deserialize<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Deserialize with folder
        /// </summary>
        /// <param name="folder">The folder to deserialize from</param>
        public void Deserialize<T>(string folder)
        {
            throw new NotImplementedException();
        }
    }
}
