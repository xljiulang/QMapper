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
        public Type TargetType { get; }

        /// <summary>
        /// 映射异常
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="inner">内部异常</param>
        public MapException(Type sourceType, Type targetType, Exception inner)
            : base(null, inner)
        {
            this.SourceType = sourceType;
            this.TargetType = targetType;
        }

        /// <summary>
        /// 获取提示信息
        /// </summary>
        public override string Message
        {
            get => $"类型{this.SourceType}转换为{this.TargetType}失败";
        }
    }
}
