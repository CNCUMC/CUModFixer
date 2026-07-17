# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres
to [Semantic Versioning](https://semver.org/).

---

## v1.1.2

### Fixed

- Added `InitFinalizer` to catch `IndexOutOfRangeException` in `RshClothing.Init()` (caused by `secondryLimbsTexture` array length mismatch), with `_initWarned` flag to prevent log spam.

