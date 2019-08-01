namespace QMapper
{
    /// <summary>
    /// 定义映射的接口
    /// 提供对相同名称的属性进行映射
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    public interface IMap<TSource, TDestination> where TSource : class
    {
        /// <summary>
        /// 映射到目标对象
        /// </summary>
        /// <param name="destination">目标对象</param>
        /// <returns></returns>
        TDestination MapTo(TDestination destination);
    }
}
