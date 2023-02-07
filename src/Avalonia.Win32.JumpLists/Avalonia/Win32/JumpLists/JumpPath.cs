// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETFRAMEWORK
namespace Avalonia.Win32.JumpLists;

public class JumpPath : JumpItem
{
    public JumpPath() : base()
    { }

    public string Path { get; set; }
}
#endif