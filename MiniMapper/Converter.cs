using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MiniMapper
{
    /// <summary>
    /// 提供类型转换
    /// </summary>
    static class Converter
    {
        /// <summary>
        /// 转换为枚举方法
        /// </summary>
        private static readonly MethodInfo convertToEnumMethod = typeof(Converter).GetMethod($"{nameof(Converter.ConvertToEnum)}", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 转换为IConvertible方法
        /// </summary>
        private static readonly MethodInfo converToConvertibleMethod = typeof(Converter).GetMethod($"{nameof(Converter.ConverToConvertible)}", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 转换为特殊类型方法
        /// </summary>
        private static readonly MethodInfo convertToTypeMethod = typeof(Converter).GetMethod($"{nameof(Converter.ConvertToType)}", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 表达式类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Expression Convert(Expression value, Type valueType, Type targetType)
        {
            if (valueType == targetType)
            {
                return value;
            }

            var notNullValueType = valueType.GetUnNullableType();
            var notNullTargetType = targetType.GetUnNullableType();

            if (notNullValueType == notNullTargetType)
            {
                return Expression.Convert(value, targetType);
            }

            var valueArg = Expression.Convert(value, typeof(object));
            var targetTypeArg = Expression.Constant(notNullTargetType);

            if (notNullTargetType.IsInheritFrom<Enum>() == true)
            {
                value = Expression.Call(null, convertToEnumMethod, valueArg, targetTypeArg);
            }
            else if (notNullTargetType.IsInheritFrom<IConvertible>() && notNullValueType.IsInheritFrom<IConvertible>())
            {
                value = Expression.Call(null, converToConvertibleMethod, valueArg, targetTypeArg);
            }
            else
            {
                value = Expression.Call(null, convertToTypeMethod, valueArg, targetTypeArg);
            }

            return Expression.Convert(value, targetType);
        }


        /// <summary>
        /// 将value转换为枚举类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="notNullTargetType">转换的目标类型</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        private static object ConvertToEnum(object value, Type notNullTargetType)
        {
            if (value == null)
            {
                return null;
            }
            return Enum.Parse(notNullTargetType, value.ToString(), true);
        }

        /// <summary>
        /// 将value转换为IConvertible类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="notNullTargetType"></param>
        /// <returns></returns>
        private static object ConverToConvertible(object value, Type notNullTargetType)
        {
            if (value == null)
            {
                return null;
            }

            var convertible = value as IConvertible;
            return convertible.ToType(notNullTargetType, null);
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="notNullTargetType">转换的目标类型</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        private static object ConvertToType(object value, Type notNullTargetType)
        {
            if (value == null)
            {
                return null;
            }

            if (typeof(Guid) == notNullTargetType)
            {
                return Guid.Parse(value.ToString());
            }

            if (typeof(DateTimeOffset) == notNullTargetType)
            {
                return DateTimeOffset.Parse(value.ToString());
            }

            if (typeof(Uri) == notNullTargetType)
            {
                return new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            }

            if (typeof(Version) == notNullTargetType)
            {
                return new Version(value.ToString());
            }

            throw new NotSupportedException($"不支持将值{value}转换为{notNullTargetType}");
        }
    }
}
