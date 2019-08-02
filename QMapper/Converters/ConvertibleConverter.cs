using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 提供Convertible类型之间转换
    /// </summary>
    class ConvertibleConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.Source.NotNullType.IsInheritFrom<IConvertible>() == false)
            {
                return this.Next.Invoke(context);
            }

            if (context.Target.NotNullType.IsInheritFrom<IConvertible>() == false)
            {
                return this.Next.Invoke(context);
            }

            var thenValue = this.CallConvertMethod(nameof(ConverToConvertible), context);
            return context.IfValueNotNullThen(thenValue);
        }
        
        /// <summary>
        /// 将IConvertible转换为IConvertible类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetNotNullType"></param>
        /// <returns></returns>
        private static object ConverToConvertible(IConvertible value, Type targetNotNullType)
        { 
            return value.ToType(targetNotNullType, null);
        }
    }
}
