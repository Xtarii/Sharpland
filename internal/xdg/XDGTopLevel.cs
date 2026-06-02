using System.Runtime.InteropServices;
using Sharpland.wayland;

namespace Sharpland.xdg;

/// <summary>
/// XDG top level object wrapper
/// </summary>
internal partial class XDGTopLevel {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_xdg_surface_get_toplevel(IntPtr surface);
    [LibraryImport(Wayland.WRAPPER, StringMarshalling = StringMarshalling.Utf8)]
    private static partial void wrapper_xdg_toplevel_set_title(IntPtr level, string title);





    /// <summary>
    /// XDG top level object instance
    /// </summary>
    internal IntPtr Instance { get; private set; }

    /// <summary>
    /// XDG title
    /// </summary>
    private string title = "";



    /// <summary>
    /// Creates XDG top level object
    /// </summary>
    /// <param name="surface">XDG surface object</param>
    internal XDGTopLevel(XDGSurface surface) {
        Instance = wrapper_xdg_surface_get_toplevel(surface.XDGInstance);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to get XDG surface to level object");
    }



    /// <summary>
    /// XDG title string
    /// </summary>
    public string Title {
        get => title;
        set {
            if(title == value) return;
            title = value;
            wrapper_xdg_toplevel_set_title(Instance, title);
        }
    }
}
