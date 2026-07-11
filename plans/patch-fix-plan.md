# 补丁失效问题分析与修复方案

## 问题现状

测试日志 `2026-07-12_02.58.19.log` 显示：

1. **第 80 行**：`KrokoshaCasualtiesMPFix` 编译失败 → `Parameter __e does not contain a valid index`
2. **第 182+ 行**：`IsOnBack`/`MpScareCheck`/`HandleLegacyGunUi` 异常仍在发生 → 补丁未生效

## 根因分析

### 问题 1：KrokoshaCasualtiesMPFix Finalizer 注入失败

```
Failed to patch void KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent::Update(): 
System.Exception: Parameter __e does not contain a valid index
```

**原因**：`KrokoshaGunScriptTrackerComponent.Update()` 是一个 **Unity 消息方法**（`private void Update()`）。Unity 引擎通过原生代码直接调用这些方法，它们没有标准的 .NET IL 异常处理块（try-catch-finally）。Harmony 无法向没有 IL 异常处理的方法注入 Finalizer。

**证据**：反编译的 `KrokoshaGunScriptTrackerComponent.cs` 第 150-173 行显示 `private void Update()` 是一个标准的 Unity 消息方法。

### 问题 2：NewFirearmsFix 补丁静默失效

日志中 **没有任何错误** 来自 NewFirearmsFix，但异常仍在发生。

**可能原因**：
- `AccessTools.DeclaredMethod()` 返回 null（方法名/参数不匹配）
- `Prepare()` 返回 null（类型未找到）
- 补丁被 Harmony 静默跳过

**需要验证**：添加调试日志确认 `Prepare()` 和 `TargetMethod()` 的返回值。

## 修复方案

### 方案 A：KrokoshaCasualtiesMPFix — 改用 Prefix 防御

不用 Finalizer 捕获异常，改用 Prefix 在方法执行前检查并跳过危险操作。

```csharp
[HarmonyPatch]
internal class KrokoshaCasualtiesMPFix
{
    private static Type TargetType => AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent");
    
    [HarmonyPrepare] public static bool Prepare() => TargetType != null;
    [HarmonyTargetMethod] public static MethodBase TargetMethod() => AccessTools.DeclaredMethod(TargetType, "Update");
    
    [HarmonyPrefix]
    public static bool Prefix()
    {
        // 如果 Krokosha 未初始化，跳过 Update 逻辑
        // 需要找到具体的 null 检查条件
        return true; // 暂时允许执行，后续根据具体异常调整
    }
}
```

**问题**：不知道 `Update()` 内部哪个对象为 null，Prefix 无法直接防御。

### 方案 B：KrokoshaCasualtiesMPFix — 手动 try-catch 包装

不使用 Harmony 的 `[HarmonyFinalizer]`，改用手动 patch：

```csharp
// 在 Awake 中手动 patch
var original = AccessTools.DeclaredMethod(targetType, "Update");
var prefix = typeof(Plugin).GetMethod("KrokoshaUpdatePrefix", BindingFlags.Static | BindingFlags.NonPublic);
harmony.Patch(original, new HarmonyMethod(prefix));
```

在 Prefix 中用 try-catch 包裹原始方法调用。

**问题**：Harmony 的 Prefix 不能直接 try-catch 原始方法。

### 方案 C：KrokoshaCasualtiesMPFix — 使用 Reverse Patch

创建一个反向补丁，在运行时动态决定是否调用原始方法。

**问题**：Reverse Patch 不适用于此场景。

### 方案 D：KrokoshaCasualtiesMPFix — 放弃 Finalizer，改用 IL 注入

使用 Mono.Cecil 或 Harmony 的 `Transpiler` 在方法开头插入 null 检查。

**问题**：复杂度高。

### 方案 E：KrokoshaCasualtiesMPFix — 使用 Harmony 的 `Patch` 方法手动指定 finalizer 参数名

Harmony 的 finalizer 错误 `Parameter __e does not contain a valid index` 可能是因为参数名不匹配。尝试使用 `Exception __exception` 或其他名称。

**问题**：Harmony 通过参数类型（`Exception`）识别 finalizer，不是参数名。

