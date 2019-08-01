using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 不支持的转换器
    /// </summary>
    class NotSupportedConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            var exception = new NotSupportedException($"不支持{context.Source.Type} {context.Value}转换为{context.Target.Type}");
            var throwExp = Expression.Throw(Expression.Constant(exception));
            return Expression.Block(throwExp, Expression.Default(context.Target.Type));
        }
    }
}
