using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ToDoList.Model
{
    public class Mapper<TSource,TTarget> 
        where TSource: class 
        where TTarget: class
    {
        public delegate object MapGet(object instance);

        public delegate void MapSet(object instance, object value);

        private class PropertyMap
        {
            public MapGet Getter;
            public MapSet Setter;
            public void Invoke(object source, object target)
            {
                Setter(target, Getter(source));
            }
        }

        private readonly List<PropertyMap> _sourceToTarget = 
            new List<PropertyMap>();

        private readonly List<PropertyMap> _targetToSource =
            new List<PropertyMap>();       

        private readonly List<string> _exclude; 
        
        public Mapper(IEnumerable<string> exclusions)
        {
            _exclude = new List<string>(exclusions);
            SetMap();
        }

        public Mapper() : this (Enumerable.Empty<string>())
        {
            
        }

        private void SetMap()
        {
            if (_sourceToTarget.Count > 0)
            {
                return;
            }

            var targetProperties = typeof (TTarget).GetProperties();

            foreach(var property in 
                typeof(TSource).GetProperties())
            {
                if (_exclude.Contains(property.Name) ||
                    !property.CanRead || !property.CanWrite)
                {
                    continue;
                }

                var targetProperty = (from p
                                            in targetProperties
                                        where p.Name.Equals(property.Name)
                                        select p).FirstOrDefault();
                
                if (targetProperty == null || !targetProperty.CanRead
                    || !targetProperty.CanWrite)
                {
                    continue;
                }

                var sourceMap = new PropertyMap
                                    {
                                        Getter = GetGetter(property),
                                        Setter = GetSetter(targetProperty)
                                    };
                var targetMap = new PropertyMap
                                    {
                                        Getter = GetGetter(targetProperty),
                                        Setter = GetSetter(property)
                                    };
                
                _sourceToTarget.Add(sourceMap);
                _targetToSource.Add(targetMap);
            }
        }

        private static MapGet GetGetter(PropertyInfo property)
        {
            return obj => property.GetGetMethod()
                .Invoke(obj, null);
        }

        private static MapSet GetSetter(PropertyInfo property)
        {
            return (obj, value) =>
                    property.GetSetMethod().Invoke(
                    obj, new[] {value});
        }

        public void MapSourceToTarget(TSource source, TTarget target)
        {
            foreach(var map in _sourceToTarget)
            {
                map.Invoke(source, target);
            }
        }

        public void MapTargetToSource(TTarget source, TSource target)
        {
            foreach(var map in _targetToSource)
            {
                map.Invoke(source, target);
            }
        }
    }
}