![Logo](Logo.png)

# CUModFixer

[English Guide](README.md)

[GitHub](https://github.com/CNCUMC/CUModFixer) | [NexusMods](https://www.nexusmods.com/scavprototype/mods/424)

_[Casualties Unknown](https://store.steampowered.com/app/3624440/Casualties_Unknown_Demo/) 的 BepInEx
兼容性修复插件，解决与第三方模组的冲突。_

---

## 概述

**CUMod Fixer**
是一个兼容性补丁插件，用于解决 [Casualties Unknown](https://store.steampowered.com/app/3624440/Casualties_Unknown_Demo/)、[KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67)
（多人模组）与第三方模组（如 [New Firearms](https://www.nexusmods.com/scavprototype/mods/122)）之间的冲突。

## 修复

* 防止 `RshGun.MpScareCheck()` 在物品缺少 `KrokoshaScavMultiGameObjectNetworkTracker` 时抛出异常。

---

## 依赖

- [BepInEx 5.x](https://github.com/BepInEx/BepInEx)
- [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) ≥ 4.0.0

## 安装

1. 为 Casualties Unknown 安装 BepInEx 5.x。
2. 安装 [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) — 放入 `BepInEx/plugins/`。
3. 从 [Releases](https://github.com/CNCUMC/CUModFixer/releases) 下载最新版 `CUModFixer.dll`。
4. 将 `CUModFixer.dll` 放入 `BepInEx/plugins/CUModFixer`。

---

## 许可

[LGPL v3](LICENSE.md)
