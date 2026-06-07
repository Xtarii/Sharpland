using System.Runtime.InteropServices;
using Sharpland.assembly.wayland;
using Sharpland.assembly.wayland.listener;

namespace Sharpland.assembly.xdg.surface;

/// <summary>
/// XDG base object wrapper
/// </summary>
public abstract partial class XDGBase : WaylandListener<XDG.XDGBaseListener> {
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_xdg_wm_base_add_listener(IntPtr @base, XDG.XDGBaseListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_xdg_wm_base_pong(IntPtr @base, uint serial);





    /// <summary>
    /// Creates a XDG base object
    /// </summary>
    /// <param name="instance">XDG base instance</param>
    internal XDGBase(IntPtr instance) : base(instance) {
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create XDG base interface.");
    }



    protected internal override unsafe void AddListener(XDG.XDGBaseListener *listener, void *data) {
        int res = wrapper_xdg_wm_base_add_listener(Instance, listener, data);
        if(res < 0)
            throw new ExternalException("Failed to add listener to XDG base object.");
    }



    /// <summary>
    /// Responds to a XDG base event ( ping ) with pong
    /// <para/>
    /// A client must respond to a ping event with a pong request
    /// or the client may be deemed unresponsive.
    /// </summary>
    /// <param name="serial">Pong serial value</param>
    public void Pong(uint serial) => wrapper_xdg_wm_base_pong(Instance, serial);



    protected override void OnDispose() { /* Do nothing */ }
}
