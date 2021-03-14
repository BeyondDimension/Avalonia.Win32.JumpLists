// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// A consolidated file of native values and enums.
//
// The naming convention for enums is to use the native prefix of common types
// as the enum name, where this makes sense to do.
// One exception is WM_, which is called WindowMessage here.
//

namespace MS.Internal.Interop
{
    using System;

    /// <summary>
    /// SHAddToRecentDocuments flags.  SHARD_*
    /// </summary>
    internal enum SHARD
    {
        /// <summary>
        /// The pv parameter points to a pointer to an item identifier list (PIDL) that identifies the document's file object.
        /// PIDLs that identify non-file objects are not accepted.
        /// </summary>
        PIDL = 0x00000001,

        /// <summary>
        /// The pv parameter points to a null-terminated ANSI string with the path and file name of the object.
        /// </summary>
        PATHA = 0x00000002,

        /// <summary>
        /// The pv parameter points to a null-terminated Unicode string with the path and file name of the object.
        /// </summary>
        PATHW = 0x00000003,

        /// <summary>
        ///  The pv parameter points to a SHARDAPPIDINFO structure that pairs an IShellItem that identifies the item
        ///  with an Application User Model ID (AppID) that associates it with a particular process or application.
        /// </summary>
        /// <remarks>
        /// Windows 7 and later.
        /// </remarks>
        APPIDINFO = 0x00000004,

        /// <summary>
        /// The pv parameter points to a SHARDAPPIDINFOIDLIST structure that pairs an absolute PIDL that identifies the item
        /// with an AppID that associates it with a particular process or application.
        /// </summary>
        /// <remarks>
        /// Windows 7 and later.
        /// </remarks>
        APPIDINFOIDLIST = 0x00000005,
        /// <summary>
        /// The pv parameter is an interface pointer to an IShellLink object.
        /// </summary>
        /// <remarks>
        /// Windows 7 and later.
        /// </remarks>
        LINK = 0x00000006, // indicates the data type is a pointer to an IShellLink instance

        /// <summary>
        /// The pv parameter points to a SHARDAPPIDINFOLINK structure that pairs an IShellLink that identifies the item
        /// with an AppID that associates it with a particular process or application.
        /// </summary>
        /// <remarks>
        /// Windows 7 and later.
        /// </remarks>
        APPIDINFOLINK = 0x00000007,

        /// <summary>
        /// The pv parameter is an interface pointer to an IShellItem object.
        /// </summary>
        /// <remarks>
        /// Windows 7 and later.
        /// </remarks>
        SHELLITEM = 0x00000008,
    }

    /// <summary>IShellLinkW::GetPath flags.  SLGP_*</summary>
    [Flags]
    internal enum SLGP
    {
        SHORTPATH = 0x1,
        UNCPRIORITY = 0x2,
        RAWPATH = 0x4
    }

    /// <summary>
    /// Common native constants.
    /// </summary>
    internal static class Win32Constant
    {
        internal const int MAX_PATH = 260;
        internal const int INFOTIPSIZE = 1024;
        internal const int TRUE = 1;
        internal const int FALSE = 0;
    }

    /// <summary>ShellItem GetDisplayName options.  SIGDN_*</summary>
    internal enum SIGDN : uint
    {                                         // lower word (& with 0xFFFF)
        NORMALDISPLAY = 0x00000000,           // SHGDN_NORMAL
        PARENTRELATIVEPARSING = 0x80018001,   // SHGDN_INFOLDER | SHGDN_FORPARSING
        DESKTOPABSOLUTEPARSING = 0x80028000,  // SHGDN_FORPARSING
        PARENTRELATIVEEDITING = 0x80031001,   // SHGDN_INFOLDER | SHGDN_FOREDITING
        DESKTOPABSOLUTEEDITING = 0x8004c000,  // SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
        FILESYSPATH = 0x80058000,             // SHGDN_FORPARSING
        URL = 0x80068000,                     // SHGDN_FORPARSING
        PARENTRELATIVEFORADDRESSBAR = 0x8007c001,     // SHGDN_INFOLDER | SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
        PARENTRELATIVE = 0x80080001,           // SHGDN_INFOLDER
    }

