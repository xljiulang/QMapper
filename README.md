# NMapper
无预先配置的最小Mapper组件

### 约定
* 属性名称相同的才能映射
* 属性名称大小写不敏感
* 不支持嵌套属性映射

### 动态配置
* 调用映射时支持传入要映射的属性名
* 调用映射时支持传入忽略映射的属性名

### 使用例子
```
var a = new A();

var b = a.AsMap().To<B>();
var b = a.AsMap("Name","Age").To<B>();
var b = a.AsMap().Ignore(item=>item.Id).To<B>();

```
