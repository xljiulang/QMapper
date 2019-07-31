using AutoMapper;
using BenchmarkDotNet.Attributes;
using EmitMapper;
using QMapper.Model;

namespace QMapper.Benchmark
{
    public class MapContext
    {
        private readonly UserDto user = new UserDto();

        private static readonly IMapper autoMapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserInfo>()).CreateMapper();

        [Benchmark]
        public void QMap()
        {
            user.AsMap().To<UserInfo>();
        }

        [Benchmark]
        public void EmitMap()
        {
            ObjectMapperManager.DefaultInstance.GetMapper<UserDto, UserInfo>().Map(user);
        }

        [Benchmark]
        public void AutoMap_Singleton_Configuration()
        {
            autoMapper.Map<UserDto, UserInfo>(user);
        }

        [Benchmark]
        public void AutoMap_Transient_Configuration()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserInfo>()).CreateMapper();
            mapper.Map<UserDto, UserInfo>(user);
        }
    }
}
