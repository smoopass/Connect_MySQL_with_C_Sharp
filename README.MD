# Connect_MySQL_with_C_Sharp
## 一、程序介绍
本系统基于前期自行设计的数据库——超市管理系统进行开发，用C#编写可视化程序与数据库进行连接，实现基本的显示指定数据表、增加记录、删除记录、修改记录、查询记录的功能。由于需要连接的数据库、需要操作的数据表均可自定义，因此换一个数据库本程序依然可以正常使用。
## 二、测试过程
### 1. 连接
输入正确的信息，连接成功，弹窗提示
![](Connect_MySQL_with_C_Sharp/img/1.png)

输入错误的信息，连接失败，弹窗提示
![](Connect_MySQL_with_C_Sharp/img/2.png)

### 2. 显示数据表
数据表名称可用，将显示表中所有内容，并自动根据字符串的长度调整列宽
![](Connect_MySQL_with_C_Sharp/img/3.png)

数据表名称不可用，抛出异常
![](Connect_MySQL_with_C_Sharp/img/4.png)

### 3. 增加记录
数据符合要求，弹窗提示插入成功，再次获取数据表即可看到新插入的记录，表中有几个字段就插入几项，多余数据项自动忽略不计
![](Connect_MySQL_with_C_Sharp/img/5.png)

数据类型不符合要求，抛出异常
![](Connect_MySQL_with_C_Sharp/img/6.png)

### 4. 删除记录
输入字段名和索引值，符合条件的记录将被删除，弹窗提示
![](Connect_MySQL_with_C_Sharp/img/7.png)

字段名有误等原因，抛出异常
![](Connect_MySQL_with_C_Sharp/img/8.png)

### 5. 修改记录
输入要修改的字段名、要替换的值、新的值，符合要求，弹窗提示
![](Connect_MySQL_with_C_Sharp/img/9.png)

字段名、数据类型有误等原因，抛出异常
![](Connect_MySQL_with_C_Sharp/img/10.png)

### 6. 查找记录
输入查询后要导出的字段名，用英文逗号分割，再输入查询的条件，无误后将在下方显示查询到的记录
![](Connect_MySQL_with_C_Sharp/img/11.png)
![](Connect_MySQL_with_C_Sharp/img/12.png)

查询条件格式等有误，抛出异常
![](Connect_MySQL_with_C_Sharp/img/13.png)

## 三、可改进的方面
1. 出于测试方便将所有功能集中在同一个页面，可以考虑每个功能分不同个页面，做到首页简洁
2. 插入记录功能字段数有限制，该部分需要重新考虑实现逻辑
3. 删除、修改功能有限制，只能实现最基本的操作，可以完善条件语句部分
