using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 定义映射的创建者接口
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IMapBuilder<TSource> where TSource : class
    {
        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="ignoreKey">忽略的字段</param>
        /// <returns></returns>
        IMapBuilder<TSource> Ignore<TKey>(Expression<Func<TSource, TKey>> ignoreKey);

        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <param name="memberName">忽略的字段</param>
        /// <returns></returns>
        IMapBuilder<TSource> Ignore(params string[] memberName);

        /// <summary>
        /// 为指定目标类型编译映射
        /// 返回编译后的映射
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <returns></returns>
        IMap<TSource, TTarget> Compile<TTarget>() where TTarget : class;

        /// <summary>
        /// 映射到目标对象      
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>     
        /// <returns></returns>
        TTarget To<TTarget>() where TTarget : class, new();

        /// <summary>
        /// 映射到目标对象     
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        TTarget To<TTarget>(TTarget target) where TTarget : class;
    }
}
