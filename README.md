![Logo](Logo.png)

# CUMod Fixer

[中文指南](README_ZH.md)

[GitHub](https://github.com/CNCUMC/CUModFixer) | [NexusMods](https://www.nexusmods.com/scavprototype/mods/424)

_A BepInEx plugin for [Casualties Unknown](https://store.steampowered.com/app/3624440/Casualties_Unknown_Demo/) that fixes compatibility issues with third-party mods._

---

## Overview

**CUMod Fixer** is a compatibility patch plugin that resolves conflicts
between [Casualties Unknown](https://store.steampowered.com/app/3624440/Casualties_Unknown_Demo/), [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) (
multiplayer mod), and third-party mods like [New Firearms](https://www.nexusmods.com/scavprototype/mods/122).

## Fix

* Prevents `RshGun.MpScareCheck()` from throwing exceptions when `KrokoshaScavMultiGameObjectNetworkTracker` is missing on the item.

---

## Requirements

- [BepInEx 5.x](https://github.com/BepInEx/BepInEx)
- [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) ≥ 4.0.0

## Installation

1. Install BepInEx 5.x for Casualties Unknown.
2. Install [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) — place into `BepInEx/plugins/`.
3. Download the latest `CUModFixer.dll` from [Releases](https://github.com/CNCUMC/CUModFixer/releases).
4. Place `CUModFixer.dll` into `BepInEx/plugins/CUModFixer`.

---

## License

[LGPL v3](LICENSE.md)
