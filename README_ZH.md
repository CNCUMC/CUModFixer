![Logo](Logo.png)

# CUMod Fixer

[English Guide](README.md)

[GitHub](https://github.com/CNCUMC/CUModFixer) | [NexusMods](https://www.nexusmods.com/scavprototype/mods/424)

_[Casualties Unknown](https://store.steampowered.com/app/3624440/Casualties_Unknown_Demo/) 的 BepInEx 兼容性修复插件，解决与第三方模组的冲突。_

---

## 概述

**CUMod Fixer** 是一个兼容性补丁插件，用于解决 [Casualties Unknown](https://store.steampowered.com/app/3624440/Casualties_Unknown_Demo/)、[KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67)（多人模组）与第三方模组（如 [New Firearms](https://www.nexusmods.com/scavprototype/mods/122)）之间的冲突。

| 修复项                          | 说明                                                                                 |
|------------------------------|------------------------------------------------------------------------------------|
| **NewFirearms MpScareCheck** | 防止 `RshGun.MpScareCheck()` 在物品缺少 `KrokoshaScavMultiGameObjectNetworkTracker` 时抛出异常 |

---

## 依赖

- [BepInEx 5.x](https://github.com/BepInEx/BepInEx)
- [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) ≥ 4.0.0
- [New Firearms](https://www.nexusmods.com/scavprototype/mods/122) ≥ 1.5.0

## 安装

1. 为 Casualties Unknown 安装 BepInEx 5.x。
2. 安装 [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) — 放入 `BepInEx/plugins/`。
3. 安装 [New Firearms](https://www.nexusmods.com/scavprototype/mods/122) — 放入 `BepInEx/plugins/`。
4. 从 [Releases](https://github.com/CNCUMC/CUModFixer/releases) 下载最新版 `CUModFixer.dll`。
5. 将 `CUModFixer.dll` 放入 `BepInEx/plugins/`。

---

## 工作原理

### NewFirearms MpScareCheck 修复

New Firearms 中的 `RshGun.MpScareCheck()` 方法会调用 `GetMpTracker()`，该方法期望物品的 GameObject 上存在 `KrokoshaScavMultiGameObjectNetworkTracker` 组件。在世界生成阶段，某些物品可能尚未附加此组件，导致反复抛出异常。

本插件通过 Harmony 补丁实现：

1. **Prefix（前缀）** — 使用反射检查追踪器组件是否存在，若不存在则跳过原方法执行
2. **Finalizer（终结器）** — 捕获并抑制任何仍然发生的异常（深度防御）

补丁安装时机：

- 若 NewFirearms 已加载，在 `Awake()` 时立即安装
- 若 NewFirearms 之后加载，通过 `AssemblyLoad` 事件安装

---

## 许可

[LGPL v3](LICENSE.md)
