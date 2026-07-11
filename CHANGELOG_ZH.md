# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v1.0.2

### 修复

- **NewClothing RshClothing 日志刷屏** — `RshClothing.Update` finalizer 仅首次输出 Warning，后续 NRE 静默抑制
