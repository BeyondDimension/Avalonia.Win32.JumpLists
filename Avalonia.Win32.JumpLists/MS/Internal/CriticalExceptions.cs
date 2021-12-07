// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MS.Internal.WindowsBase;
using System;

namespace MS.Internal
{
    [FriendAccessAllowed]
    internal static class CriticalExceptions
    {
        // these are exceptions that we should treat as critical when they
        // arise during callbacks into application code
        [FriendAccessAllowed]
        internal static bool IsCriticalApplicationException(Exception ex)
        {
            ex = Unwrap(ex);

            return ex is StackOverflowException ||
                   ex is OutOfMemoryException ||
                   ex is System.Threading.ThreadAbortException ||
                   ex is System.Security.SecurityException;
        }

        [FriendAccessAllowed]
        internal static Exception Unwrap(Exception ex)
        {
            // for certain types of exceptions, we care more about the inner
            // exception
            while (ex.InnerException != null &&
                    (ex is System.Reflection.TargetInvocationException
                    ))
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}