    // IShellFolder::GetAttributesOf flags
    [Flags]
    internal enum SFGAO : uint
    {
        /// <summary>Objects can be copied</summary>
        /// <remarks>DROPEFFECT_COPY</remarks>
        CANCOPY = 0x1,
        /// <summary>Objects can be moved</summary>
        /// <remarks>DROPEFFECT_MOVE</remarks>
        CANMOVE = 0x2,
        /// <summary>Objects can be linked</summary>
        /// <remarks>
        /// DROPEFFECT_LINK.
        ///
        /// If this bit is set on an item in the shell folder, a
        /// 'Create Shortcut' menu item will be added to the File
        /// menu and context menus for the item.  If the user selects
        /// that command, your IContextMenu::InvokeCommand() will be called
        /// with 'link'.
        /// That flag will also be used to determine if 'Create Shortcut'
        /// should be added when the item in your folder is dragged to another
        /// folder.
        /// </remarks>
        CANLINK = 0x4,
        /// <summary>supports BindToObject(IID_IStorage)</summary>
        STORAGE = 0x00000008,
        /// <summary>Objects can be renamed</summary>
        CANRENAME = 0x00000010,
        /// <summary>Objects can be deleted</summary>
        CANDELETE = 0x00000020,
        /// <summary>Objects have property sheets</summary>
        HASPROPSHEET = 0x00000040,

        // unused = 0x00000080,

        /// <summary>Objects are drop target</summary>
        DROPTARGET = 0x00000100,
        CAPABILITYMASK = 0x00000177,
        // unused = 0x00000200,
        // unused = 0x00000400,
        // unused = 0x00000800,
        // unused = 0x00001000,
        /// <summary>Object is encrypted (use alt color)</summary>
        ENCRYPTED = 0x00002000,
        /// <summary>'Slow' object</summary>
        ISSLOW = 0x00004000,
        /// <summary>Ghosted icon</summary>
        GHOSTED = 0x00008000,
        /// <summary>Shortcut (link)</summary>
        LINK = 0x00010000,
        /// <summary>Shared</summary>
        SHARE = 0x00020000,
        /// <summary>Read-only</summary>
        READONLY = 0x00040000,
        /// <summary> Hidden object</summary>
        HIDDEN = 0x00080000,
        DISPLAYATTRMASK = 0x000FC000,
        /// <summary> May contain children with SFGAO_FILESYSTEM</summary>
        FILESYSANCESTOR = 0x10000000,
        /// <summary>Support BindToObject(IID_IShellFolder)</summary>
        FOLDER = 0x20000000,
        /// <summary>Is a win32 file system object (file/folder/root)</summary>
        FILESYSTEM = 0x40000000,
        /// <summary>May contain children with SFGAO_FOLDER (may be slow)</summary>
        HASSUBFOLDER = 0x80000000,
        CONTENTSMASK = 0x80000000,
        /// <summary>Invalidate cached information (may be slow)</summary>
        VALIDATE = 0x01000000,
        /// <summary>Is this removeable media?</summary>
        REMOVABLE = 0x02000000,
        /// <summary> Object is compressed (use alt color)</summary>
        COMPRESSED = 0x04000000,
        /// <summary>Supports IShellFolder, but only implements CreateViewObject() (non-folder view)</summary>
        BROWSABLE = 0x08000000,
        /// <summary>Is a non-enumerated object (should be hidden)</summary>
        NONENUMERATED = 0x00100000,
        /// <summary>Should show bold in explorer tree</summary>
        NEWCONTENT = 0x00200000,
        /// <summary>Obsolete</summary>
        CANMONIKER = 0x00400000,
        /// <summary>Obsolete</summary>
        HASSTORAGE = 0x00400000,
        /// <summary>Supports BindToObject(IID_IStream)</summary>
        STREAM = 0x00400000,
        /// <summary>May contain children with SFGAO_STORAGE or SFGAO_STREAM</summary>
        STORAGEANCESTOR = 0x00800000,
        /// <summary>For determining storage capabilities, ie for open/save semantics</summary>
        STORAGECAPMASK = 0x70C50008,
        /// <summary>
        /// Attributes that are masked out for PKEY_SFGAOFlags because they are considered
        /// to cause slow calculations or lack context
        /// (SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER and others)
        /// </summary>
        PKEYSFGAOMASK = 0x81044000,
    }

    /// <summary>
    /// SHELLITEMCOMPAREHINTF.  SICHINT_*.
    /// </summary>
    [Flags]
    internal enum SICHINT : uint
    {
        /// <summary>iOrder based on display in a folder view</summary>
        DISPLAY = 0x00000000,
        /// <summary>exact instance compare</summary>
        ALLFIELDS = 0x80000000,
        /// <summary>iOrder based on canonical name (better performance)</summary>
        CANONICAL = 0x10000000,
        /// <summary>Windows 7 and later.</summary>
        TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,
    };

