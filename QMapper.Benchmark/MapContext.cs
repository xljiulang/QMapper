using AutoMapper;
using BenchmarkDotNet.Attributes;
using EmitMapper;
using QMapper.Model;

namespace QMapper.Benchmark
{
    public class MapContext
    {
        private readonly UserDto source = new UserDto();

        private static readonly IMapper autoMapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserInfo>()).CreateMapper();

        private static readonly QMapper.IMapper<UserDto, UserInfo> qMapper = Map.From<UserDto>().Compile<UserInfo>();

        private static readonly ObjectsMapper<UserDto, UserInfo> emitmapper = ObjectMapperManager.DefaultInstance.GetMapper<UserDto, UserInfo>();


        [Benchmark]
        public void QMap()
        {
            source.AsMap().To<UserInfo>();

            // qMapper.Map(source, new UserInfo());
        }

        [Benchmark]
        public void EmitMap()
        {
            ObjectMapperManager.DefaultInstance.GetMapper<UserDto, UserInfo>().Map(source);

            // emitmapper.Map(source);
        }

        [Benchmark]
        public void AutoMap_Singleton_Configuration()
        {
            autoMapper.Map<UserDto, UserInfo>(source);
        }

        [Benchmark]
        public void AutoMap_Transient_Configuration()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserInfo>()).CreateMapper();
            mapper.Map<UserDto, UserInfo>(source);
        }
    }
}
