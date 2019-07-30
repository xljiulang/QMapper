using AutoMapper;
using BenchmarkDotNet.Attributes;
using EmitMapper;

namespace MiniMapper.Benchmark
{
    public class MapContext
    {
        private readonly A a = new A();

        private static readonly IMapper autoMapper = new MapperConfiguration(cfg => cfg.CreateMap<A, B>()).CreateMapper();

        [Benchmark]
        public void MiniMap()
        {
            a.AsMap().To<B>();
        }

        [Benchmark]
        public void EmitMap()
        {
            ObjectMapperManager.DefaultInstance.GetMapper<A, B>().Map(a);
        }

        [Benchmark]
        public void AutoMap_Singleton_Configuration()
        {
            autoMapper.Map<A, B>(a);
        }

        [Benchmark]
        public void AutoMap_Transient_Configuration()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<A, B>()).CreateMapper();
            mapper.Map<A, B>(a);
        }
    }

    public class A
    {
        public string Name { get; set; } = "A";

        public int? Age { get; set; } = 9;

        public string Email { get; set; } = "@A";
    }

    public class B
    {
        public string Name { get; set; } = "B";

        public int Age { get; set; } = 100;

        public string Email { get; set; } = "@B";
    }
}
