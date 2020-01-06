基于UGUI的拖拽组件系统，主要支持以下功能：

1. **Item**的自由拖拽及与**Target**的附着；
2. **Item**和**Target**响应Enter、Exit、Attach、Detach等事件，并通过代理为其添加回调；
3. 使用`matchingChannel`来限制只有特定通道的**Item**和**Target**可以匹配；
4. 使用`matchingTag`来识别不同的目标类别；
5. 支持点击或长按**Item**使其从**Target**脱离；
6. 除拖放之外，**Item**还支持响应点击事件，并兼容`ScrollRect`的拖动事件；

用法详见工程中附带的示例，场景`Scene`及脚本`TestScript`。

![test.gif](test.gif)


---



A Drag&Drop components system based on UGUI that has the following features:

1. Drag **Item**s freely and attach them to **Target**s;
2. **Item**s and **Target**s can receive callback events like Enter, Exit, Attach, Detach etc;
3. Use `matchingChannel` to make sure that only **Item**s and **Target**s with exact channels can match;
4. Use `matchingTag` to identify different **Item** or **Target** types;
5. Detach with long press is supported;
6. **Item**s can also receive click events and are able to work with drag events of a `ScrollRect` as well;

For more details please run `Scene` or view `TestScript`.

