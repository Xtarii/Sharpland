using System.Runtime.InteropServices;
using Sharpland.wayland;
using Sharpland.wayland.renderer;

namespace Sharpland.xdg.surface;

/// <summary>
/// XDG surface object wrapper
/// </summary>
internal partial class XDGSurface : WaylandSurface {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_xdg_wm_base_get_xdg_surface(IntPtr XDGBase, IntPtr surface);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_xdg_surface_add_listener(IntPtr xdgSurface, XDG.XDGSurfaceListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_xdg_surface_ack_configure(IntPtr xdgSurface, uint serial);





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

    /// <summary>
    /// When a configure event is received, if a client commits the
    /// surface in response to the configure event, then the client
    /// must make an <c>AckConfigure</c> request sometime before the commit
    /// request, passing along the serial of the configure event.
    /// <para/>
    /// For instance, for <c>toplevel surfaces</c> the compositor might use this
    /// information to move a surface to the top left only when the client has
    /// drawn itself for the maximized or fullscreen state.
    /// <para/>
    /// If the client receives multiple configure events before it
    /// can respond to one, it only has to ack the last configure event.
    /// <para/>
    /// A client is not required to commit immediately after sending
    /// an <c>AckConfigure</c> request - it may even <c>AckConfigure</c>
    /// several times before its next surface commit.
    /// <para/>
    /// A client may send multiple <c>AckConfigure</c> requests before committing,
    /// but only the last request sent before a commit indicates which configure
    /// event the client really is responding to.
    /// </summary>
    /// <param name="serial">Serial value</param>
    public void AckConfigure(uint serial) => wrapper_xdg_surface_ack_configure(XDGInstance, serial);
}
