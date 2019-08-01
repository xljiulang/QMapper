using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 值类型自动转换
    /// </summary>
    class ValueTypeConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.Source.IsValueType && context.Target.IsValueType)
            {
                return Expression.Convert(context.Value, context.Target.Type);
            }

            return this.Next.Invoke(context);
        }
    }
}
