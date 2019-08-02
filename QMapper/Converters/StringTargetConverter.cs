using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QMapper
{
    /// <summary>
    /// 目标为string的转换器
    /// </summary>
    class StringTargetConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.Target.Type != typeof(string))
            {
                return this.Next.Invoke(context);
            }

            var method = context.Source.Type.GetMethod(nameof(ToString), new Type[0]);
            var toString = Expression.Call(context.Value, method);
            return context.IfValueNotNullThen(toString);
        }
    }
}
