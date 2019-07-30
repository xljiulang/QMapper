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
