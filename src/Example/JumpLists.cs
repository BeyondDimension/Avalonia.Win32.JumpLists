using System.Text;
using Avalonia;
using Avalonia.Win32.JumpLists;
#if NETFRAMEWORK
using System.Windows.Shell;
using JumpList = Avalonia.Win32.JumpLists.JumpList;
#endif

namespace Example;

public static class JumpLists
{
    public static void Init()
    {
        // https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.shell.jumplist?view=net-5.0
        var jumpList1 = new JumpList
        {
            ShowRecentCategory = true,
            ShowFrequentCategory = true,
        };
        jumpList1.JumpItemsRejected += JumpList1_JumpItemsRejected;
        jumpList1.JumpItemsRemovedByUser += JumpList1_JumpItemsRemovedByUser;
        jumpList1.JumpItems.Add(new JumpTask
        {
            Title = "Notepad",
            Description = "Open Notepad.",
            ApplicationPath = @"C:\Windows\notepad.exe",
            IconResourcePath = @"C:\Windows\notepad.exe",
        });
        jumpList1.JumpItems.Add(new JumpTask
        {
            Title = "Read Me",
            Description = "Open readme.txt in Notepad.",
            ApplicationPath = @"C:\Windows\notepad.exe",
            IconResourcePath = @"C:\Windows\System32\imageres.dll",
            IconResourceIndex = 14,
            WorkingDirectory = @"C:\Users\Public\Documents",
            Arguments = "readme.txt",
        });
        JumpList.SetJumpList(Application.Current!, jumpList1);
    }

    static void JumpList1_JumpItemsRejected(object? sender, JumpItemsRejectedEventArgs e)
    {
        StringBuilder sb = new();
        sb.AppendFormat("{0} Jump Items Rejected:\n", e.RejectionReasons.Count);
        for (int i = 0; i < e.RejectionReasons.Count; ++i)
        {
            if (e.RejectedItems[i].GetType() == typeof(JumpPath))
                sb.AppendFormat("Reason: {0}\tItem: {1}\n", e.RejectionReasons[i], ((JumpPath)e.RejectedItems[i]).Path);
            else
                sb.AppendFormat("Reason: {0}\tItem: {1}\n", e.RejectionReasons[i], ((JumpTask)e.RejectedItems[i]).ApplicationPath);
        }

        MessageBoxShow(sb.ToString());
    }

    static void JumpList1_JumpItemsRemovedByUser(object? sender, JumpItemsRemovedEventArgs e)
    {
        StringBuilder sb = new();
        sb.AppendFormat("{0} Jump Items Removed by the user:\n", e.RemovedItems.Count);
        for (int i = 0; i < e.RemovedItems.Count; ++i)
        {
            sb.AppendFormat("{0}\n", e.RemovedItems[i]);
        }

        MessageBoxShow(sb.ToString());
    }

    static void MessageBoxShow(string content) => MS.Win32.MessageBox.Show(content, "Avalonia.Win32.JumpLists Example");

    public static void OptionalResourceStringLocalization()
    {
        SRID.Get = (name, get_default) =>
        {
            // see Avalonia.Win32.JumpLists/Resources/Strings.resx
            return name switch
            {
                //SRID.JumpItemsRejectedEventArgs_CountMismatch => "Custom localized language text.",
                _ => get_default(),
            };
        };
    }
}