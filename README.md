# QMapper
快、准、狠的Mapper组件

### 约定和限制
* 属性名称相同的才能映射
* 属性名称大小写不敏感
* 不支持嵌套属性映射

### 功能与特性
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

### Benchmark说明
#### 特殊性说明
AutoMap_Singleton_Configuration和其它三个测试条件不一样，为了能够进行测试，只能直接调用它配置好的IMapper实例，但在应用环境中，我们必须从DI或缓存中根据类型映射获取类型对应的IMapper实例，一般的缓存查找将消耗3倍数的时间。

#### QMap为什么快
QMap存在必须的约定和限制，其在架构设计时就可以抛弃缓存，使用泛型类型的静态方法或静态属性取代缓存功能，从而获取到比使用缓存更高的性能。另外，QMap在属性类型转换时，构造Expression时分析使用最优的转换方法，从而在运行时的转换逻辑包含最少的IL指令，同时将装箱拆箱操作降低到最少。

