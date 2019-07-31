using System;

namespace QMapper
{
    /// <summary>
    /// 表示映射异常
    /// </summary>
    public class MapException : Exception
    {
        /// <summary>
        /// 获取源类型
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// 获取目标类型
        /// </summary>
        public Type DestinationType { get; }

        /// <summary>
        /// 映射异常
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="inner"></param>
        public MapException(Type sourceType, Type destinationType, Exception inner)
            : base(null, inner)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
        }

        /// <summary>
        /// 获取提示信息
        /// </summary>
        public override string Message
        {
            get => $"类型{this.SourceType}转换为{this.DestinationType}失败";
        }
    }
}
