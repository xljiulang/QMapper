using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 两类型一样直接返回源值
    /// </summary>
    class TypeEqualsConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.ValueType == context.TargetType)
            {
                return context.Value;
            }
            return this.Next.Invoke(context);
        }
    }
}
