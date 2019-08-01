using System;
using System.Linq;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 提供一些常见类型的转换
    /// </summary>
    class OthersTargetConverter : Converter
    {
        /// <summary>
        /// 本类支持的目标类型
        /// </summary>
        private static readonly Type[] supportedTypes = new Type[] { typeof(Guid), typeof(DateTimeOffset), typeof(Uri), typeof(Version) };

        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (supportedTypes.Contains(context.Target.NotNullType) == false)
            {
                return this.Next.Invoke(context);
            }

            var checkNull = this.CheckNullValueSupported(context);
            var callConvert = this.CallStaticConvert(context, nameof(ConvertToType));
            return Expression.Block(checkNull, callConvert);
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetNotNullType">转换的目标类型</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        private static object ConvertToType(object value, Type targetNotNullType)
        {
            if (typeof(Guid) == targetNotNullType)
            {
                return Guid.Parse(value.ToString());
            }

            if (typeof(DateTimeOffset) == targetNotNullType)
            {
                return DateTimeOffset.Parse(value.ToString());
            }

            if (value == null)
            {
                return null;
            }

            if (typeof(Uri) == targetNotNullType)
            {
                return new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            }

            if (typeof(Version) == targetNotNullType)
            {
                return new Version(value.ToString());
            }

            throw new NotImplementedException();
        }
    }
}
