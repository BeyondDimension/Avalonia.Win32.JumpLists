// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/wpf/blob/v6.0.6/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/MessageBox.cs

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MS.Win32;

public static partial class MessageBox
{
    const uint DEFAULT_BUTTON1 = 0x00000000;
    const uint DEFAULT_BUTTON2 = 0x00000100;
    const uint DEFAULT_BUTTON3 = 0x00000200;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static MessageBoxResult Win32ToMessageBoxResult(PInvoke.User32.MessageBoxResult value) => value switch
    {
        PInvoke.User32.MessageBoxResult.IDOK => MessageBoxResult.OK,
        PInvoke.User32.MessageBoxResult.IDCANCEL => MessageBoxResult.Cancel,
        PInvoke.User32.MessageBoxResult.IDYES => MessageBoxResult.Yes,
        PInvoke.User32.MessageBoxResult.IDNO => MessageBoxResult.No,
        _ => MessageBoxResult.No,
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MessageBoxResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            MessageBoxOptions options) => ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, defaultResult, options);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MessageBoxResult Show(
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon,
          MessageBoxResult defaultResult) => ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, defaultResult, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MessageBoxResult Show(
         string messageBoxText,
         string caption,
         MessageBoxButton button,
         MessageBoxImage icon) => ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MessageBoxResult Show(
          string messageBoxText,
          string caption,
          MessageBoxButton button) => ShowCore(IntPtr.Zero, messageBoxText, caption, button, MessageBoxImage.None, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MessageBoxResult Show(string messageBoxText, string caption) => ShowCore(IntPtr.Zero, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MessageBoxResult Show(string messageBoxText) => ShowCore(IntPtr.Zero, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static uint DefaultResultToButtonNumber(MessageBoxResult result, MessageBoxButton button)
    {
        if (result == 0) return DEFAULT_BUTTON1;

        switch (button)
        {
            case MessageBoxButton.OK:
                return DEFAULT_BUTTON1;
            case MessageBoxButton.OKCancel:
                if (result == MessageBoxResult.Cancel) return DEFAULT_BUTTON2;
                return DEFAULT_BUTTON1;
            case MessageBoxButton.YesNo:
                if (result == MessageBoxResult.No) return DEFAULT_BUTTON2;
                return DEFAULT_BUTTON1;
            case MessageBoxButton.YesNoCancel:
                if (result == MessageBoxResult.No) return DEFAULT_BUTTON2;
                if (result == MessageBoxResult.Cancel) return DEFAULT_BUTTON3;
                return DEFAULT_BUTTON1;
            default:
                return DEFAULT_BUTTON1;
        }
    }

    internal static MessageBoxResult ShowCore(
           IntPtr owner,
           string messageBoxText,
           string caption,
           MessageBoxButton button,
           MessageBoxImage icon,
           MessageBoxResult defaultResult,
           MessageBoxOptions options)
    {
        if (!IsValidMessageBoxButton(button))
        {
            throw new InvalidEnumArgumentException("button", (int)button, typeof(MessageBoxButton));
        }
        if (!IsValidMessageBoxImage(icon))
        {
            throw new InvalidEnumArgumentException("icon", (int)icon, typeof(MessageBoxImage));
        }
        if (!IsValidMessageBoxResult(defaultResult))
        {
            throw new InvalidEnumArgumentException("defaultResult", (int)defaultResult, typeof(MessageBoxResult));
        }
        if (!IsValidMessageBoxOptions(options))
        {
            throw new InvalidEnumArgumentException("options", (int)options, typeof(MessageBoxOptions));
        }

        // UserInteractive??
        //
        /*if (!SystemInformation.UserInteractive && (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) == 0) {
            throw new InvalidOperationException("UNDONE: SR.GetString(SR.CantShowModalOnNonInteractive)");
        }*/

        if ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != 0)
        {
            if (owner != IntPtr.Zero)
            {
                throw new ArgumentException(SR.Get(SRID.CantShowMBServiceWithOwner));
            }
        }
        else
        {
            if (owner == IntPtr.Zero)
            {
                owner = PInvoke.User32.GetActiveWindow();
            }
        }

        uint style = (uint)button | (uint)icon | DefaultResultToButtonNumber(defaultResult, button) | (uint)options;

        // modal dialog notification?
        //
        //Application.BeginModalMessageLoop();
        //MessageBoxResult result = Win32ToMessageBoxResult(SafeNativeMethods.MessageBox(new HandleRef(owner, handle), messageBoxText, caption, style));
        MessageBoxResult result = Win32ToMessageBoxResult(
            PInvoke.User32.MessageBox(new HandleRef(null, owner).Handle,
            messageBoxText,
            caption,
            (PInvoke.User32.MessageBoxOptions)style));
        // modal dialog notification?
        //
        //Application.EndModalMessageLoop();

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool IsValidMessageBoxButton(MessageBoxButton value) => value == MessageBoxButton.OK
           || value == MessageBoxButton.OKCancel
           || value == MessageBoxButton.YesNo
           || value == MessageBoxButton.YesNoCancel;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool IsValidMessageBoxImage(MessageBoxImage value) => value == MessageBoxImage.Asterisk
           || value == MessageBoxImage.Error
           || value == MessageBoxImage.Exclamation
           || value == MessageBoxImage.Hand
           || value == MessageBoxImage.Information
           || value == MessageBoxImage.None
           || value == MessageBoxImage.Question
           || value == MessageBoxImage.Stop
           || value == MessageBoxImage.Warning;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool IsValidMessageBoxResult(MessageBoxResult value) => value == MessageBoxResult.Cancel
           || value == MessageBoxResult.No
           || value == MessageBoxResult.None
           || value == MessageBoxResult.OK
           || value == MessageBoxResult.Yes;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool IsValidMessageBoxOptions(MessageBoxOptions value)
    {
        int mask = ~((int)MessageBoxOptions.ServiceNotification |
                     (int)MessageBoxOptions.DefaultDesktopOnly |
                     (int)MessageBoxOptions.RightAlign |
                     (int)MessageBoxOptions.RtlReading);

        if (((int)value & mask) == 0)
            return true;
        return false;
    }
}

public enum MessageBoxResult : byte
{
    None = 0,

    OK = 1,

    Cancel = 2,

    Yes = 6,

    No = 7,

    // NOTE: if you add or remove any values in this enum, be sure to update MessageBox.IsValidMessageBoxResult()
}

[Flags]
public enum MessageBoxOptions : uint
{
    None = 0x00000000,

    ServiceNotification = 0x00200000,

    DefaultDesktopOnly = 0x00020000,

    RightAlign = 0x00080000,

    RtlReading = 0x00100000,
}

public enum MessageBoxImage : uint
{
    None = 0,

    Hand = 0x00000010,

    Question = 0x00000020,

    Exclamation = 0x00000030,

    Asterisk = 0x00000040,

    Stop = Hand,

    Error = Hand,

    Warning = Exclamation,

    Information = Asterisk,

    // NOTE: if you add or remove any values in this enum, be sure to update MessageBox.IsValidMessageBoxIcon()    
}

public enum MessageBoxButton : uint
{
    OK = 0x00000000,

    OKCancel = 0x00000001,

    YesNoCancel = 0x00000003,

    YesNo = 0x00000004,

    // NOTE: if you add or remove any values in this enum, be sure to update MessageBox.IsValidMessageBoxButton()
}

partial class MessageBox
{
    static class SR
    {
        public static string Get(string name) => name switch
        {
            SRID.CantShowMBServiceWithOwner => "Cannot show MessageBox Service with Owner.",
            _ => "",
        };
    }

    static class SRID
    {
        /// <summary>
        /// https://github.com/dotnet/wpf/blob/v6.0.6/src/Microsoft.DotNet.Wpf/src/PresentationFramework/Resources/Strings.resx#L510
        /// </summary>
        public const string CantShowMBServiceWithOwner = "CantShowMBServiceWithOwner";
    }
}