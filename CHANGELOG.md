# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## v1.0.1

### Added

- **NewFirearms IsOnBack fix** — patches `RshGun.IsOnBack()` with a Harmony finalizer to suppress `NullReferenceException` when `NetPlayer.GetNetPlayerFromBody(body)` returns `null` during world generation
    - Only logs the warning once; subsequent exceptions are silently suppressed
- **NewFirearms HandleLegacyGunUi fix** — patches `PlayerCameraPatch1.HandleLegacyGunUi()` with a Harmony finalizer to suppress `NullReferenceException` when `__instance.body` is `null` during world generation
- **NewFirearms stun collider log spam** — suppresses repeated `Debug.LogWarning("[NewFirearms] Can not add stun collider...")` from `SpiderHandlerPatch.Postfix`; only the first occurrence is logged
- **NewClothing RshClothing fix** — patches `RshClothing.Update()` with a Harmony finalizer to suppress `NullReferenceException` when `this.it` is `null`
- **Krokosha GunScriptTrackerComponent fix** — patches `KrokoshaGunScriptTrackerComponent.Update()` with a Harmony finalizer to suppress `NullReferenceException` when `PlayerCamera.main.body` is `null`

### Changed

- **Project structure** — fixer classes extracted from `Plugin.cs` into `Fixers/NewFirearmsFix.cs`, `Fixers/NewClothingFix.cs`, and `Fixers/KrokoshaCasualtiesMPFix.cs`
