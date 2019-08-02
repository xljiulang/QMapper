namespace QMapper
{
    /// <summary>
    /// 定义固化了配置的映射器
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    public interface IMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// 将source映射到target
        /// 返回映射后的target
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        TTarget Map(TSource source, TTarget target);
    }
}
