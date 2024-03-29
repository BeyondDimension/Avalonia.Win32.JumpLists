﻿// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.Versioning;

namespace MS.Internal;

[SupportedOSPlatform("Windows")]
static internal partial class TraceShell
{
    private static readonly AvTrace _avTrace = new(
            delegate () { return PresentationTraceSources.ShellSource; },
            delegate () { PresentationTraceSources._ShellSource = null; }
            );

    static AvTraceDetails _NotOnWindows7;
    static public AvTraceDetails NotOnWindows7
    {
        get
        {
            _NotOnWindows7 ??= new AvTraceDetails(1, new string[] { "Shell integration features are not being applied because the host OS does not support the feature." });

            return _NotOnWindows7;
        }
    }

    static AvTraceDetails _ExplorerTaskbarTimeout;
    static public AvTraceDetails ExplorerTaskbarTimeout
    {
        get
        {
            _ExplorerTaskbarTimeout ??= new AvTraceDetails(2, new string[] { "Communication with Explorer timed out while trying to update the taskbar item for the window." });

            return _ExplorerTaskbarTimeout;
        }
    }

    static AvTraceDetails _ExplorerTaskbarRetrying;
    static public AvTraceDetails ExplorerTaskbarRetrying
    {
        get
        {
            _ExplorerTaskbarRetrying ??= new AvTraceDetails(3, new string[] { "Making another attempt to update the taskbar." });

            return _ExplorerTaskbarRetrying;
        }
    }

    static AvTraceDetails _ExplorerTaskbarNotRunning;
    static public AvTraceDetails ExplorerTaskbarNotRunning
    {
        get
        {
            _ExplorerTaskbarNotRunning ??= new AvTraceDetails(4, new string[] { "Halting attempts at Shell integration with the taskbar because it appears that Explorer is not running." });

            return _ExplorerTaskbarNotRunning;
        }
    }

    static AvTraceDetails _NativeTaskbarError;
    static public AvTraceDetails NativeTaskbarError(params object[] args)
    {
        _NativeTaskbarError ??= new AvTraceDetails(5, new string[] { "The native ITaskbarList3 interface failed a method call with error {0}." });
        return new AvTraceFormat(_NativeTaskbarError, args);
    }

    static AvTraceDetails _RejectingJumpItemsBecauseCatastrophicFailure;
    static public AvTraceDetails RejectingJumpItemsBecauseCatastrophicFailure
    {
        get
        {
            _RejectingJumpItemsBecauseCatastrophicFailure ??= new AvTraceDetails(6, new string[] { "Failed to apply items to the JumpList because the native interfaces failed." });

            return _RejectingJumpItemsBecauseCatastrophicFailure;
        }
    }

    static AvTraceDetails _RejectingJumpListCategoryBecauseNoRegisteredHandler;
    static public AvTraceDetails RejectingJumpListCategoryBecauseNoRegisteredHandler(params object[] args)
    {
        _RejectingJumpListCategoryBecauseNoRegisteredHandler ??= new AvTraceDetails(7, new string[] { "Rejecting the category {0} from the jump list because this application is not registered for file types contained in the list.  JumpPath items will be removed and the operation will be retried." });
        return new AvTraceFormat(_RejectingJumpListCategoryBecauseNoRegisteredHandler, args);
    }

    /// <summary> Send a single trace output </summary>
    static public void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
    {
        _avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
    }

    /// <summary> These help delay allocation of object array </summary>
    static public void Trace(TraceEventType type, AvTraceDetails traceDetails)
    {
        _avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
    }
    static public void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
    {
        _avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1 });
    }
    static public void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
    {
        _avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2 });
    }
    static public void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
    {
        _avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2, p3 });
    }

    /// <summary> Send a singleton "activity" trace (really, this sends the same trace as both a Start and a Stop) </summary>
    static public void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
    {
        _avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
    }

    /// <summary> These help delay allocation of object array </summary>
    static public void TraceActivityItem(AvTraceDetails traceDetails)
    {
        _avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
    }
    static public void TraceActivityItem(AvTraceDetails traceDetails, object p1)
    {
        _avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1 });
    }
    static public void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
    {
        _avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2 });
    }
    static public void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
    {
        _avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2, p3 });
    }

    static public bool IsEnabled
    {
        get { return _avTrace != null && _avTrace.IsEnabled; }
    }

    /// <summary> Is there a Tracesource?  (See comment on AvTrace.IsEnabledOverride.) </summary>
    static public bool IsEnabledOverride
    {
        get { return _avTrace.IsEnabledOverride; }
    }

    /// <summary> Re-read the configuration for this trace source </summary>
    static public void Refresh()
    {
        _avTrace.Refresh();
    }
}