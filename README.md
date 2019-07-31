# QMapper
快、准、狠的Mapper组件

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


|                          Method |           Mean |          Error |        StdDev |         Median |
|-------------------------------- |---------------:|---------------:|--------------:|---------------:|
|                            QMap |       201.0 ns |       4.368 ns |      11.74 ns |       198.2 ns |
|                         EmitMap |       832.6 ns |      42.569 ns |     119.37 ns |       780.7 ns |
| AutoMap_Singleton_Configuration |       428.1 ns |      12.006 ns |      33.86 ns |       415.0 ns |
| AutoMap_Transient_Configuration | 4,694,588.0 ns | 117,854.402 ns | 341,916.99 ns | 4,559,848.4 ns |

```
