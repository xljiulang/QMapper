using System;

namespace MiniMapper
{
    /// <summary>
    /// 表示映射异常
    /// </summary>
    public class MapException : Exception
    {
        /// <summary>
        /// 获取映射的属性名称
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 映射异常
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="inner"></param>
        public MapException(string propertyName, Exception inner)
            : base($"属性{propertyName}类型转换异常", inner)
        {
            this.PropertyName = propertyName;
        }
    }
}
