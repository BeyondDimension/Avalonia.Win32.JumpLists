# Avalonia.Win32.JumpLists
System.Windows.Shell.JumpList for Avalonia  
- 从 [WPF](https://github.com/dotnet/wpf/tree/release/5.0) 中 剥离 ```System.Windows.Shell.JumpList``` 到 Avalonia 中  
- ```net5.0-windows``` 独立部署不依赖 WPF 与 WindowsForms 减少程序体积

## 常见问题
```Program.cs``` 中 Main 函数如果必须标记 ```[STAThread]``` 否则将抛出异常
