// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETFRAMEWORK
namespace Avalonia.Win32.JumpLists;

public class JumpTask : JumpItem
{
    public JumpTask() : base()
    { }

    public string Title { get; set; }

    public string Description { get; set; }

    public string ApplicationPath { get; set; }

    public string Arguments { get; set; }

    public string WorkingDirectory { get; set; }

    public string IconResourcePath { get; set; }

    public int IconResourceIndex { get; set; }
}
#endif