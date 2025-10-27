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

## Aras IOM API 参考手册

PLM.ArasUtils 基于以下 Aras Innovator .NET API 方法构建，了解原生 API 有助于更好地使用扩展方法。

### 连接方法

#### IomFactory - 连接工厂类

```csharp
// 基本连接
HttpServerConnection connection = IomFactory.CreateHttpServerConnection(
    "http://server/InnovatorServer.aspx");

// 带认证的连接
HttpServerConnection connection = IomFactory.CreateHttpServerConnection(
    "http://server/InnovatorServer.aspx",
    "Database",
    "username",
    "password");

// 创建 Innovator 实例
Innovator innovator = IomFactory.CreateInnovator(connection);

// Windows 认证
HttpServerConnection winAuthConnection = IomFactory.CreateWinAuthHttpServerConnection(
    "http://server/InnovatorServer.aspx", "Database");
```

#### HttpServerConnection 操作

```csharp
// 登录和注销
Item loginResult = connection.Login();
if (loginResult.isError()) {
    Console.WriteLine("登录失败: " + loginResult.getErrorString());
}

// 执行操作
connection.Logout();

// 获取信息
string dbName = connection.GetDatabaseName();
string userId = connection.getUserID();
Item databases = connection.GetDatabases();
Item licenseInfo = connection.GetLicenseInfo();
```

### 核心 Item 操作方法

#### 创建和初始化

```csharp
// 创建不同类型的 Item
Item emptyItem = innovator.newItem();
Item part = innovator.newItem("Part");
Item newPart = innovator.newItem("Part", "add");

// 从 AML 加载
Item item = innovator.newItem();
item.loadAML("<Item type='Part' id='12345' />");
```

#### 属性操作

```csharp
// 设置属性
item.setProperty("name", "我的零件");
item.setProperty("item_number", "PART-001");

// 获取属性
string name = item.getProperty("name");
string number = item.getProperty("item_number");

// 移除属性
item.removeProperty("description");

// 属性特性操作
item.setPropertyAttribute("name", "multilingual", "true");
string multilingual = item.getPropertyAttribute("name", "multilingual");
```

#### 关系操作

```csharp
// 添加关系
Item relationship = item.addRelationship("Related Item");
relationship.setProperty("related_id", "12345");

// 创建新关系
Item newRel = item.createRelationship("CAD", "add");

// 获取关系
Item relationships = item.fetchRelationships("Related Items");

// 移除关系
item.removeRelationship(relationship);
```

#### 标识符操作

```csharp
// ID 操作
string id = item.getID();
item.setID("12345");
item.setNewID();

// 类型操作
string itemType = item.getType();
item.setType("Part");
```

#### 应用和保存

```csharp
// 应用更改
Item result = item.apply();  // 默认动作
Item result = item.apply("add");    // 添加
Item result = item.apply("update"); // 更新
Item result = item.apply("delete"); // 删除

// 直接执行查询
Item sqlResult = innovator.applySQL("SELECT * FROM [Part] WHERE item_number = 'PART-001'");
Item amlResult = innovator.applyAML("<Item type='Part' action='get' select='name,item_number' />");
Item methodResult = innovator.applyMethod("methodName", parametersAML);

// 动作操作
string action = item.getAction();
item.setAction("update");
```

#### 查询操作

```csharp
// XPath 查询
Item children = item.getItemsByXPath("//Relationships/Item[@type='Related Item']");
```

#### 工作流操作

```csharp
// 启动工作流
Item wfResult = item.startWorkflow("WorkflowName");

// 生命周期操作
Item promoteResult = item.promote("StateName", "Comment");
Item cancelResult = item.cancelWorkflow();
Item closeResult = item.closeWorkflow();
```

#### 锁定操作

```csharp
// 锁定和解锁
Item lockResult = item.lockItem();
Item unlockResult = item.unlockItem();
bool isLocked = item.isLocked();
```

#### 错误处理

```csharp
// 检查错误
bool hasError = item.isError();
string errorCode = item.getErrorCode();
string errorMessage = item.getErrorString();
string errorDetail = item.getErrorDetail();
```

#### Item 操作

```csharp
// 克隆和子项操作
Item cloned = item.clone();
item.removeItem(childItem);
item.appendItem(childItem);
```

### 文件操作方法

#### 文件上传

```csharp
// 附加物理文件
item.attachPhysicalFile(@"C:\path\to\file.pdf", "document.pdf");
item.attachPhysicalFile(@"C:\temp\file.pdf", "document.pdf", true); // 上传后删除

// 通过流附加文件
using (FileStream stream = File.OpenRead(@"C:\path\to\file.pdf"))
{
    item.attachPhysicalFileViaStream(stream, "document.pdf");
}

// 设置文件属性
item.setFileProperty("file_property", @"C:\path\to\file.pdf");

// 通过流设置文件属性
using (FileStream stream = File.OpenRead(@"C:\path\to\file.pdf"))
{
    item.setFilePropertyViaStream("file_property", stream, "document.pdf");
}

// 文件名操作
string fileName = item.getFileName();
item.setFileName("document.pdf");
```

