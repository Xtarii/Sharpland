using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.renderer;
using Sharpland.assembly.xdg;
using Sharpland.assembly.xdg.surface;

namespace Sharpland.xdg;

/// <summary>
/// XDG surface object callback
/// </summary>
/// <param name="data">Data sent with each event</param>
/// <param name="surface">XDG surface object</param>
/// <param name="serial">Event serial value</param>
/// <typeparam name="K">Type of data used in the events</typeparam>
public delegate void XDGSurfaceCallback<K>(K data, XDGSurface<K> surface, uint serial) where K : unmanaged;



/// <summary>
/// XDG surface object
/// </summary>
/// <typeparam name="K">Type of data to use in events</typeparam>
public class XDGSurface<K> : XDGSurface, IWaylandListener<XDG.XDGSurfaceListener, XDGSurfaceCallback<K>> where K : unmanaged {
    /// <summary>
    /// Creates a XDG surface object
    /// </summary>
    /// <param name="surface">Wayland surface object</param>
    /// <param name="xdgBase">XDG base object</param>
    public XDGSurface(WaylandSurface surface, XDGBase xdgBase) : base(surface, xdgBase) {}



    public unsafe void AddListener<T>(XDGSurfaceCallback<K> listener, ref T data) where T : unmanaged {
        IWaylandListener<XDG.XDGSurfaceListener> instance = this;

        if(!instance.HasListener()) {
            XDG.XDGSurfaceListener native = new() {
                Configure = &Configure
            };
            WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceCallback<K>> obj = new(native);
            instance.SetNativeListener(obj, ref data);
        }

        fixed(T *ptr = &data) {
            WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceCallback<K>> obj = (WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceCallback<K>>)Listener!;
            obj.Events += listener;
        }
    }





    /// <summary>
    /// Surface configure callback method
    /// </summary>
    /// <param name="data">Data used by the event</param>
    /// <param name="surface">The surface object</param>
    /// <param name="serial">Serial value</param>
    private static unsafe void Configure(void *data, IntPtr surface, uint serial) {
        K *ptr = (K*)data;
        XDGSurface<K> instance = GetInstanceOf<XDGSurface<K>>(surface);
        ((WaylandListenerObject<XDG.XDGSurfaceListener, XDGSurfaceCallback<K>>)instance.Listener!).Events?.Invoke(*ptr, instance, serial);
    }
}
