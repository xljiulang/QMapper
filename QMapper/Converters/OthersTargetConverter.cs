using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 提供一些常见类型的转换
    /// </summary>
    class OthersTargetConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            var method = this.GetStaticMethod($"{nameof(ConvertToType)}");
            var valueArg = Expression.Convert(context.Value, typeof(object));
            var targetTypeArg = Expression.Constant(context.TargetNotNullType);

            var value = Expression.Call(null, method, valueArg, targetTypeArg);
            return Expression.Convert(value, context.TargetType);
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetNotNullType">转换的目标类型</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        private static object ConvertToType(object value, Type targetNotNullType)
        {
            if (value == null)
            {
                return null;
            }

            if (typeof(Guid) == targetNotNullType)
            {
                return Guid.Parse(value.ToString());
            }

            if (typeof(DateTimeOffset) == targetNotNullType)
            {
                return DateTimeOffset.Parse(value.ToString());
            }

            if (typeof(Uri) == targetNotNullType)
            {
                return new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            }

            if (typeof(Version) == targetNotNullType)
            {
                return new Version(value.ToString());
            }

            throw new NotSupportedException($"不支持将值{value}转换为{targetNotNullType}");
        }
    }
}
