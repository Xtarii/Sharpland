using System.Runtime.InteropServices;
using Sharpland.wayland;

namespace Sharpland.xdg;

internal partial class XDGSurface : WaylandSurface {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_xdg_wm_base_get_xdg_surface(IntPtr XDGBase, IntPtr surface);





    /// <summary>
    /// XDG surface instance
    /// </summary>
    internal IntPtr XDGInstance { get; private set; }



    /// <summary>
    /// Creates a XDG surface object
    /// </summary>
    /// <param name="compositor">Wayland compositor object</param>
    /// <param name="base">XDG base interface object</param>
    internal XDGSurface(WaylandCompositor compositor, XDGBase @base) : base(compositor) {
        XDGInstance = wrapper_xdg_wm_base_get_xdg_surface(@base.Instance, Instance);
        if(XDGInstance == IntPtr.Zero)
            throw new ExternalException("Failed to create a XDG instance.");
    }
}
