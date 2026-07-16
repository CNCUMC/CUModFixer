![Logo](Logo.png)

# CUModFixer

[English Guide](README.md)

[GitHub](https://github.com/CNCUMC/CUModFixer) | [NexusMods](https://www.nexusmods.com/scavprototype/mods/424)

_[Casualties Unknown](https://store.steampowered.com/app/4576490/) 的 BepInEx 兼容性修复插件，解决与第三方模组的冲突。_

## 概述

**CUMod Fixer**
是一个兼容性补丁插件，用于解决 [Casualties Unknown](https://store.steampowered.com/app/4576490/)、[KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67)
（多人模组）与第三方模组（如 [New Firearms](https://www.nexusmods.com/scavprototype/mods/122)
和 [New Clothing](https://www.nexusmods.com/scavprototype/mods/122)）之间的冲突。

## 修复

### 原版游戏

- 防止 `BuildingEntity.Update()` 在 `Resources.Load("DustBig")` 返回 `null` 时抛出 `ArgumentException`。

### New Firearms

- 防止 `RshGun.MpScareCheck()` 在物品缺少 `KrokoshaScavMultiGameObjectNetworkTracker` 时抛出异常。
- 防止 `RshGun.IsOnBack()` 在世界生成阶段因 `NetPlayer.GetNetPlayerFromBody(body)` 返回 `null` 而触发空引用异常。
- 防止 `PlayerCameraPatch1.HandleLegacyGunUi()` 在世界生成阶段因 `PlayerCamera.body` 为 `null` 而触发空引用异常。
- 抑制重复的 "[NewFirearms] Can not add stun collider to spider" 日志警告（仅首次输出）。

### New Clothing

- 防止 `RshClothing.Update()` 因 `this.it` 为 `null` 而触发空引用异常。

### KrokoshaCasualtiesMP

- 防止 `KrokoshaGunScriptTrackerComponent.Update()` 在 `PlayerCamera.main.body` 为 `null` 时触发空引用异常。

## 许可

[LGPL v3](LICENSE.md)
