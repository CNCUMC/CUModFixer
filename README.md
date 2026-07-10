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

| Fix                          | Description                                                                                                                       |
|------------------------------|-----------------------------------------------------------------------------------------------------------------------------------|
| **NewFirearms MpScareCheck** | Prevents `RshGun.MpScareCheck()` from throwing exceptions when `KrokoshaScavMultiGameObjectNetworkTracker` is missing on the item |

---

## Requirements

- [BepInEx 5.x](https://github.com/BepInEx/BepInEx)
- [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) ≥ 4.0.0
- [New Firearms](https://www.nexusmods.com/scavprototype/mods/122) ≥ 1.5.0

## Installation

1. Install BepInEx 5.x for Casualties Unknown.
2. Install [KrokoshaCasualtiesMP](https://www.nexusmods.com/scavprototype/mods/67) — place into `BepInEx/plugins/`.
3. Install [New Firearms](https://www.nexusmods.com/scavprototype/mods/122) — place into `BepInEx/plugins/`.
4. Download the latest `CUModFixer.dll` from [Releases](https://github.com/CNCUMC/CUModFixer/releases).
5. Place `CUModFixer.dll` into `BepInEx/plugins/`.

---

## How It Works

### NewFirearms MpScareCheck Fix

The `RshGun.MpScareCheck()` method in New Firearms calls `GetMpTracker()` which expects a
`KrokoshaScavMultiGameObjectNetworkTracker` component to be present on the item's GameObject. During world generation,
some items may not yet have this component attached, causing repeated exceptions.

This plugin applies a Harmony patch with:

1. **Prefix** — Uses reflection to check if the tracker component exists before allowing the original method to run
2. **Finalizer** — Catches and suppresses any exceptions that still occur (defense in depth)

The patch is installed:

- Immediately on `Awake()` if NewFirearms is already loaded
- Via `AssemblyLoad` event if NewFirearms loads after this plugin


---

## License

[LGPL v3](LICENSE.md)
