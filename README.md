# MiniMapper
快、准、狠的小小Mapper组件

### 约定
* 属性名称相同的才能映射
* 属性名称大小写不敏感
* 不支持嵌套属性映射

### 特性
* 调用映射时支持传入要映射的属性名
* 调用映射时支持传入忽略映射的属性名
* 无缓存无字典，性能是EmitMapper的3倍
* 不使用Emit，支持非public类型映射

### 使用例子
```
var a = new A();

var b = a.AsMap().To<B>();
var b = a.AsMap("Name","Age").To<B>();
var b = a.AsMap().Ignore(item=>item.Id).To<B>();

```

### Benchmark
```
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.885 (1803/April2018Update/Redstone4)
Intel Core i3-4150 CPU 3.50GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=3.0.100-preview6-012264
  [Host]     : .NET Core 3.0.0-preview6-27804-01 (CoreCLR 4.700.19.30373, CoreFX 4.700.19.30308), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0-preview6-27804-01 (CoreCLR 4.700.19.30373, CoreFX 4.700.19.30308), 64bit RyuJIT


|                          Method |           Mean |          Error |         StdDev |         Median |
|-------------------------------- |---------------:|---------------:|---------------:|---------------:|
|                         MiniMap |       139.4 ns |       3.345 ns |       9.599 ns |       136.7 ns |
|                         EmitMap |       782.0 ns |      31.975 ns |      93.777 ns |       767.7 ns |
| AutoMap_Singleton_Configuration |       210.7 ns |       6.243 ns |      18.210 ns |       205.9 ns |
| AutoMap_Transient_Configuration | 3,176,564.5 ns | 107,328.247 ns | 304,472.373 ns | 3,075,543.4 ns |

```
