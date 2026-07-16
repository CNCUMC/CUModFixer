# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres
to [Semantic Versioning](https://semver.org/).

---

## v1.1.1

### Changed

- Renamed `BuildingEntityFix` to `GameFix`, dedicated to vanilla game fixes.
- Renamed `Finalizer` method to `BuildingEntityUpdateFinalizer` for better readability.
- Removed explicit `Install()` call for `GameFix`; now handled automatically by `[HarmonyPatch]` attribute via `PatchAll()`.

### Fixed

- Added `_warned` flag to `BuildingEntityUpdateFinalizer` to prevent log spam (only outputs once).
- Fixed `GameFix` missing `[HarmonyFinalizer]` attribute, causing the Finalizer to not be recognized.

