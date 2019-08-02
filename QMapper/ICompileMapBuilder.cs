using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 定义编译型映射创建者接口
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface ICompileMapBuilder<TSource> where TSource : class
    {
        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="ignoreKey">忽略的字段</param>
        /// <returns></returns>
        ICompileMapBuilder<TSource> Ignore<TKey>(Expression<Func<TSource, TKey>> ignoreKey);

        /// <summary>
        /// 忽略映射的字段
        /// </summary>
        /// <param name="memberName">忽略的字段</param>
        /// <returns></returns>
        ICompileMapBuilder<TSource> Ignore(params string[] memberName);

        /// <summary>
        /// 为指定目标类型编译映射
        /// 返回编译后的映射器
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <returns></returns>
        IMapper<TSource, TTarget> Compile<TTarget>() where TTarget : class;
    }
}
