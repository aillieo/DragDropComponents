基于UGUI的拖拽组件系统，两个组件`DragDropItem`和`DragDropTarget`，主要支持以下功能：

1. Item的自由拖拽及与Target的附着；
2. Item和Target响应Enter、Exit、Attach、Detach等事件，并通过代理为其添加回调；
3. 可配合LuaFramework使用，用于添加Lua回调；
3. 使用`matchingChannel`来限制那些item和Target的匹配通道，并支持万能匹配`universalMatching`；
4. 使用`matchingTag`来识别不同的目标；
5. 支持点击或长按Item使其从Target脱离；
6. 除拖放之外，Item还支持响应点击事件，并兼容SrollRect的拖动事件；
7. 可配置Target最大可附着的Item数量，以及当超出数量时是否替换掉最早附着的Item

用法详见工程中附带的示例，场景`Scene`及脚本`TestScript`。

![test.gif](test.gif)
