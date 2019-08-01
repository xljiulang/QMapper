namespace QMapper
{
    /// <summary>
    /// 定义映射的接口
    /// 提供对相同名称的属性进行映射
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IMap<TSource, TDestination> where TSource : class
    {
        /// <summary>
        /// 映射到目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="destination">目标对象</param>
        /// <returns></returns>
        TDestination MapTo(TDestination destination);
    }
}
