// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Interfaces and enums are taken from ShObjIdl.idl

namespace MS.Internal.AppModel
{
    using MS.Internal.Interop;
    using MS.Win32;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;

    [
       ComImport,
       InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
       Guid(IID.ShellLink),
    ]
    internal interface IShellLinkW
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, [In, Out] WIN32_FIND_DATAW pfd, SLGP fFlags);
        IntPtr GetIDList();
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        short GetHotKey();
        void SetHotKey(short wHotKey);
        uint GetShowCmd();
        void SetShowCmd(uint iShowCmd);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
        void Resolve(IntPtr hwnd, uint fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    internal static class NativeMethods2
    {
        [DllImport(ExternDll.Shell32, EntryPoint = "SHAddToRecentDocs")]
        private static extern void SHAddToRecentDocsString(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

        // This overload is required.  There's a cast in the Shell code that causes the wrong vtbl to be used
        // if we let the marshaller convert the parameter to an IUnknown.
        [DllImport(ExternDll.Shell32, EntryPoint = "SHAddToRecentDocs")]
        private static extern void SHAddToRecentDocs_ShellLink(SHARD uFlags, IShellLinkW pv);

        internal static void SHAddToRecentDocs(string path)
        {
            SHAddToRecentDocsString(SHARD.PATHW, path);
        }

        // Win7 only.
        internal static void SHAddToRecentDocs(IShellLinkW shellLink)
        {
            SHAddToRecentDocs_ShellLink(SHARD.LINK, shellLink);
        }

        // Vista only.  Also inconsistently doced on MSDN.  It was available in some versions of the SDK, and it mentioned on several pages, but isn't specifically doced.
        [DllImport(ExternDll.Shell32)]
        internal static extern HRESULT SHGetFolderPathEx([In] ref Guid rfid, KF_FLAG dwFlags, [In, Optional] IntPtr hToken, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath, uint cchPath);

        // Vista only
        [DllImport(ExternDll.Shell32)]
        internal static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

        /// <summary>
        /// Sets the User Model AppID for the current process, enabling Windows to retrieve this ID
        /// </summary>
        /// <param name="AppID"></param>
        [DllImport(ExternDll.Shell32, PreserveSig = false)]
        internal static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

        /// <summary>
        /// Retrieves the User Model AppID that has been explicitly set for the current process via SetCurrentProcessExplicitAppUserModelID
        /// </summary>
        /// <param name="AppID"></param>
        [DllImport(ExternDll.Shell32)]
        internal static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
    }

    [
     ComImport,
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     Guid(IID.ObjectArray),
    ]
    internal interface IObjectArray
    {
        uint GetCount();
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetAt([In] uint uiIndex, [In] ref Guid riid);
    }

    // Custom Destination List
    [
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid(IID.CustomDestinationList)
    ]
    internal interface ICustomDestinationList
    {
        void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

        // Retrieve IObjectArray of IShellItems or IShellLinks that represent removed destinations
        [return: MarshalAs(UnmanagedType.Interface)]
        object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

        // PreserveSig because this will return custom errors when attempting to add unregistered ShellItems.
        // Can't readily detect that case without just trying to append it.
        [PreserveSig]
        HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);
        void AppendKnownCategory(KDC category);
        [PreserveSig]
        HRESULT AddUserTasks(IObjectArray poa);
        void CommitList();

        // Retrieve IObjectCollection of IShellItems
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetRemovedDestinations([In] ref Guid riid);
        void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
        void AbortList();
    }

    /// <summary>
    /// KNOWNDESTCATEGORY.  KDC_*
    /// </summary>
    internal enum KDC
    {
        FREQUENT = 1,
        RECENT,
    }

    [
    ComImport,
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid(IID.ShellItem),
    ]
    internal interface IShellItem
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

        IShellItem GetParent();

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetDisplayName(SIGDN sigdnName);

        uint GetAttributes(SFGAO sfgaoMask);

        int Compare(IShellItem psi, SICHINT hint);
    }

    /// <summary>
    /// Shell Namespace helper 2
    /// </summary>
    [
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid(IID.ShellItem2),
    ]
    interface IShellItem2 : IShellItem
    {
        #region IShellItem redeclarations
        [return: MarshalAs(UnmanagedType.Interface)]
        new object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);
        new IShellItem GetParent();
        [return: MarshalAs(UnmanagedType.LPWStr)]
        new string GetDisplayName(SIGDN sigdnName);
        new SFGAO GetAttributes(SFGAO sfgaoMask);
        new int Compare(IShellItem psi, SICHINT hint);
        #endregion

        [return: MarshalAs(UnmanagedType.Interface)]
        object GetPropertyStore(
            GPS flags,
            [In] ref Guid riid);

        [return: MarshalAs(UnmanagedType.Interface)]
        object GetPropertyStoreWithCreateObject(
            GPS flags,
            [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject,   // factory for low-rights creation of type ICreateObject
            [In] ref Guid riid);

        [return: MarshalAs(UnmanagedType.Interface)]
        object GetPropertyStoreForKeys(
            IntPtr rgKeys,
            uint cKeys,
            GPS flags,
            [In] ref Guid riid);

        [return: MarshalAs(UnmanagedType.Interface)]
        object GetPropertyDescriptionList(
            IntPtr keyType,
            [In] ref Guid riid);

        // Ensures any cached information in this item is up to date, or returns __HRESULT_FROM_WIN32(ERROR_FILE_NOT_FOUND) if the item does not exist.
        void Update(IBindCtx pbc);

        void GetProperty(IntPtr key, [In, Out] PROPVARIANT pv);

        Guid GetCLSID(IntPtr key);

        FILETIME GetFileTime(IntPtr key);

        int GetInt32(IntPtr key);

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetString(IntPtr key);

        uint GetUInt32(IntPtr key);

        ulong GetUInt64(IntPtr key);

        [return: MarshalAs(UnmanagedType.Bool)]
        bool GetBool(IntPtr key);
    }

    [
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid(IID.ObjectArray),
    ]
    interface IObjectCollection : IObjectArray
    {
        #region IObjectArray redeclarations
        new uint GetCount();
        [return: MarshalAs(UnmanagedType.IUnknown)]
        new object GetAt([In] uint uiIndex, [In] ref Guid riid);
        #endregion

        void AddObject([MarshalAs(UnmanagedType.IUnknown)] object punk);
        void AddFromArray(IObjectArray poaSource);
        void RemoveObjectAt(uint uiIndex);
        void Clear();
    }

    [
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid(IID.PropertyStore)
    ]
    internal interface IPropertyStore
    {
        uint GetCount();
        PKEY GetAt(uint iProp);

        void GetValue([In] ref PKEY pkey, [In, Out] PROPVARIANT pv);

        void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

        void Commit();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct PKEY
    {
        /// <summary>fmtid</summary>
        private readonly Guid _fmtid;
        /// <summary>pid</summary>
        private readonly uint _pid;

        private PKEY(Guid fmtid, uint pid)
        {
            _fmtid = fmtid;
            _pid = pid;
        }

        /// <summary>PKEY_Title</summary>
        public static readonly PKEY Title = new(new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2);
        /// <summary>PKEY_AppUserModel_ID</summary>
        public static readonly PKEY AppUserModel_ID = new(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);
        /// <summary>PKEY_AppUserModel_IsDestListSeparator</summary>
        public static readonly PKEY AppUserModel_IsDestListSeparator = new(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 6);
        /// <summary>PKEY_AppUserModel_RelaunchCommand</summary>
        public static readonly PKEY AppUserModel_RelaunchCommand = new(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 2);
        /// <summary>PKEY_AppUserModel_RelaunchDisplayNameResource</summary>
        public static readonly PKEY AppUserModel_RelaunchDisplayNameResource = new(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 4);
        /// <summary>PKEY_AppUserModel_RelaunchIconResource</summary>
        public static readonly PKEY AppUserModel_RelaunchIconResource = new(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 3);
    }

    /// <remarks>
    /// Methods in this class will only work on Vista and above.
    /// </remarks>
    internal static class ShellUtil
    {
        public static string GetPathFromShellItem(IShellItem item)
        {
            return item.GetDisplayName(SIGDN.DESKTOPABSOLUTEPARSING);
        }

        public static string GetPathForKnownFolder(Guid knownFolder)
        {
            if (knownFolder == default(Guid))
            {
                return null;
            }

            var pathBuilder = new StringBuilder(NativeMethods.MAX_PATH);
            HRESULT hr = NativeMethods2.SHGetFolderPathEx(ref knownFolder, 0, IntPtr.Zero, pathBuilder, (uint)pathBuilder.Capacity);
            // If we failed to find a path for the known folder then just ignore it.
            return hr.Succeeded
                ? pathBuilder.ToString()
                : null;
        }

        public static IShellItem2 GetShellItemForPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                // Internal function.  Should have verified this before calling if we cared.
                return null;
            }

            Guid iidShellItem2 = new Guid(IID.ShellItem2);
            object unk;
            HRESULT hr = NativeMethods2.SHCreateItemFromParsingName(path, null, ref iidShellItem2, out unk);

            // Silently absorb errors such as ERROR_FILE_NOT_FOUND, ERROR_PATH_NOT_FOUND.
            // Let others pass through
            if (hr == (HRESULT)Win32Error.ERROR_FILE_NOT_FOUND || hr == (HRESULT)Win32Error.ERROR_PATH_NOT_FOUND)
            {
                hr = HRESULT.S_OK;
                unk = null;
            }

            hr.ThrowIfFailed();

            return (IShellItem2)unk;
        }
    }
}