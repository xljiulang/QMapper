using System;
using System.Linq.Expressions;

namespace MiniMapper
{
    /// <summary>
    /// 定义表示映射体的接口
    /// 提供对相同名称的属性进行映射
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IMap<TSource> where TSource : class
    {
        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="ignoreKey">忽略的字段</param>
        /// <returns></returns>
        IMap<TSource> Ignore<TKey>(Expression<Func<TSource, TKey>> ignoreKey);

        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <param name="memberName">忽略的字段</param>
        /// <returns></returns>
        IMap<TSource> Ignore(params string[] memberName);

        /// <summary>
        /// 映射到目标对象
        /// 要求destination为public修饰
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>     
        /// <returns></returns>
        TDestination To<TDestination>() where TDestination : class, new();

        /// <summary>
        /// 映射到目标对象
        /// 要求destination为public修饰
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="destination">目标对象</param>
        /// <returns></returns>
        TDestination To<TDestination>(TDestination destination) where TDestination : class;
    }
}