#### 文件下载

```csharp
// 获取文件数据
byte[] fileData = item.fetchFileProperty("file_property");

// 通过流获取文件
using (Stream stream = item.fetchFilePropertyWithStream("file_property"))
{
    // 处理文件流
}
```

#### 高级文件管理

```csharp
// 检入管理器
CheckinManager checkinManager = IomFactory.CreateCheckinManager(configurationItem);
Item checkinResult = checkinManager.Checkin(2); // 使用2个线程

// 异步检入
await checkinManager.CheckinAsync();
await checkinManager.CheckinPauseAsync();
await checkinManager.CheckinResumeAsync();
await checkinManager.CheckinCancelAsync();

// 检出管理器
CheckoutManager checkoutManager = IomFactory.CreateCheckoutManager(connection);
DownloadResult downloadResult = checkoutManager.DownloadFiles();

// 异步下载
await checkoutManager.DownloadFilesAsync();
await checkoutManager.DownloadFilesPauseAsync();
await checkoutManager.DownloadFilesResumeAsync();
await checkoutManager.DownloadFilesCancelAsync();
```

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

### 完整工作流示例
```csharp
// 1. 创建连接和登录
HttpServerConnection connection = IomFactory.CreateHttpServerConnection(
    "http://myserver/InnovatorServer.aspx",
    "MyDatabase",
    "username",
    "password");

Item loginResult = connection.Login();
if (loginResult.isError()) {
    Console.WriteLine("登录失败: " + loginResult.getErrorString());
    return;
}

// 2. 创建 Innovator 实例
Innovator innovator = IomFactory.CreateInnovator(connection);

// 3. 使用 PLM.ArasUtils 扩展方法创建零件
var newPart = innovator.newItem("Part", "add");
newPart.setProperty("name", "测试零件")
       .setProperty("item_number", "TEST-001");

// 4. 附加文件
newPart.attachPhysicalFile(@"C:\documents\spec.pdf", "技术规范.pdf");

// 5. 应用并检查错误
var result = newPart.apply();
result.CheckError(); // 使用扩展方法进行错误检查

// 6. 查询并使用 LINQ
var query = innovator.newItem("Part", "get")
    .setProperty("item_number", "TEST-001")
    .apply();

if (!query.isError() && query.getItemCount() > 0) {
    var part = query.getItemByIndex(0);

    // 使用类型安全的属性访问
    var partInfo = new {
        Id = part.Id<string>(),
        Name = part.KeyedName<string>(),
        Created = part.CreatedOn<DateTime>(),
        Modified = part.ModifiedOn<DateTime>(),
        IsReleased = part.IsReleased<bool>()
    };

    Console.WriteLine($"找到零件: {partInfo.Name} (ID: {partInfo.Id})");
    Console.WriteLine($"创建时间: {partInfo.Created}");
    Console.WriteLine($"是否已发布: {partInfo.IsReleased}");
}

// 7. 批量操作示例
var allParts = innovator.newItem("Part", "get").apply().ToList();
var makeParts = allParts
    .Where(p => p.getProperty<string>("make_buy") == "Make")
    .ToList();

// 批量更新
makeParts.SetProperty("review_status", "pending")
         .SetAction("update")
         .apply()
         .CheckError();

// 8. 登出
connection.Logout();
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

## 最佳实践和注意事项

### 🚨 重要注意事项

#### 1. 登录要求
- **必须先登录**: 在调用任何需要服务器交互的方法之前，必须先调用 `connection.Login()`
- **检查登录结果**: 始终检查登录返回的 Item 是否为错误项
- **会话管理**: 注意服务器配置的会话超时设置，未登录就发送请求会导致异常

```csharp
Item loginResult = connection.Login();
if (loginResult.isError()) {
    Console.WriteLine("登录失败: " + loginResult.getErrorString());
    return;
}
```

#### 2. 错误处理
- **始终检查错误**: 使用 `isError()` 方法检查操作结果
- **使用扩展方法**: 推荐使用 `CheckError()` 扩展方法进行错误检查
- **错误信息**: 获取详细的错误信息用于调试

```csharp
// 原生方式
if (result.isError()) {
    Console.WriteLine($"错误: {result.getErrorString()}");
}

// 使用扩展方法
result.CheckError(); // 如果有错误会抛出异常
```

#### 3. 资源管理
- **释放资源**: 使用 `using` 语句或手动调用 `Dispose()` 释放管理器资源
- **连接管理**: 及时调用 `Logout()` 释放服务器连接

```csharp
// 使用 using 语句
using (var connection = IomFactory.CreateHttpServerConnection(url, db, user, pass))
{
    connection.Login();
    // 执行操作
    connection.Logout();
}
```

#### 4. 性能优化
- **批量操作**: 对于大量数据，使用批量操作方法
- **并行处理**: 使用 `AsParallel()` 进行并行处理大数据集
- **异步文件操作**: 对于大文件操作，使用异步方法避免阻塞
- **连接复用**: 复用连接对象避免重复创建

```csharp
// 并行处理大数据集
var parts = inn.newItem("Part", "get").apply()
    .ToList()
    .AsParallel()
    .Where(p => p.getProperty<string>("make_buy") == "Make")
    .ToList();

