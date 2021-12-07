// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MS.Win32
{
    using MS.Internal.WindowsBase;
    using System.Runtime.InteropServices;

    [FriendAccessAllowed]
    internal partial class SafeNativeMethods
    {
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool IsDebuggerPresent();
    }
}