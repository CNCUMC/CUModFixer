# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v1.0.0

### 新增

- **NewFirearms MpScareCheck 守卫** — 补丁 `RshGun.MpScareCheck()` 防止在 `KrokoshaScavMultiGameObjectNetworkTracker` 缺失时抛出异常
    - 反射前缀检查 — 当网络追踪器组件不存在时跳过执行
    - 终结器回退 — 捕获并抑制任何残留异常
    - 若 NewFirearms 已加载则在 `Awake()` 时立即安装
    - AssemblyLoad 事件处理器用于延迟加载场景
