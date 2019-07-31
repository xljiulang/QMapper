using System;
using System.Reflection;

namespace QMapper
{
    /// <summary>
    /// 类型扩展
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 是否可以从TBase类型派生
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInheritFrom<TBase>(this Type type)
        {
            return typeof(TBase).IsAssignableFrom(type);
        }  

#if !NETSTANDARD1_3
        /// <summary>
        /// 返回type的详细类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }

        /// <summary>
        /// 转换为Type类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type AsType(this Type type)
        {
            return type;
        }
#endif 
    }
}