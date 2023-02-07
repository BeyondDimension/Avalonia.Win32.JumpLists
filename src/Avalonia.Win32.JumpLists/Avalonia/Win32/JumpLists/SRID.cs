// This source file is adapted from the Windows Presentation Foundation project. 
// (https://github.com/dotnet/wpf/) 
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Avalonia.Win32.JumpLists;

public static class SRID
{
    public const string JumpItemsRejectedEventArgs_CountMismatch = "JumpItemsRejectedEventArgs_CountMismatch";

    public const string Verify_FileExists = "Verify_FileExists";

    public const string Verify_NeitherNullNorEmpty = "Verify_NeitherNullNorEmpty";

    public const string JumpList_CantNestBeginInitCalls = "JumpList_CantNestBeginInitCalls";

    public const string JumpList_CantCallUnbalancedEndInit = "JumpList_CantCallUnbalancedEndInit";

    public const string JumpList_CantApplyUntilEndInit = "JumpList_CantApplyUntilEndInit";

    public const string Verify_ApartmentState = "Verify_ApartmentState";

    public static Func<string, Func<string>, string> Get { get; set; }
}