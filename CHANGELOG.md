# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## v1.0.0

### Added

- **NewFirearms MpScareCheck guard** — patches `RshGun.MpScareCheck()` to prevent exceptions when `KrokoshaScavMultiGameObjectNetworkTracker` is missing
    - Prefix check via reflection — skips execution if the network tracker component is not present
    - Finalizer fallback — catches and suppresses any remaining exceptions
    - Immediate installation on `Awake()` if NewFirearms is already loaded
    - AssemblyLoad event handler for deferred loading scenarios
