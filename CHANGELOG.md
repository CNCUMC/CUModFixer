# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## v1.0.2

### Changed

- **Refactored to Harmony annotations** — all fixer classes converted from manual `Install(Harmony)` to `[HarmonyPatch]` annotations, auto-discovered by `PatchAll()`
- **Plugin.cs simplified** — removed `OnAssemblyLoad` event and manual `Install()` calls; uses `[BepInDependency(SoftDependency)]` + `Harmony.CreateAndPatchAll()`
- **NewFirearmsFix.cs consolidated** — 4 patch guards merged into single file (`NewFirearmsFix.cs`) with per-method `[HarmonyPatch]` attributes
- **KrokoshaCasualtiesMPFix.cs converted** — from manual `Install()` to `[HarmonyPatch("TypeName", "MethodName")]` annotation syntax
- **NewClothingFix.cs converted** — same annotation pattern

### Fixed

- **NewClothing RshClothing log spam** — `RshClothing.Update` finalizer now only logs the warning once; subsequent NREs are silently suppressed
