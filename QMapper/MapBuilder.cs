using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QMapper
{
    /// <summary>
    /// 映射创建者
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    class MapBuilder<TSource> : IMapBuilder<TSource> where TSource : class
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
        /// 映射创建者
        /// </summary>
        /// <param name="source">数据源</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapBuilder(TSource source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// 映射创建者
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="includeMembers">映射的的属性名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapBuilder(TSource source, IEnumerable<string> includeMembers)
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
        public IMapBuilder<TSource> Ignore<TKey>(Expression<Func<TSource, TKey>> ignoreKey)
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
        public IMapBuilder<TSource> Ignore(params string[] memberName)
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
        /// 为指定目标类型编译映射
        /// 返回编译后的映射
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public IMap<TSource, TDestination> Compile<TDestination>() where TDestination : class
        {
            try
            {
                return new Map<TDestination>(this.source, this.includeMembers);
            }
            catch (MapException)
            {
                throw;
            }
            catch (TypeInitializationException ex)
            {
                throw new MapException(typeof(TSource), typeof(TDestination), ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new MapException(typeof(TSource), typeof(TDestination), ex);
            }
        }

        /// <summary>
        /// 映射到目标对象      
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>     
        /// <returns></returns>
        public TDestination To<TDestination>() where TDestination : class, new()
        {
            return this.To(new TDestination());
        }

        /// <summary>
        /// 映射到目标对象       
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="destination">目标对象</param>
        /// <returns></returns>
        public TDestination To<TDestination>(TDestination destination) where TDestination : class
        {
            try
            {
                return Map<TDestination>.DynamicMap(this.source, destination, this.includeMembers);
            }
            catch (MapException)
            {
                throw;
            }
            catch (TypeInitializationException ex)
            {
                throw new MapException(typeof(TSource), typeof(TDestination), ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new MapException(typeof(TSource), typeof(TDestination), ex);
            }
        }

        /// <summary>
        /// 表示对象映射
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        private class Map<TDestination> : IMap<TSource, TDestination> where TDestination : class
        {
            /// <summary>
            /// 所有映射属性
            /// </summary>
            private static readonly MapItem[] allMapItems;

            /// <summary>
            /// 静态构造器
            /// </summary>
            static Map()
            {
                var q = from s in sourceProperies
                        join d in typeof(TDestination).GetProperties()
                        on s.Name.ToLower() equals d.Name.ToLower()
                        let map = new MapItem(s, d)
                        where map.IsEnable
                        select map;

                allMapItems = q.ToArray();
            }

            /// <summary>
            /// 源实例
            /// </summary>
            private readonly TSource source;

            /// <summary>
            /// 要映射的属性
            /// </summary>
            private readonly MapItem[] mapItems;

            /// <summary>
            /// 对象映射
            /// </summary>
            /// <param name="source">源实例</param>
            /// <param name="members">映射的字段</param>
            public Map(TSource source, ICollection<string> members)
            {
                this.source = source;
                if (members == null)
                {
                    this.mapItems = allMapItems;
                }
                else
                {
                    this.mapItems = allMapItems.Where(item => members.Contains(item.Name)).ToArray();
                }
            }

            /// <summary>
            /// 映射到目标对象     
            /// </summary>
            /// <param name="destination">目标对象</param>
            /// <returns></returns>
            public TDestination MapTo(TDestination destination)
            {
                if (destination == null)
                {
                    return null;
                }

                foreach (var item in this.mapItems)
                {
                    item.Invoke(this.source, destination);
                }
                return destination;
            }

            /// <summary>
            /// 动态映射
            /// </summary>
            /// <param name="source">源</param>
            /// <param name="destination">目标</param>
            /// <param name="members">映射的属性</param>
            /// <returns></returns>
            public static TDestination DynamicMap(TSource source, TDestination destination, HashSet<string> members)
            {
                if (destination == null)
                {
                    return null;
                }

                if (members == null)
                {
                    foreach (var item in allMapItems)
                    {
                        item.Invoke(source, destination);
                    }
                }
                else
                {
                    foreach (var item in allMapItems)
                    {
                        if (members.Contains(item.Name) == true)
                        {
                            item.Invoke(source, destination);
                        }
                    }
                }

                return destination;
            }


            /// <summary>
            /// 表示映射属性项
            /// </summary>
            private class MapItem
            {
                /// <summary>
                /// 映射委托
                /// </summary>
                private readonly Action<TSource, TDestination> action;

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
                public MapItem(PropertyInfo propertySource, PropertyInfo propertyDestination)
                {
                    this.action = CreateAction(propertySource, propertyDestination);
                    this.Name = propertySource.Name;
                    this.IsEnable = this.action != null;
                }

                /// <summary>
                /// 创建映射委托
                /// (source,destination) => destination.Name = source.Name;
                /// </summary>                  
                /// <param name="propertySource">源属性</param>
                /// <param name="propertyDestination">目标属性</param>
                /// <returns></returns>
                private static Action<TSource, TDestination> CreateAction(PropertyInfo propertySource, PropertyInfo propertyDestination)
                {
                    if (propertySource.GetGetMethod() == null || propertyDestination.GetSetMethod() == null)
                    {
                        return null;
                    }

                    var source = Expression.Parameter(typeof(TSource), "source");
                    var destination = Expression.Parameter(typeof(TDestination), "destination");

                    var value = Expression.Property(source, propertySource);
                    var valueCasted = Converter.Convert(value, propertyDestination.PropertyType);

                    var body = Expression.Assign(Expression.Property(destination, propertyDestination), valueCasted);
                    return Expression.Lambda<Action<TSource, TDestination>>(body, source, destination).Compile();
                }

                /// <summary>
                /// 执行映射
                /// </summary>
                /// <param name="source">源</param>
                /// <param name="destination">目标</param>
                public void Invoke(TSource source, TDestination destination)
                {
                    this.action.Invoke(source, destination);
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
