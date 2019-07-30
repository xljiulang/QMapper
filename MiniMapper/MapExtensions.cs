using System;

namespace MiniMapper
{
    /// <summary>
    /// 提供Map扩展
    /// </summary>
    public static class MapExtensions
    {      
        /// <summary>
        /// 转换为可映射对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IMap<TSource> AsMap<TSource>(this TSource value) where TSource : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new Map<TSource>(value);
        }

        /// <summary>
        /// 转换为可映射对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="value"></param>
        /// <param name="includeMembers">要映射的成员名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IMap<TSource> AsMap<TSource>(this TSource value, params string[] includeMembers) where TSource : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new Map<TSource>(value, includeMembers);
        }       
    }
}