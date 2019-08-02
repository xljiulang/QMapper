using System;

namespace QMapper
{
    /// <summary>
    /// 提供编译型MapBuilder的创建
    /// 提供对象转换为动态MapBuilder的扩展
    /// </summary>
    public static class Map
    {
        /// <summary>
        /// 从源类型创建可编译的MapBuilder
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static ICompileMapBuilder<TSource> From<TSource>() where TSource : class
        {
            return new MapBuilder<TSource>();
        }

        /// <summary>
        /// 转换为动态MapBuilder
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IDynamicMapBuilder<TSource> AsMap<TSource>(this TSource value) where TSource : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new MapBuilder<TSource>(value);
        }

        /// <summary>
        /// 转换为动态MapBuilder
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="value"></param>
        /// <param name="includeMembers">要映射的成员名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IDynamicMapBuilder<TSource> AsMap<TSource>(this TSource value, params string[] includeMembers) where TSource : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new MapBuilder<TSource>(value, includeMembers);
        }
    }
}