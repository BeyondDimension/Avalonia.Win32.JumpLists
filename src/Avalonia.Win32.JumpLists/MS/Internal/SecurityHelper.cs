// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/***************************************************************************\
*
* Purpose:  Helper functions that require elevation but are safe to use.
*
\***************************************************************************/

// The SecurityHelper class differs between assemblies and could not actually be
//  shared, so it is duplicated across namespaces to prevent name collision.
// This duplication seems hardly necessary now. We should continue
// trying to reduce it by pushing things from Framework to Core (whenever it makes sense).

using Microsoft.Win32;
using System.Runtime.Versioning;

namespace MS.Internal.WindowsBase;

// This existed originally to allow FontCache service to
// see the WindowsBase variant of this class. We no longer have
// a FontCache service, but over time other parts of WPF might
// have started to depend on this, so we leave it as-is for
// compat.
[FriendAccessAllowed]
[SupportedOSPlatform("Windows")]
internal static class SecurityHelper
{
    ///
    /// Read and return a registry value.
    static internal object ReadRegistryValue(RegistryKey baseRegistryKey, string keyName, string valueName)
    {
        object value = null;

        RegistryKey key = baseRegistryKey.OpenSubKey(keyName);
        if (key != null)
        {
            using (key)
            {
                value = key.GetValue(valueName);
            }
        }

        return value;
    }
}