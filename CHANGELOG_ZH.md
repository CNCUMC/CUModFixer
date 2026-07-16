# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v1.1.0

### 新增

- 添加 `BuildingEntityFix`：修复原版 `BuildingEntity.Update()` 在 `Resources.Load("DustBig")` 返回 `null` 时抛出 `ArgumentException` 的问题。

### 变更

- 重构所有 Fixer 使用 **Prefix 拦截**模式，从源头阻止异常发生，而非事后捕获。
- 提取 `FixerHelper` 工具类，统一处理类型定位、补丁安装和异常抑制。
- 改用 `Chainloader.PluginInfos` GUID 匹配识别已安装模组。

### 修复

- `RshClothing.Update()` 空引用异常（`this.it` 为 null）
- `KrokoshaGunScriptTrackerComponent.Update()` 空引用异常（`PlayerCamera.main.body` 为 null）
- `RshGun.MpScareCheck()` 空引用异常
- `RshGun.IsOnBack()` 空引用异常
- `PlayerCameraPatch1.HandleLegacyGunUi()` 空引用异常

