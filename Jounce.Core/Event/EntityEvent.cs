using System;

namespace Jounce.Core.Event
{
    /// <summary>
    ///     The entity event args
    /// </summary>
    public class EntityEvent 
    {
        /// <summary>
        ///     Entity event args
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="command">The command</param>
        /// <returns>The args to wrap the command</returns>
        public static EntityEvent CreateArgsFor(object entity, EntityCommand command)
        {
            return new EntityEvent
                       {
                           Command = command,
                           Entity = entity,
                           EntityType = entity.GetType()
                       };
        }

        /// <summary>
        ///     Typed create - allows for null args
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="entity">The object</param>
        /// <param name="command">The comman</param>
        /// <returns>The args</returns>
        public static EntityEvent CreateArgsFor<T>(object entity, EntityCommand command)
        {
            return new EntityEvent
            {
                Command = command,
                Entity = entity,
                EntityType = typeof(T)
            };
        }
        /// <summary>
        ///     Type of the entity
        /// </summary>
        public Type EntityType { get; private set; }

        /// <summary>
        ///     Command for the entity
        /// </summary>
        public EntityCommand Command { get; private set; }

        /// <summary>
        ///     Entity itself
        /// </summary>
        public object Entity { get; private set; }
        
        /// <summary>
        ///     cast it
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The cast entity</returns>
        public T EntityForType<T>()
        {
            return Entity == null ? default(T) : (T) Entity;           
        }

        public override string ToString()
        {
            return string.Format("EntityEvent : {0} : {1} : {2}", Command, EntityType.FullName, Entity);
        }
    }
}
