# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v1.0.2

### 变更

- **重构为 Harmony 注解模式** — 所有 fixer 类从手动 `Install(Harmony)` 迁移至 `[HarmonyPatch]` 注解，由 `PatchAll()` 自动发现
- **Plugin.cs 简化** — 移除 `OnAssemblyLoad` 事件和手动 `Install()` 调用；使用 `[BepInDependency(SoftDependency)]` + `Harmony.CreateAndPatchAll()`
- **NewFirearmsFix.cs 合并** — 4 个补丁守卫合并到一个文件，每方法独立 `[HarmonyPatch]` 属性
- **KrokoshaCasualtiesMPFix.cs 转换** — 从手动 `Install()` 改为 `[HarmonyPatch("TypeName", "MethodName")]` 注解语法
- **NewClothingFix.cs 转换** — 同上注解模式

### 修复

- **NewClothing RshClothing 日志刷屏** — `RshClothing.Update` finalizer 仅首次输出 Warning，后续 NRE 静默抑制