### 推荐方案：KrokoshaCasualtiesMPFix — 使用 `harmony.Patch()` 手动 patch + 自定义 finalizer

```csharp
[HarmonyPatch]
internal class KrokoshaCasualtiesMPFix
{
    private static Type TargetType => AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent");
    
    [HarmonyPrepare] public static bool Prepare() => TargetType != null;
    [HarmonyTargetMethod] public static MethodBase TargetMethod() => AccessTools.DeclaredMethod(TargetType, "Update");
    
    // 不用 [HarmonyFinalizer]，改用手动 patch
    // 在 Awake 中：
    // var m = AccessTools.DeclaredMethod(targetType, "Update");
    // harmony.Patch(m, finalizer: new HarmonyMethod(typeof(KrokoshaCasualtiesMPFix), "Finalizer"));
    
    public static Exception Finalizer(Exception __e) => null;
}
```

**问题**：`harmony.Patch()` 的 `finalizer` 参数内部仍然使用相同的 IL 注入逻辑，可能仍然失败。

### 最终推荐方案：KrokoshaCasualtiesMPFix — 使用 `PatchAll` + 自定义 `HarmonyManipulator`

放弃 `[HarmonyFinalizer]`，在 `Awake` 中手动创建 Harmony 实例并 patch：

```csharp
// Plugin.cs Awake 中
var harmony = new Harmony(Guid);
var targetType = AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent");
var original = AccessTools.DeclaredMethod(targetType, "Update");
if (original != null)
{
    // 使用 MonoMod 的 IL 手动注入 try-catch
    // 或者使用 Harmony 的 Transpiler
}
```

### 更实际的方案：使用 `Transpiler` 注入 null 检查

对于 `KrokoshaGunScriptTrackerComponent.Update()`，使用 Transpiler 在方法开头插入 null 检查逻辑，提前返回。

**问题**：不知道具体哪个字段为 null。

### 最实际的方案：使用 `Reverse Patch` + 手动调用

```csharp
[HarmonyPatch]
internal class KrokoshaCasualtiesMPFix
{
    private static Type TargetType => AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent");
    
    [HarmonyPrepare] public static bool Prepare() => TargetType != null;
    [HarmonyTargetMethod] public static MethodBase TargetMethod() => AccessTools.DeclaredMethod(TargetType, "Update");
    
    [HarmonyReversePatch]
    public static Func<object, void> OriginalUpdate(object instance)
    {
        throw new NotImplementedException();
    }
    
    [HarmonyPrefix]
    public static bool Prefix(ref bool __runOriginal)
    {
        // 检查是否应该跳过
        __runOriginal = /* 检查条件 */;
        return false;
    }
}
```

## 需要验证的问题

1. **NewFirearmsFix 补丁为何静默失效？**
   - 添加日志确认 `Prepare()` 返回值
   - 添加日志确认 `TargetMethod()` 返回值
   - 检查 `AccessTools.DeclaredMethod()` 是否返回 null

2. **KrokoshaCasualtiesMPFix 的 `Update()` 方法内部哪个对象为 null？**
   - 需要查看反编译的 `KrokoshaGunScriptTrackerComponent.Update()` 方法体
   - 确定 null 检查条件

3. **是否所有目标方法都是 Unity 消息？**
   - `IsOnBack` - 不是 Unity 消息（private 方法，被 `FixedUpdate` 调用）
   - `MpScareCheck` - 不是 Unity 消息（private 方法）
   - `HandleLegacyGunUi` - 不是 Unity 消息（public static 方法）
   - `Update()` - 是 Unity 消息

## 实施步骤

### 步骤 1：添加调试日志

在 `Prepare()` 和 `TargetMethod()` 中添加日志，确认补丁为何失效。

### 步骤 2：修复 KrokoshaCasualtiesMPFix

根据 `Update()` 方法体确定 null 检查条件，使用 Prefix 防御。

### 步骤 3：修复 NewFirearmsFix

根据步骤 1 的日志结果，调整 `TargetMethod()` 或 `Prepare()` 逻辑。

### 步骤 4：构建并测试

运行 `dotnet build`，启动游戏，检查日志。
