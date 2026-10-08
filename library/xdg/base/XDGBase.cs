using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.registry;
using Sharpland.assembly.xdg;
using Sharpland.xdg.events;

namespace Sharpland.xdg;

/// <summary>
/// XDG base object ping callback
/// </summary>
/// <param name="e">Event data</param>
public delegate void XDGBaseEventHandler(XDGBaseEvent e);



/// <summary>
/// XDG base object
/// </summary>
public class XDGBase : assembly.xdg.surface.NativeXDGBase, IWaylandListener<XDG.XDGBaseListener, XDGBaseEventHandler> {
    /// <summary>
    /// Creates XDG base object
    /// </summary>
    /// <param name="registry">Wayland registry</param>
    /// <param name="name">Base name</param>
    /// <param name="version">Base version</param>
    public XDGBase(WaylandRegistry registry, uint name, uint version) : base(registry.Bind(XDGInterface.Base(), name, version)) {}



    /// <inheritdoc/>
    public void AddListener<T>(XDGBaseEventHandler listener, ref T data) where T : unmanaged {
        IWaylandListener<XDG.XDGBaseListener, XDGBaseEventHandler> instance = this;

        if(instance.GetWaylandListener<WaylandListenerObject<XDG.XDGBaseListener, XDGBaseEventHandler>>(out var obj)) {
            instance.SetNativeListener(obj, ref data);
        }

        obj.Events += listener;
    }

    /// <inheritdoc/>
    public void AddListener(XDGBaseEventHandler listener) {
        IWaylandListener<XDG.XDGBaseListener, XDGBaseEventHandler> instance = this;

        if(instance.GetWaylandListener<WaylandListenerObject<XDG.XDGBaseListener, XDGBaseEventHandler>>(out var obj)) {
            instance.SetNativeListener(obj);
        }

        obj.Events += listener;
    }

    /// <inheritdoc/>
    protected internal unsafe override WaylandListenerObject<XDG.XDGBaseListener> CreateListener() {
        XDG.XDGBaseListener native = new() {
            Ping = &Ping
        };
        return new WaylandListenerObject<XDG.XDGBaseListener, XDGBaseEventHandler>(native);
    }





    /// <summary>
    /// Ping callback handler
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="bs">Base instance</param>
    /// <param name="serial">Serial number</param>
    static unsafe void Ping(void *data, IntPtr bs, uint serial) {
        XDGBase instance = GetInstanceOf<XDGBase>(bs);
        XDGBaseEvent e = new(instance, serial, data);
        ((WaylandListenerObject<XDG.XDGBaseListener, XDGBaseEventHandler>)instance.Listener!).Events?.Invoke(e);
    }
}
