using System.Runtime.InteropServices;
using Sharpland.wayland;

namespace Sharpland.xdg;

/// <summary>
/// XDG base object wrapper
/// </summary>
internal partial class XDGBase {
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_xdg_wm_base_add_listener(IntPtr @base, XDG.XDGBaseListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_xdg_wm_base_pong(IntPtr @base, uint serial);





    /// <summary>
    /// XDG base object instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates a XDG base object
    /// </summary>
    /// <param name="registry">Wayland registry</param>
    /// <param name="name">XDG interface name</param>
    /// <param name="version">XDG interface version</param>
    internal XDGBase(WaylandRegistry registry, uint name, uint version) {
        Instance = registry.Bind(XDGInterface.Base(), name, version);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create XDG base interface.");
    }



    /// <summary>
    /// Adds listener to XDG base object events
    /// <para/>
    /// The <paramref name="data"/> parameter is the
    /// data to use between events. It is sent with
    /// the event when an event is invoked.
    /// </summary>
    /// <param name="listener">Listener object</param>
    /// <param name="data">Data to use in the events</param>
    /// <returns>Add listener status</returns>
    public unsafe int AddListener(XDG.XDGBaseListener *listener, void *data) {
        int res = wrapper_xdg_wm_base_add_listener(Instance, listener, data);
        return res;
    }

    /// <summary>
    /// Responds to a XDG base event ( ping ) with pong
    /// <para/>
    /// A client must respond to a ping event with a pong request
    /// or the client may be deemed unresponsive.
    /// </summary>
    /// <param name="serial">Pong serial value</param>
    public void Pong(uint serial) => wrapper_xdg_wm_base_pong(Instance, serial);
}
