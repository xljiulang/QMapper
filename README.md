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
BenchmarkDotNet=v0.11.5, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-4460 CPU 3.20GHz (Haswell), 1 CPU, 4 logical and 4 physical cores
Frequency=3117851 Hz, Resolution=320.7337 ns, Timer=TSC
.NET Core SDK=3.0.100-preview6-012264
  [Host]     : .NET Core 3.0.0-preview6-27804-01 (CoreCLR 4.700.19.30373, CoreFX 4.700.19.30308), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0-preview6-27804-01 (CoreCLR 4.700.19.30373, CoreFX 4.700.19.30308), 64bit RyuJIT


|                          Method |           Mean |          Error |         StdDev |
|-------------------------------- |---------------:|---------------:|---------------:|
|                            QMap |       117.0 ns |      0.6963 ns |      0.6513 ns |
|                         EmitMap |       574.5 ns |     11.0869 ns |     10.3707 ns |
| AutoMap_Singleton_Configuration |       203.9 ns |      2.4170 ns |      2.2609 ns |
| AutoMap_Transient_Configuration | 2,251,689.1 ns | 28,790.2958 ns | 24,041.2009 ns |

```