// 异步文件上传
await checkinManager.CheckinAsync();
```

#### 5. 线程安全
- **检入线程数**: 检入操作支持 1-10 个线程，根据服务器性能调整
- **避免并发修改**: 同一 Item 的并发修改可能导致数据不一致
- **连接共享**: HttpServerConnection 不是线程安全的

#### 6. 文件操作
- **文件大小**: 对于大文件操作，考虑使用流式处理
- **网络稳定性**: 确保网络连接稳定，特别是对于大文件上传下载
- **临时文件**: 合理设置临时文件清理策略

```csharp
// 流式处理大文件
using (FileStream stream = File.OpenRead(largeFile))
{
    item.attachPhysicalFileViaStream(stream, "large_file.pdf");
}
```

#### 7. 权限和安全
- **权限检查**: 确保用户具有执行相应操作所需的权限
- **敏感信息**: 不要在代码中硬编码密码，使用配置文件或环境变量
- **OAuth 支持**: 支持多种认证方式，选择适合的安全方案

### 💡 性能优化建议

#### 1. 查询优化
```csharp
// 好的做法：指定需要的字段
Item query = innovator.newItem("Part", "get");
query.setProperty("select", "item_number,name,created_on");

// 避免查询过多数据
query.setProperty("page_size", "100");
```

#### 2. 批量操作
```csharp
// 批量设置属性
items.SetProperty("status", "updated")
     .SetAction("update")
     .apply();
```

#### 3. 缓存策略
```csharp
// 缓存频繁访问的数据
private static readonly Dictionary<string, string> _labelCache = new();

public string GetCachedLabel(string property, string lang)
{
    string key = $"{property}_{lang}";
    if (!_labelCache.TryGetValue(key, out string label))
    {
        label = item.GetListLabelByLang(property, lang);
        _labelCache[key] = label;
    }
    return label;
}
```

### 🔧 调试技巧

#### 1. AML 调试
```csharp
// 打印 AML 查询
Console.WriteLine(item.apply().dom.xml());
```

#### 2. 错误日志
```csharp
try {
    var result = item.apply();
    result.CheckError();
} catch (Exception ex) {
    Console.WriteLine($"操作失败: {ex.Message}");
    // 记录详细的错误信息
}
```

#### 3. 性能监控
```csharp
var stopwatch = Stopwatch.StartNew();
var result = item.apply();
stopwatch.Stop();
Console.WriteLine($"操作耗时: {stopwatch.ElapsedMilliseconds}ms");
```

### 📋 版本兼容性

#### Aras 版本支持
- **Released 配置**: com.broadway.aras.2023r (v14.1.3)
- **11sp12Released 配置**: Aras v11 SP12
- **12sp9Released 配置**: Aras v12 SP9

#### .NET 兼容性
- **目标框架**: .NET Standard 2.0
- **支持平台**: .NET Framework 4.6.1+, .NET Core 2.0+, .NET 5.0+

### 🌐 本地化支持

#### 多语言标签
```csharp
// 支持的语言代码
var zhLabel = item.GetListLabelByLang("make_buy", "zc");  // 简体中文
var enLabel = item.GetListLabelByLang("make_buy", "en");  // 英文
var jaLabel = item.GetListLabelByLang("make_buy", "ja");  // 日文
```

#### 默认语言
- 列表标签获取默认语言为中文 ("zc")
- 建议在多语言环境中明确指定语言代码

### 🔄 常见问题和解决方案

#### 1. 连接问题
```csharp
// 连接超时
connection.setConnectionTimeout(30000); // 30秒超时

// 重试机制
int retryCount = 3;
while (retryCount-- > 0)
{
    try {
        var result = item.apply();
        break;
    } catch (Exception ex) {
        if (retryCount == 0) throw;
        Thread.Sleep(1000); // 等待1秒后重试
    }
}
```

#### 2. 内存泄漏
```csharp
// 正确的资源释放
using (var innovator = IomFactory.CreateInnovator(connection))
{
    // 使用 innovator
} // 自动释放
```

#### 3. 大数据处理
```csharp
// 分页处理
int pageSize = 1000;
int page = 0;
while (true)
{
    var query = innovator.newItem("Part", "get");
    query.setProperty("page_size", pageSize.ToString());
    query.setProperty("page", page.ToString());

    var results = query.apply();
    if (results.getItemCount() == 0) break;

    // 处理当前页数据
    ProcessPage(results);
    page++;
}
```

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
