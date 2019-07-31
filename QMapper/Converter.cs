using System;
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
        /// 获取本类型声明的静态方法
        /// </summary>
        /// <param name="name">方法名</param>
        /// <returns></returns>
        protected MethodInfo GetStaticMethod(string name)
        {
            return this.GetType().GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// 转换器实例
        /// </summary>
        private static readonly Converter converter;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static Converter()
        {
            var converters = new Converter[]
            {
                new TypeEqualsConverter(),
                new TypeAndNullableTypeConverter(),
                new StringTargetConverter(),
                new EnumTargetConverter(),
                new ConvertibleConverter()
            };

            converters.Aggregate((pre, next) =>
            {
                pre.Next = next;
                return next;
            }).Next = new OthersTargetConverter();

            converter = converters.First();
        }

        /// <summary>
        /// 表达式类型转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static Expression Convert(Expression value, Type targetType)
        {
            var context = new Context(value, targetType);
            return converter.Invoke(context);
        }
    }
}
