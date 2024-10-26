# 简介
该项目跟着 https://github.com/GuoYaxiang/craftinginterpreters_zh/blob/main/content/8.%E8%A1%A8%E8%BE%BE%E5%BC%8F%E5%92%8C%E7%8A%B6%E6%80%81.md#user-content-fnref-12-5b6bc3e33dd4ef84d3f46ed83b5f398c 学习编写

# 支持语法
绝大部分都与C语法相似。

目前支持以下内容：
1. 基础的逻辑与预算：+ - * /,且包括括号的嵌套
2. 变量的声明,语法如下
```
var a = 1;
```
3. 基础的控制流，for循环，while循环，if语句。但目前不支持break与Continue
4. 支持函数的声明，但是不支持函数的嵌套，（非要嵌套也行，只不过有问题）

# todo（但是大概率不会继续了）
1. 支持数组的调用
2. 支持类或者结构体作为数据载体
3. 支持Continue与break
