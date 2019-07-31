using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 源类型和目标类型为某类型及其可空类型
    /// </summary>
    class TypeAndNullableTypeConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.ValueNotNullType == context.TargetNotNullType)
            {
                return Expression.Convert(context.Value, context.TargetType);
            }

            return this.Next.Invoke(context);
        }
    }
}
