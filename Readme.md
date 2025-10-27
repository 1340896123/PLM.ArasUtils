# PLM.ArasUtils

[![NuGet](https://img.shields.io/nuget/v/PLM.ArasUtils.svg)](https://www.nuget.org/packages/PLM.ArasUtils/)
[![NuGet Info](https://img.shields.io/badge/nuget.info-PLM.ArasUtils-blue)](https://nuget.info/packages/PLM.ArasUtils)
[![GitHub license](https://img.shields.io/github/license/1340896123/PLM.ArasUtils.svg)](https://github.com/1340896123/PLM.ArasUtils/blob/main/LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/1340896123/PLM.ArasUtils.svg)](https://github.com/1340896123/PLM.ArasUtils/stargazers)

## 项目概述

PLM.ArasUtils 是一个针对 Aras Innovator PLM 系统的 .NET 扩展库，提供了丰富的扩展方法来简化 Aras IOM API 的使用。该库使开发者能够更方便地操作 Aras Item 对象，支持 LINQ 查询、链式调用和类型安全的属性访问。

## 项目信息

- **项目名称**: PLM.ArasUtils
- **版本**: 1.0.7
- **作者**: Liaoyujie
- **目标框架**: .NET Standard 2.0
- **语言版本**: C# 13
- **许可证**: MIT License
- **GitHub**: https://github.com/1340896123/PLM.ArasUtils
- **NuGet**: https://www.nuget.org/packages/PLM.ArasUtils/
- **NuGet Info**: https://nuget.info/packages/PLM.ArasUtils

## 支持的 Aras 版本

该库支持多个 Aras Innovator 版本：

- **Released 配置**: 使用 com.broadway.aras.2023r (v14.1.3)
- **11sp12Released 配置**: Aras v11 SP12 (IOM.dll 路径: E:\Program Files (x86)\Aras\Innovatorv11sp12_bw\Innovator\Server\bin\)
- **12sp9Released 配置**: Aras v12 SP9 (IOM.dll 路径: E:\Program Files (x86)\Aras\12SP9\Innovator\Server\bin\)

## 核心功能

### 1. Item 集合操作

#### LINQ 支持
```csharp
// 将 Item 转换为 List 并支持 LINQ 操作
var items = inn.newItem("part").apply().ToList().AsParallel();
var item_numbers = items.Select(l => l.getProperty<string>("item_number"));
```

#### 链式调用
```csharp
// 支持链式设置属性
items.setProperty("make_buy", "buy").setProperty("name", "1234");
```

#### 批量操作
```csharp
// 批量应用操作
var results = items.apply();
// 批量设置 Action
items.setAction("update");
// 批量设置属性
items.SetProperty("property_name", "value");
```

### 2. 类型安全的属性访问

#### 标准属性访问器
```csharp
Item item;
item.CreatedById<string>();      // 获取创建者ID
item.CreatedOn<DateTime>();      // 获取创建时间
item.Classification<string>();   // 获取分类
item.OwnedById<string>();        // 获取所有者ID
item.KeyedName<string>();        // 获取KeyedName
item.Id<string>();               // 获取ID
item.ModifiedOn<DateTime>();     // 获取修改时间
item.CurrentState<string>();     // 获取当前状态
item.MajorRev<string>();         // 获取主版本
item.MinorRev<string>();         // 获取次版本
item.IsReleased<bool>();         // 是否已发布
```

#### 泛型属性获取
```csharp
// 支持泛型的属性获取
var intValue = item.getValue<int>("quantity");
var dateValue = item.getValue<DateTime>("created_on");
var stringValue = item.getValue<string>("name");
```

### 3. 关系操作

#### 获取关系项
```csharp
// 获取所有关系类型
var relationships = item.FetchRelationships();

// 获取指定类型的关系
var specificRelations = item.FetchRelationships("Part BOM");

// 获取源项
var sourceItem = item.getItemSourceItem();

// 获取相关项
var relatedItem = item.getItemRelatedItem();

// 获取属性项
var propertyItem = item.getItemPropertyItem("property_name");
```

### 4. 数据转换和序列化

#### JSON 支持
```csharp
// 字符串转 JObject
var jsonObject = jsonString.ToJObject();

// 对象序列化
var json = obj.ToJson();

// 字符串转 JArray
var jsonArray = jsonString.ToJArrary();
```

#### 属性字典
```csharp
// 获取所有属性作为字典
var properties = item.getPropertys();
```

### 5. 列表和本地化支持

#### 获取列表标签
```csharp
// 获取列表的本地化标签
var label = item.GetListLabelByLang("property_name", "zc");

// 获取过滤列表标签
var filterLabel = item.GetFilterListLabel("property_name", "zc");

// 获取项的 KeyedName
var keyName = item.GetItemKeyName("property_name", "default_value", "zh-cn");
```

### 6. 错误处理和验证

#### 错误检查
```csharp
// 检查错误并抛出异常
item.CheckError();

// 批量错误检查
items.CheckError();
```

## API 参考

### Extend 类

该类包含了对 Aras Item 对象的主要扩展方法。

#### 集合操作方法
- `Count(this Item items)`: 获取 Item 集合的数量
- `ToList(this Item items)`: 将 Item 转换为 List<Item>
- `Enumerator(this Item items)`: 获取 Item 枚举器
- `ToItems(this IEnumerable<Item> listItem)`: 将 Item 列表转换为单个 Item

#### 属性操作方法
- `getValue(this Item item, string propName, string defaultValue = "")`: 获取属性值
- `setValue(this Item item, string propName, string value)`: 设置属性值
- `getValue<T>(this Item item, string propName, T defaultvalue = default)`: 泛型获取属性值
- `getProperty<T>(this Item item, string propertyName)`: 泛型获取属性值

#### 关系操作方法
- `FetchRelationships(this Item item, string type)`: 获取关系项
- `getItemSourceItem(this Item item, string type)`: 获取源项
- `getItemRelatedItem(this Item item, string type)`: 获取相关项
- `getItemById(this Item item, string type, string id)`: 根据类型和ID获取项

#### 批量操作方法
- `apply(this IEnumerable<Item> listItem, string action = default)`: 批量应用操作
- `setAction(this IEnumerable<Item> listItem, string action)`: 批量设置动作
- `SetProperty(this IEnumerable<Item> listItem, string prop, string value, string lang = default)`: 批量设置属性

### ItemPropertyExtend 类

该类提供了对标准 Aras 属性的类型安全访问器。

#### 系统属性
- `CreatedById<T>(this Item item, T defaultValue = default)`
- `CreatedOn<T>(this Item item, T defaultValue = default)`
- `ModifiedById<T>(this Item item, T defaultValue = default)`
- `ModifiedOn<T>(this Item item, T defaultValue = default)`
- `OwnedById<T>(this Item item, T defaultValue = default)`
- `ManagedById<T>(this Item item, T defaultValue = default)`

#### 项属性
- `Classification<T>(this Item item, T defaultValue = default)`
- `KeyedName<T>(this Item item, T defaultValue = default)`
- `Id<T>(this Item item, T defaultValue = default)`
- `CurrentState<T>(this Item item, T defaultValue = default)`
- `State<T>(this Item item, T defaultValue = default)`

#### 版本控制属性
- `MajorRev<T>(this Item item, T defaultValue = default)`
- `MinorRev<T>(this Item item, T defaultValue = default)`
- `Generation<T>(this Item item, T defaultValue = default)`
- `IsCurrent<T>(this Item item, T defaultValue = default)`
- `IsReleased<T>(this Item item, T defaultValue = default)`

## 使用示例

### 基本查询和操作
```csharp
// 获取所有部件并并行处理
var parts = inn.newItem("Part", "get").apply()
    .ToList()
    .AsParallel()
    .Where(p => p.getProperty<string>("make_buy") == "Make")
    .Select(p => new {
        Id = p.Id<string>(),
        Number = p.KeyedName<string>(),
        Created = p.CreatedOn<DateTime>()
    })
    .ToList();
```

### 关系操作
```csharp
// 获取部件的 BOM 关系
var part = inn.newItem("Part", "get").setProperty("item_number", "12345").apply();
var bomItems = part.FetchRelationships("Part BOM");

// 遍历 BOM 项
foreach (var bom in bomItems.Enumerator())
{
    var relatedPart = bom.getItemRelatedItem();
    if (relatedPart != null)
    {
        Console.WriteLine($"Related Part: {relatedPart.getKeyedName()}");
    }
}
```

### 批量更新
```csharp
// 批量更新多个项
var items = inn.newItem("Part", "get").apply().ToList();
var result = items
    .SetProperty("modified_by_id", currentUserId)
    .SetAction("update")
    .apply();
```

### 本地化标签获取
```csharp
// 获取属性的本地化标签
var makeBuyLabel = item.GetListLabelByLang("make_buy", "zc");
var statusLabel = item.GetFilterListLabel("status", "en");
```

## 安装

### NuGet 包信息

- **包名**: `PLM.ArasUtils`
- **版本**: 1.0.7
- **NuGet Gallery**: https://www.nuget.org/packages/PLM.ArasUtils/
- **NuGet Info**: https://nuget.info/packages/PLM.ArasUtils

### NuGet 安装

#### Package Manager Console
```bash
Install-Package PLM.ArasUtils
```

#### .NET CLI
```bash
dotnet add package PLM.ArasUtils
```

#### 项目文件引用
```xml
<PackageReference Include="PLM.ArasUtils" Version="1.0.7" />
```

## 快速开始

### 基本用法

```csharp
using PLM.ArasUtils;

// 创建 Innovator 连接
Innovator inn = // 你的 Innovator 实例

// 基本查询
var parts = inn.newItem("Part", "get").apply().ToList();

// LINQ 查询支持
var makeParts = parts
    .Where(p => p.getProperty<string>("make_buy") == "Make")
    .Select(p => new {
        Id = p.Id<string>(),
        Number = p.KeyedName<string>(),
        Created = p.CreatedOn<DateTime>()
    })
    .ToList();

// 链式操作
var result = parts
    .SetProperty("make_buy", "Buy")
    .SetAction("update")
    .apply();
```

### 依赖项

### NuGet 包依赖
- **com.broadway.aras.2023r**: v14.1.3 (Released 配置)
- **Newtonsoft.Json**: v13.0.3 (Released 配置)

### 版本特定依赖
- **Aras v11 SP12**: Newtonsoft.Json v8.0.0.0
- **Aras v12 SP9**: Newtonsoft.Json v11.0.0.0

## 注意事项

1. **版本兼容性**: 确保使用与 Aras 版本匹配的配置
2. **错误处理**: 建议使用 `CheckError()` 方法进行错误检查
3. **性能优化**: 对于大数据集，考虑使用 `AsParallel()` 进行并行处理
4. **本地化**: 列表标签获取支持多语言，默认语言为中文 ("zc")
5. **内存管理**: 处理大量数据时注意内存使用

## 构建

项目支持多个配置：
- Debug: 调试版本
- Release: 发布版本
- 11sp12Released: Aras 11 SP12 发布版本
- 12sp9Released: Aras 12 SP9 发布版本

## 许可证

本项目使用 MIT 许可证。详情请查看 [LICENSE](https://github.com/1340896123/PLM.ArasUtils/blob/main/LICENSE) 文件。

## 贡献

欢迎提交 [Issue](https://github.com/1340896123/PLM.ArasUtils/issues) 和 [Pull Request](https://github.com/1340896123/PLM.ArasUtils/pulls) 来改进这个项目。

### 如何贡献
1. Fork 本仓库
2. 创建你的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交你的更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启一个 Pull Request
