using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.renderer;
using Sharpland.assembly.xdg;
using Sharpland.assembly.xdg.surface;
using Sharpland.xdg.events;

namespace Sharpland.xdg;

/// <summary>
/// XDG surface object callback
/// </summary>
/// <param name="e">Event data</param>
public delegate void XDGSurfaceEventHandler(XDGSurfaceEvent e);



/// <summary>
/// XDG surface object
/// </summary>
public class XDGSurface : NativeXDGSurface, IWaylandListener<XDG.XDGSurfaceListener, XDGSurfaceEventHandler> {
    /// <summary>
    /// Creates a XDG surface object
    /// </summary>
    /// <param name="surface">Wayland surface object</param>
    /// <param name="xdgBase">XDG base object</param>
    public XDGSurface(WaylandSurface surface, XDGBase xdgBase) : base(surface, xdgBase) {}



    /// <inheritdoc/>
    public void AddListener<T>(XDGSurfaceEventHandler listener, ref T data) where T : unmanaged {
        IWaylandListener<XDG.XDGSurfaceListener> instance = this;

        if(instance.GetWaylandListener<WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceEventHandler>>(out var obj)) {
            instance.SetNativeListener(obj, ref data);
        }

        obj.Events += listener;
    }

    /// <inheritdoc/>
    public void AddListener(XDGSurfaceEventHandler listener) {
        IWaylandListener<XDG.XDGSurfaceListener> instance = this;

        if(instance.GetWaylandListener<WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceEventHandler>>(out var obj)) {
            instance.SetNativeListener(obj);
        }

        obj.Events += listener;
    }

    /// <inheritdoc/>
    protected internal unsafe override WaylandListenerObject<XDG.XDGSurfaceListener> CreateListener() {
        XDG.XDGSurfaceListener listener = new() {
            Configure = &Configure
        };
        return new WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceEventHandler>(listener);
    }





    /// <summary>
    /// Surface configure callback method
    /// </summary>
    /// <param name="data">Data used by the event</param>
    /// <param name="surface">The surface object</param>
    /// <param name="serial">Serial value</param>
    private static unsafe void Configure(void *data, IntPtr surface, uint serial) {
        XDGSurface instance = GetInstanceOf<XDGSurface>(surface);
        XDGSurfaceEvent e = new(instance, serial, data);
        ((WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceEventHandler>)instance.Listener!).Events?.Invoke(e);
    }
}
