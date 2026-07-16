# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres
to [Semantic Versioning](https://semver.org/).

---

## v1.1.0

### Added

- Added `BuildingEntityFix`: Fixes `BuildingEntity.Update()` throwing `ArgumentException` when
  `Resources.Load("DustBig")` returns `null`.

### Changed

- Refactored all Fixers to use **Prefix interception** pattern, preventing exceptions at the source instead of catching
  them after the fact.
- Extracted `FixerHelper` utility class to centralize type resolution, patch installation, and exception suppression.
- Switched to `Chainloader.PluginInfos` GUID matching for identifying installed mods.

### Fixed

- `RshClothing.Update()` NullReferenceException (`this.it` is null)
- `KrokoshaGunScriptTrackerComponent.Update()` NullReferenceException (`PlayerCamera.main.body` is null)
- `RshGun.MpScareCheck()` NullReferenceException
- `RshGun.IsOnBack()` NullReferenceException
- `PlayerCameraPatch1.HandleLegacyGunUi()` NullReferenceException

