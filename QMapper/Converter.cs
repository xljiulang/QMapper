using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QMapper
{
    /// <summary>
    /// 提供高效的属性转换器
    /// </summary>
    abstract class Converter
    {
        /// <summary>
        /// 转换器实例
        /// </summary>
        private static readonly Converter converter;

        /// <summary>
        /// 下一个转换器
        /// </summary>
        public Converter Next { get; set; }

        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public abstract Expression Invoke(Context context);


        /// <summary>
        /// 静态构造器
        /// </summary>
        static Converter()
        {
            var converters = new Converter[]
            {
                new TypeEqualsConverter(),
                new ExplicitImplicitConverter(),
                new StringTargetConverter(),
                new EnumTargetConverter(),
                new ConvertibleConverter(),
                new OthersTargetConverter()
            };

            converters.Aggregate((pre, next) =>
            {
                pre.Next = next;
                return next;
            }).Next = new NotSupportedConverter();

            converter = converters.First();
        }

        /// <summary>
        /// 表达式类型转换
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public static Expression Convert(Context context)
        {
            return converter.Invoke(context);
        }

        /// <summary>
        /// 调用静态转换方法
        /// TResult ConvertMethod(TValue value, Type targetNonNullableType)
        /// 第一个参数为context.Value，第二个方法为目标类型的非空类型
        /// </summary>
        /// <param name="methodName">转换方法</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        protected Expression CallConvertMethod(string methodName, Context context)
        {
            var method = this.GetType().GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var parameters = method.GetParameters();
            var args = new Expression[parameters.Length];

            Debug.Assert(args.Length == 1 || args.Length == 2);

            var valueArgType = parameters[0].ParameterType;
            if (context.Source.Type == valueArgType)
            {
                args[0] = context.Value;
            }
            else
            {
                args[0] = Expression.Convert(context.Value, valueArgType);
            }

            if (parameters.Length == 2)
            {
                args[1] = Expression.Constant(context.Target.NonNullableType);
            }

            var result = Expression.Call(null, method, args);
            return result.Type == context.Target.Type ? result : (Expression)Expression.Convert(result, context.Target.Type);
        }
    }
}
