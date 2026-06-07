using System.Runtime.InteropServices;
using Sharpland.assembly.wayland;
using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.renderer;

namespace Sharpland.assembly.xdg.surface;

/// <summary>
/// XDG surface object wrapper
/// </summary>
public abstract partial class XDGSurface : WaylandListener<XDG.XDGSurfaceListener> {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_xdg_wm_base_get_xdg_surface(IntPtr XDGBase, IntPtr surface);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_xdg_surface_add_listener(IntPtr xdgSurface, XDG.XDGSurfaceListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_xdg_surface_ack_configure(IntPtr xdgSurface, uint serial);





    /// <summary>
    /// Wayland surface object
    /// </summary>
    public WaylandSurface Surface { get; private set; }



    /// <summary>
    /// Creates a XDG surface object
    /// </summary>
    /// <param name="surface">Wayland surface object</param>
    /// <param name="base">XDG base interface object</param>
    internal XDGSurface(WaylandSurface surface, XDGBase @base) : base(wrapper_xdg_wm_base_get_xdg_surface(@base.Instance, surface.Instance)) {
        Surface = surface;
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create a XDG instance.");
    }



    protected override void OnDispose() { /* Do nothing */ }



    protected internal override unsafe void AddListener(XDG.XDGSurfaceListener *listener, void *data) {
        int res = wrapper_xdg_surface_add_listener(Instance, listener, data);
        if(res < 0)
            throw new AccessViolationException("Could not add listener to Wayland object.");
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
    public void AckConfigure(uint serial) => wrapper_xdg_surface_ack_configure(Instance, serial);
}
