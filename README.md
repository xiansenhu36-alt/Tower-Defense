TowerDefense Game
# Tower Defense
一个使用 Unity 制作的 3D 塔防游戏项目。玩家可以从主菜单进入关卡选择界面，选择关卡后进入塔防场景，通过消耗金币建造不同类型的防御塔，阻止敌人沿路径到达终点。
## 项目功能
+- 主菜单界面：开始游戏并进入关卡选择场景
+- 关卡选择界面：选择关卡或返回主菜单
+- 游戏场景菜单：从战斗场景返回关卡选择或主菜单
+- 建塔系统：选择防御塔后，在可建造地块上消耗金币生成防御塔
+- 防御塔系统：自动寻找范围内最近的敌人，旋转瞄准并发射子弹
+- 敌人系统：敌人沿路径点移动，到达终点后扣除玩家生命
+- 敌人攻击：敌人接近防御塔后会停止移动并攻击防御塔
+- 波次系统：按配置生成多波敌人，并随波次提升生命值和速度
+- 资源与生命 UI：显示金币、生命值和当前波次
+- 血条 UI：敌人、防御塔和终点生命显示
+## 场景结构
+项目主要包含以下场景：
+| 场景 | 用途 |
+| --- | --- |
+| `MainMenuScene` | 游戏主菜单 |
+| `LevelSelectScene` | 关卡选择界面 |
+| `TowerDefenseScene` | 核心塔防战斗场景 |
+
+建议在 Unity 的 Build Profiles / Scene List 中按以下顺序添加：
+```text
+0 MainMenuScene
+1 LevelSelectScene
+2 TowerDefenseScene
+```
+## 核心脚本
+| 脚本 | 说明 |
+| --- | --- |
+| `MainMenu.cs` | 主菜单按钮逻辑，进入关卡选择场景 |
+| `LevelSelectMenu.cs` | 关卡选择按钮逻辑，进入战斗场景或返回主菜单 |
+| `GameSceneMenu.cs` | 战斗场景中的返回按钮逻辑 |
+| `GameManager.cs` | 管理金币、生命值、波次和 UI 更新 |
+| `BuildTile.cs` | 可建造地块逻辑，负责生成防御塔 |
+| `TowerBuildManager.cs` | 保存当前选择的防御塔数据 |
+| `TowerData.cs` | 防御塔配置数据，包括价格、伤害、射速、血量等 |
+| `Tower.cs` | 防御塔攻击、瞄准、血量和发射逻辑 |
+| `Projectile.cs` | 子弹追踪目标并造成伤害 |
+| `WaveManager.cs` | 敌人波次生成和难度成长 |
+| `EnemySpawner.cs` | 根据敌人数据和路径点生成敌人 |
+| `Enemy.cs` | 敌人移动、攻击、防御塔交互、死亡和到达终点逻辑 |
+| `EnemyData.cs` | 敌人配置数据，包括生命、速度、奖励、伤害等 |
+| `BillboardUI.cs` | 让世界空间 UI 始终朝向摄像机 |
+| `EndPointHealthBar.cs` | 根据玩家生命值更新终点血条 |
+## 如何运行
+1. 使用 Unity 6.3 LTS 或兼容版本打开项目。
+2. 打开 `MainMenuScene`。
+3. 确认 Build Profiles 的 Scene List 中包含主菜单、关卡选择和战斗场景。
+4. 点击 Unity 顶部的 Play 按钮运行。
+5. 从主菜单点击 Start Game，进入关卡选择并开始游戏。
+## 操作方式
+- 在主菜单点击 Start Game 进入关卡选择。
+- 在关卡选择界面点击关卡按钮进入战斗。
+- 在战斗场景中选择防御塔类型。
+- 点击地图上的建造地块放置防御塔。
+- 防御塔会自动攻击进入范围的敌人。
+- 敌人到达终点会扣除生命值，生命值归零后游戏失败。
+## 项目目录
```text
Assets/
  Scripts/
    Enemy/
    GameManager.cs
    WaveManager.cs
    Tower.cs
    TowerData.cs
    TowerBuildManager.cs
    BuildTile.cs
    Projectile.cs
    MainMenu.cs
    LevelSelectMenu.cs
    GameSceneMenu.cs
+Packages/
+ProjectSettings/
+```
+## GitHub 上传注意事项
+
+Unity 项目上传 GitHub 时建议提交：
+
+```text
+Assets/
+Packages/
+ProjectSettings/
+.gitignore
+README.md
+```
+
+不要提交以下自动生成或本地缓存目录：
+
+```text
+Library/
+Temp/
+Obj/
+Logs/
+Build/
+Builds/
+UserSettings/
+```
+
+本项目建议使用 Unity 类型的 `.gitignore`。
+
+## 当前状态
+
+项目已完成基础塔防玩法框架，包括菜单切换、关卡进入、建塔、敌人移动、波次生成、防御塔攻击、金币和生命值系统。后续可以继续扩展更多关卡、更多敌人类型、防御塔升级、胜利结算界面和音效系统。
