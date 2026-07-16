# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v1.1.1

### 变更

- 将 `BuildingEntityFix` 重命名为 `GameFix`，专门用于放置原版游戏的修复。
- 将 `Finalizer` 方法重命名为 `BuildingEntityUpdateFinalizer`，提高可读性。
- 移除 `GameFix` 的显式 `Install()` 调用，由 `[HarmonyPatch]` 属性通过 `PatchAll()` 自动处理。

### 修复

- 为 `BuildingEntityUpdateFinalizer` 添加 `_warned` 标志位，防止警告日志刷屏（仅首次输出）。
- 修复 `GameFix` 缺少 `[HarmonyFinalizer]` 属性导致 Finalizer 未被识别的问题。

