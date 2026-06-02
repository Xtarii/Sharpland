using System.Runtime.InteropServices;
using Sharpland.wayland;

namespace Sharpland.xdg;

internal partial class XDGSurface : WaylandSurface {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_xdg_wm_base_get_xdg_surface(IntPtr XDGBase, IntPtr surface);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_xdg_surface_add_listener(IntPtr xdgSurface, XDG.XDGSurfaceListener *listener, void *data);





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



    /// <summary>
    /// Adds XDG surface object event listener
    /// <para/>
    /// The <paramref name="data"/> parameter specifies
    /// the data to send between events.
    /// </summary>
    /// <param name="listener">Listener object</param>
    /// <param name="data">Data to send in the events</param>
    /// <returns>Add listener status</returns>
    public unsafe int AddListener(XDG.XDGSurfaceListener *listener, void *data) {
        int res = wrapper_xdg_surface_add_listener(XDGInstance, listener, data);
        return res;
    }
}
