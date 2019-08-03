using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 显式或隐式转换
    /// </summary>
    class ExplicitImplicitConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            try
            {
                return Expression.Convert(context.Value, context.Target.Type);
            }
            catch (Exception)
            {
                return this.Next.Invoke(context);
            }
        }
    }
}
