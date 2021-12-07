// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MS.Win32
{
    using MS.Internal.Interop;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;

    internal partial class UnsafeNativeMethods
    {
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleFileName", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int IntGetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

        internal static string GetModuleFileName(HandleRef hModule)
        {
            // .Net is currently far behind Windows with regard to supporting paths longer than MAX_PATH.
            // At one point it was tested trying to load UNC paths longer than MAX_PATH and mscorlib threw
            // FileIOExceptions before WPF was even on the stack.
            // All the same, we still want to have this grow-and-retry logic because the CLR can be hosted
            // in a native application.  Callers bothering to use this rather than Assembly based reflection
            // are likely doing so because of (at least the potential for) the returned name referring to a
            // native module.
            StringBuilder buffer = new StringBuilder(Win32Constant.MAX_PATH);
            while (true)
            {
                int size = IntGetModuleFileName(hModule, buffer, buffer.Capacity);
                if (size == 0)
                {
                    throw new Win32Exception();
                }

                // GetModuleFileName returns nSize when it's truncated but does NOT set the last error.
                // MSDN documentation says this has changed in Windows 2000+.
                if (size == buffer.Capacity)
                {
                    // Enlarge the buffer and try again.
                    buffer.EnsureCapacity(buffer.Capacity * 2);
                    continue;
                }

                return buffer.ToString();
            }
        }
    }
}