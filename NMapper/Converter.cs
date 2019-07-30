using System;
using System.Reflection;

namespace NMapper
{
    /// <summary>
    /// 提供类型转换
    /// </summary>
    static class Converter
    {
        /// <summary>
        /// 获取类型转换方法
        /// </summary>
        public static readonly MethodInfo ConvertToTypeMethod = typeof(Converter).GetMethod($"{nameof(Converter.ConvertToType)}", BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetType">转换的目标类型</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        public static object ConvertToType(object value, Type targetType)
        {
            if (value == null)
            {
                return null;
            }

            if (value.GetType() == targetType)
            {
                return value;
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                targetType = underlyingType;
            }

            if (targetType.GetTypeInfo().IsEnum == true)
            {
                return Enum.Parse(targetType, value.ToString(), true);
            }

            if (value is IConvertible convertible && typeof(IConvertible).IsAssignableFrom(targetType))
            {
                return convertible.ToType(targetType, null);
            }

            if (typeof(Guid) == targetType)
            {
                return Guid.Parse(value.ToString());
            }

            if (typeof(DateTimeOffset) == targetType)
            {
                return DateTimeOffset.Parse(value.ToString());
            }

            throw new NotSupportedException($"不支持将对象{value}转换为{targetType}");
        }
    }
}
