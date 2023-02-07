using Avalonia.Win32.JumpLists;
using System.Resources;

namespace System;

internal partial class SR
{
    static ResourceManager resourceMan;

    static ResourceManager ResourceManager
    {
        get
        {
            if (resourceMan is null)
            {
                ResourceManager temp = new("Resources.Strings", typeof(SR).Assembly);
                resourceMan = temp;
            }
            return resourceMan;
        }
    }

    public static string Get(string name) => SRID.Get == null ?
        ResourceManager.GetString(name) :
        SRID.Get(name, () => ResourceManager.GetString(name));

    public static string Get(string name, params object[] args)
    {
        var value = Get(name);
        try
        {
            return string.Format(value, args);
        }
        catch
        {
            return value ?? string.Empty;
        }
    }
}