# Tower Defense Project

一个基于 Unity 的 3D 塔防游戏原型。玩家在可建造格子上选择并放置防御塔，防御塔自动锁定范围内敌人并发射投射物；敌人沿路径前进，进入攻击范围时会攻击防御塔，抵达终点后扣除玩家生命值。

## 项目特点

- 塔防核心循环：选择防御塔、消耗金币建造、自动索敌、旋转瞄准、发射子弹。
- 敌人波次系统：支持多个波次、每波多个敌人组、生成间隔和波次间隔配置。
- ScriptableObject 数据配置：防御塔和敌人的数值都通过资源文件配置，便于扩展。
- 敌人行为：沿路点移动、攻击附近防御塔、死亡奖励金币、抵达终点扣生命。
- UI 显示：金币、生命值、当前波次、终点生命条和单位血条。
- 菜单流程：主菜单、关卡选择、游戏场景之间切换。

## 目录结构

```text
Assets/Scripts
+-- Enemy/
|   +-- Enemy.cs             # 敌人移动、攻击、受伤、死亡和抵达终点逻辑
|   +-- EnemyData.cs         # 敌人配置数据
|   +-- EnemySpawner.cs      # 根据路径和敌人数据生成敌人
+-- BillboardUI.cs           # 让世界空间 UI 面向摄像机
+-- BuildTile.cs             # 建造格点击建塔逻辑
+-- EndPointHealthBar.cs     # 终点生命条
+-- GameManager.cs           # 金币、生命、波次和 UI 状态管理
+-- GameSceneMenu.cs         # 游戏场景菜单按钮
+-- LevelSelectMenu.cs       # 关卡选择菜单
+-- MainMenu.cs              # 主菜单
+-- Projectile.cs            # 投射物追踪和伤害逻辑
+-- Tower.cs                 # 防御塔索敌、旋转、射击和生命逻辑
+-- TowerBuildManager.cs     # 当前选中防御塔管理
+-- TowerData.cs             # 防御塔配置数据
+-- WaveManager.cs           # 波次生成和难度成长
```

## 运行环境

- Unity 2022 或更新版本建议使用。
- 使用 TextMeshPro 显示金币、生命和波次 UI。
- 项目脚本使用 C# 和 Unity MonoBehaviour 生命周期。

## 场景说明

当前脚本中使用了以下场景名称：

- `MainMenuScene`：主菜单场景。
- `LevelSelectScene`：关卡选择场景。
- `TowerDefenseScene`：塔防游戏主场景。

请在 Unity 的 `File > Build Settings` 中将这些场景加入 Scenes In Build，并确保场景名称与脚本中的字符串一致。

## 快速开始

1. 打开 Unity 项目。
2. 在 Build Settings 中添加 `MainMenuScene`、`LevelSelectScene` 和 `TowerDefenseScene`。
3. 在主游戏场景中创建并配置 `GameManager`：
   - 绑定金币、生命、波次的 TextMeshPro 文本。
   - 设置初始金币 `gold` 和最大生命 `maxLives`。
4. 创建 `TowerBuildManager`，用于保存当前选择的防御塔数据。
5. 创建防御塔数据：
   - 在 Project 窗口右键选择 `Create > Tower Defense > Tower Data`。
   - 配置防御塔 Prefab、消耗、攻击范围、射速、伤害、子弹速度和生命值。
6. 创建敌人数据：
   - 在 Project 窗口右键选择 `Create > Tower Defense > Enemy Data`。
   - 配置敌人 Prefab、生命、速度、金币奖励、攻击参数和抵达终点造成的伤害。
7. 在场景中放置 `EnemySpawner`，并配置路径点 `waypoints`。
8. 在场景中放置 `WaveManager`，绑定 `EnemySpawner`，配置波次数组。
9. 给可建造位置添加 `BuildTile` 和碰撞体，点击后会根据当前选中的 `TowerData` 建造防御塔。
10. 运行游戏，从菜单进入关卡或直接运行 `TowerDefenseScene` 测试。

## 核心系统

### 防御塔建造

`TowerBuildManager` 负责保存当前选中的 `TowerData`。当玩家点击带有 `BuildTile` 的格子时，系统会检查格子是否已有防御塔、是否已选择塔、金币是否足够，然后实例化对应防御塔 Prefab。

### 防御塔战斗

`Tower` 会在每帧查找攻击范围内最近的敌人，水平旋转并瞄准目标。只有当角度误差小于 `aimTolerance` 时才会开火。投射物由 `Projectile` 负责追踪目标并造成伤害。

### 敌人行为

`Enemy` 通过 `EnemyData` 初始化数值，并沿 `EnemySpawner` 提供的路径点移动。敌人会优先攻击攻击范围内的防御塔；如果没有目标，则继续沿路径前进。死亡时奖励金币，抵达终点时扣除玩家生命。

### 波次管理

`WaveManager` 支持在 Inspector 中配置多个波次。每个波次包含若干敌人组，每组可以指定敌人类型、数量和生成间隔。随着波次推进，可以通过 `healthGrowthPerWave` 和 `speedGrowthPerWave` 增加敌人难度。

## Inspector 配置重点

- `TowerData.prefab` 必须绑定带有 `Tower` 脚本的防御塔 Prefab。
- `Tower.projectilePrefab` 必须绑定带有 `Projectile` 脚本的投射物 Prefab。
- `Tower.firePoint` 可选；不绑定时会从防御塔自身位置发射。
- `EnemyData.prefab` 必须绑定带有 `Enemy` 脚本的敌人 Prefab。
- `EnemySpawner.waypoints` 至少需要一个路径点。
- `WaveManager.enemySpawner` 必须绑定场景中的 `EnemySpawner`。
- `GameManager` 建议在游戏主场景中保持唯一实例。
- 需要给 `BuildTile` 所在物体添加 Collider，才能响应 `OnMouseDown`。

## 操作流程

1. 在 UI 中调用 `TowerBuildManager.SelectTower(TowerData towerData)` 选择要建造的防御塔。
2. 点击场景中的建造格。
3. 防御塔消耗金币并生成。
4. 敌人按波次生成并沿路径前进。
5. 防御塔自动攻击敌人，敌人死亡奖励金币。
6. 敌人抵达终点会扣除生命，生命归零时游戏暂停并判定失败。

## 可扩展方向

- 增加防御塔升级、出售和拆除功能。
- 实现 `TowerEffectType` 中的减速、灼烧和范围伤害效果。
- 增加胜利/失败 UI 面板。
- 增加更多关卡和敌人路径。
- 增加音效、命中特效和死亡特效。
- 将波次完成和游戏胜利事件暴露给 UI 或关卡管理器。

## 已知注意事项

- 部分脚本中的中文调试日志可能存在编码显示异常，建议统一保存为 UTF-8。
- 当前防御塔索敌和敌人找塔使用 `FindObjectsByType`，敌人和防御塔数量较多时可以改为集中注册管理以提升性能。
- `TowerData.effectType` 已预留效果类型，但当前投射物命中逻辑只处理直接伤害。
