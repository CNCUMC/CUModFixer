# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v1.1.2

### 修复

- 添加 `InitFinalizer` 捕获 `RshClothing.Init()` 中的 `IndexOutOfRangeException`（`secondryLimbsTexture` 数组长度不匹配），带 `_initWarned` 标志位防止日志刷屏。

