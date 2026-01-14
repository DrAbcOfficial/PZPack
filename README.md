# PZPack

PZPack 是一个用于处理 Project Zomboid（僵尸毁灭工程）游戏资源包格式的 C# 库。

PZPack is a C# library for handling Project Zomboid game resource pack formats.

## 功能特点 (Features)

- 支持检测 Project Zomboid 资源包格式
- 支持解析和处理 V1 版本资源包
- 支持解析和处理 V2 版本资源包
- 提供资源包编码功能
- 管理纹理图集和 PNG 图像数据

- Supports detection of Project Zomboid resource pack formats
- Supports parsing and handling of V1 resource packs
- Supports parsing and handling of V2 resource packs
- Provides resource pack encoding functionality
- Manages texture atlases and PNG image data

## 安装 (Installation)

### 通过 NuGet 安装 (Install via NuGet)

您可以通过 NuGet 包管理器安装 PZPack：

[https://www.nuget.org/packages/DrAbc.PZPack](https://www.nuget.org/packages/DrAbc.PZPack)

You can install PZPack via NuGet Package Manager:

[https://www.nuget.org/packages/DrAbc.PZPack](https://www.nuget.org/packages/DrAbc.PZPack)

### 手动安装 (Manual Installation)

将 PZPack 项目添加到您的解决方案中：

1. 克隆或下载此仓库
2. 在 Visual Studio 中打开您的解决方案
3. 右键单击解决方案 -> 添加 -> 现有项目
4. 选择 PZPack.csproj 文件
5. 添加对 PZPack 项目的引用到您的主项目

Add the PZPack project to your solution:

1. Clone or download this repository
2. Open your solution in Visual Studio
3. Right-click on the solution -> Add -> Existing Project
4. Select the PZPack.csproj file
5. Add a reference to the PZPack project in your main project

## 使用示例 (Usage Examples)

### 检测资源包类型 (Detect resource pack type)

```csharp
using PZPack;

string filePath = "path/to/resource.pack";
PZPack.PZPackType type = PZPack.IsFileAPZPack(filePath);

if (type == PZPack.PZPackType.V1)
    Console.WriteLine("This is a V1 resource pack");
else if (type == PZPack.PZPackType.V2)
    Console.WriteLine("This is a V2 resource pack");
else
    Console.WriteLine("This is not a valid Project Zomboid resource pack");
```

### 打开 V1 资源包 (Open a V1 resource pack)

```csharp
using PZPack;

string filePath = "path/to/resource.v1.pack";
PZPackV1 pack = PZPack.OpenV1(filePath);

// 访问纹理图集
foreach (var page in pack.Pages)
{
    Console.WriteLine($"Page: {page.Name}, Width: {page.Width}, Height: {page.Height}");
    
    // 访问页面中的资源条目
    foreach (var entry in page.Entries)
    {
        Console.WriteLine($"  Entry: {entry.Name}, X: {entry.X}, Y: {entry.Y}, Width: {entry.Width}, Height: {entry.Height}");
    }
}

// 访问 PNG 图像数据
byte[] pngData = pack.Png;
```

### 打开 V2 资源包 (Open a V2 resource pack)

```csharp
using PZPack;

string filePath = "path/to/resource.v2.pack";
PZPackV2 pack = PZPack.OpenV2(filePath);

// 访问纹理图集
foreach (var page in pack.Pages)
{
    Console.WriteLine($"Page: {page.Name}, Width: {page.Width}, Height: {page.Height}");
    
    // 访问页面中的资源条目
    foreach (var entry in page.Entries)
    {
        Console.WriteLine($"  Entry: {entry.Name}, X: {entry.X}, Y: {entry.Y}, Width: {entry.Width}, Height: {entry.Height}");
    }
}

// 访问 PNG 图像数据
byte[] pngData = pack.Png;
```

### 创建并保存资源包 (Create and save a resource pack)

```csharp
using PZPack;
using PZPack.Interface;

// 创建 V1 资源包
PZPackV1 pack = new PZPackV1();

// 设置纹理图集
pack.Pages = new[]
{
    new PZPage
    {
        Name = "ui_atlas",
        Width = 1024,
        Height = 1024,
        Entries = new[]
        {
            new PZEntry
            {
                Name = "button_normal",
                X = 0,
                Y = 0,
                Width = 100,
                Height = 30
            },
            new PZEntry
            {
                Name = "button_hover",
                X = 0,
                Y = 30,
                Width = 100,
                Height = 30
            }
        }
    }
};

// 设置 PNG 图像数据（从文件加载）
pack.Png = File.ReadAllBytes("path/to/atlas.png");

// 保存资源包
using FileStream fs = new("output.v1.pack", FileMode.Create);
pack.Encode(fs);
```

## 许可证 (License)

MIT License