namespace QMapper
{
    /// <summary>
    /// 定义映射的接口
    /// 提供对相同名称的属性进行映射
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    public interface IMap<TSource, TTarget> where TSource : class
    {
        /// <summary>
        /// 映射到目标对象
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        TTarget MapTo(TTarget target);
    }
}