    /// <summary>
    /// GetPropertyStoreFlags.  GPS_*.
    /// </summary>
    /// <remarks>
    /// These are new for Vista, but are used in downlevel components
    /// </remarks>
    [Flags]
    internal enum GPS
    {
        // If no flags are specified (GPS_DEFAULT), a read-only property store is returned that includes properties for the file or item.
        // In the case that the shell item is a file, the property store contains:
        //     1. properties about the file from the file system
        //     2. properties from the file itself provided by the file's property handler, unless that file is offline,
        //         see GPS_OPENSLOWITEM
        //     3. if requested by the file's property handler and supported by the file system, properties stored in the
        //     alternate property store.
        //
        // Non-file shell items should return a similar read-only store
        //
        // Specifying other GPS_ flags modifies the store that is returned
        DEFAULT = 0x00000000,
        HANDLERPROPERTIESONLY = 0x00000001,   // only include properties directly from the file's property handler
        READWRITE = 0x00000002,   // Writable stores will only include handler properties
        TEMPORARY = 0x00000004,   // A read/write store that only holds properties for the lifetime of the IShellItem object
        FASTPROPERTIESONLY = 0x00000008,   // do not include any properties from the file's property handler (because the file's property handler will hit the disk)
        OPENSLOWITEM = 0x00000010,   // include properties from a file's property handler, even if it means retrieving the file from offline storage.
        DELAYCREATION = 0x00000020,   // delay the creation of the file's property handler until those properties are read, written, or enumerated
        BESTEFFORT = 0x00000040,   // For readonly stores, succeed and return all available properties, even if one or more sources of properties fails. Not valid with GPS_READWRITE.
        NO_OPLOCK = 0x00000080,   // some data sources protect the read property store with an oplock, this disables that
        MASK_VALID = 0x000000FF,
    }

    /// <summary>Flags for Known Folder APIs.  KF_FLAG_*</summary>
    /// <remarks>native enum was called KNOWN_FOLDER_FLAG</remarks>
    [Flags]
    internal enum KF_FLAG : uint
    {
        DEFAULT = 0x00000000,

        // Make sure that the folder already exists or create it and apply security specified in folder definition
        // If folder can not be created then function will return failure and no folder path (IDList) will be returned
        // If folder is located on the network the function may take long time to execute
        CREATE = 0x00008000,

        // If this flag is specified then the folder path is returned and no verification is performed
        // Use this flag is you want to get folder's path (IDList) and do not need to verify folder's existence
        //
        // If this flag is NOT specified then Known Folder API will try to verify that the folder exists
        //     If folder does not exist or can not be accessed then function will return failure and no folder path (IDList) will be returned
        //     If folder is located on the network the function may take long time to execute
        DONT_VERIFY = 0x00004000,

        // Set folder path as is and do not try to substitute parts of the path with environments variables.
        // If flag is not specified then Known Folder will try to replace parts of the path with some
        // known environment variables (%USERPROFILE%, %APPDATA% etc.)
        DONT_UNEXPAND = 0x00002000,

        // Get file system based IDList if available. If the flag is not specified the Known Folder API
        // will try to return aliased IDList by default. Example for FOLDERID_Documents -
        // Aliased - [desktop]\[user]\[Documents] - exact location is determined by shell namespace layout and might change
        // Non aliased - [desktop]\[computer]\[disk_c]\[users]\[user]\[Documents] - location is determined by folder location in the file system
        NO_ALIAS = 0x00001000,

        // Initialize the folder with desktop.ini settings
        // If folder can not be initialized then function will return failure and no folder path will be returned
        // If folder is located on the network the function may take long time to execute
        INIT = 0x00000800,

        // Get the default path, will also verify folder existence unless KF_FLAG_DONT_VERIFY is also specified
        DEFAULT_PATH = 0x00000400,

        // Get the not-parent-relative default path. Only valid with KF_FLAG_DEFAULT_PATH
        NOT_PARENT_RELATIVE = 0x00000200,

        // Build simple IDList
        SIMPLE_IDLIST = 0x00000100,

        // only return the aliased IDLists, don't fallback to file system path
        ALIAS_ONLY = 0x80000000,
    }
}