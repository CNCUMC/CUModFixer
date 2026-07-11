# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v1.0.1

### 新增

- **NewFirearms IsOnBack 修复** — 补丁 `RshGun.IsOnBack()` 使用 Harmony finalizer 抑制 `NetPlayer.GetNetPlayerFromBody(body)` 返回 `null` 时的空引用异常
    - 仅首次打印日志警告，后续异常静默抑制

- **NewFirearms HandleLegacyGunUi 修复** — 补丁 `PlayerCameraPatch1.HandleLegacyGunUi()` 使用 Harmony finalizer 抑制 `__instance.body` 为 `null` 时的空引用异常

- **NewFirearms 眩晕碰撞体日志抑制** — 抑制 `SpiderHandlerPatch.Postfix` 中重复的 `Debug.LogWarning("[NewFirearms] Can not add stun collider...")`；仅首次输出

- **NewClothing RshClothing 修复** — 补丁 `RshClothing.Update()` 使用 Harmony finalizer 抑制 `this.it` 为 `null` 时的空引用异常

- **Krokosha GunScriptTrackerComponent 修复** — 补丁 `KrokoshaGunScriptTrackerComponent.Update()` 使用 Harmony finalizer 抑制 `PlayerCamera.main.body` 为 `null` 时的空引用异常

### 变更

- **项目结构** — fixer 类从 `Plugin.cs` 拆分到 `Fixers/NewFirearmsFix.cs`、`Fixers/NewClothingFix.cs` 和 `Fixers/KrokoshaCasualtiesMPFix.cs`
