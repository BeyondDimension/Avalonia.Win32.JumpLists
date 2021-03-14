using System;

namespace Avalonia.Win32.JumpLists
{
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
}