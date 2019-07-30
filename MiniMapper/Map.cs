using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MiniMapper
{
    /// <summary>
    /// 表示映射体
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    class Map<TSource> : IMap<TSource> where TSource : class
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private readonly TSource source;

        /// <summary>
        /// 包含的属性名称
        /// </summary>
        private HashSet<string> includeMembers;

        /// <summary>
        /// 源类型的所有属性
        /// </summary>
        private static readonly PropertyInfo[] sourceProperies = typeof(TSource).GetProperties();

        /// <summary>
        /// 源类型的所有属性名称
        /// </summary>
        private static readonly string[] sourceMembers = sourceProperies.Select(item => item.Name).ToArray();


        /// <summary>
        /// 映射体
        /// </summary>
        /// <param name="source">数据源</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Map(TSource source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// 映射体
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="includeMembers"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Map(TSource source, IEnumerable<string> includeMembers)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.includeMembers = new HashSet<string>(includeMembers, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="ignoreKey">忽略的字段</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public IMap<TSource> Ignore<TKey>(Expression<Func<TSource, TKey>> ignoreKey)
        {
            if (ignoreKey == null)
            {
                throw new ArgumentNullException(nameof(ignoreKey));
            }

            if (ignoreKey.Body is MemberExpression body)
            {
                this.Ignore(body.Member.Name);
            }

            return this;
        }

        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <param name="memberName">忽略的字段</param>
        /// <returns></returns>
        public IMap<TSource> Ignore(params string[] memberName)
        {
            if (this.includeMembers == null)
            {
                this.includeMembers = new HashSet<string>(sourceMembers, StringComparer.OrdinalIgnoreCase);
            }

            foreach (var item in memberName)
            {
                this.includeMembers.Remove(item);
            }

            return this;
        }

        /// <summary>
        /// 映射到目标对象
        /// 要求destination为public修饰
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>     
        /// <returns></returns>
        public TDestination To<TDestination>() where TDestination : class, new()
        {
            return this.To(new TDestination());
        }

        /// <summary>
        /// 映射到目标对象
        /// 要求destination为public修饰
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="destination">目标对象</param>
        /// <returns></returns>
        public TDestination To<TDestination>(TDestination destination) where TDestination : class
        {
            if (destination == null)
            {
                return null;
            }

            if (this.includeMembers == null)
            {
                return MapItem<TDestination>.Map(this.source, destination);
            }
            else
            {
                return MapItem<TDestination>.Map(this.source, destination, this.includeMembers);
            }
        }

        /// <summary>
        /// 表示映射单元
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        private static class MapItem<TDestination>
        {
            /// <summary>
            /// 所有映射属性
            /// </summary>
            private static readonly MapProperty[] maps;

            /// <summary>
            /// 静态构造器
            /// </summary>
            static MapItem()
            {
                var q = from s in sourceProperies
                        join d in typeof(TDestination).GetProperties()
                        on s.Name.ToLower() equals d.Name.ToLower()
                        let map = new MapProperty(s, d)
                        where map.IsEnable
                        select map;

                maps = q.ToArray();
            }

            /// <summary>
            /// 映射所有默认匹配的属性
            /// </summary>
            /// <param name="source">源</param>
            /// <param name="destination">目标</param>             
            /// <returns></returns>
            public static TDestination Map(TSource source, TDestination destination)
            {
                foreach (var map in maps)
                {
                    map.Invoke(source, destination);
                }
                return destination;
            }

            /// <summary>
            /// 映射目标属性
            /// </summary>
            /// <param name="source">源</param>
            /// <param name="destination">目标</param>
            /// <param name="members">映射的属性</param>
            /// <returns></returns>
            public static TDestination Map(TSource source, TDestination destination, HashSet<string> members)
            {
                foreach (var map in maps)
                {
                    if (members.Contains(map.Name) == true)
                    {
                        map.Invoke(source, destination);
                    }
                }
                return destination;
            }

            /// <summary>
            /// 表示映射属性
            /// </summary>
            private class MapProperty
            {
                /// <summary>
                /// 映射委托
                /// </summary>
                private readonly Action<TSource, TDestination> mapAction;

                /// <summary>
                /// 获取属性名称
                /// </summary>
                public string Name { get; }

                /// <summary>
                /// 获取是否可用
                /// </summary>
                public bool IsEnable { get; }

                /// <summary>
                /// 映射属性
                /// </summary>
                /// <param name="propertySource">源属性</param>
                /// <param name="propertyDestination">目标属性</param>
                public MapProperty(PropertyInfo propertySource, PropertyInfo propertyDestination)
                {
                    this.mapAction = CreateMapAction(propertySource, propertyDestination);
                    this.Name = propertySource.Name;
                    this.IsEnable = this.mapAction != null;
                }

                /// <summary>
                /// 创建映射委托
                /// (source,destination) => destination.SetName(source.Name);
                /// </summary>                  
                /// <param name="propertySource">源属性</param>
                /// <param name="propertyDestination">目标属性</param>
                /// <returns></returns>
                private static Action<TSource, TDestination> CreateMapAction(PropertyInfo propertySource, PropertyInfo propertyDestination)
                {
                    var getter = propertySource.GetGetMethod();
                    var setter = propertyDestination.GetSetMethod();
                    if (getter == null || setter == null)
                    {
                        return null;
                    }

                    var source = Expression.Parameter(typeof(TSource), "source");
                    var destination = Expression.Parameter(typeof(TDestination), "destination");
                    var value = (Expression)Expression.Property(source, propertySource);

                    if (propertySource.PropertyType != propertyDestination.PropertyType)
                    {
                        var valueArg = Expression.Convert(value, typeof(object));
                        var targetTypeArg = Expression.Constant(propertyDestination.PropertyType);
                        var objectValue = Expression.Call(null, Converter.ConvertToTypeMethod, valueArg, targetTypeArg);
                        value = Expression.Convert(objectValue, propertyDestination.PropertyType);
                    }

                    var body = Expression.Call(destination, setter, value);
                    return Expression.Lambda<Action<TSource, TDestination>>(body, source, destination).Compile();
                }

                /// <summary>
                /// 执行映射
                /// </summary>
                /// <param name="source">源</param>
                /// <param name="destination">目标</param>
                public void Invoke(TSource source, TDestination destination)
                {
                    this.mapAction.Invoke(source, destination);
                }

                /// <summary>
                /// 转换为字符串
                /// </summary>
                /// <returns></returns>
                public override string ToString()
                {
                    return this.Name;
                }
            }
        }
    }
}
