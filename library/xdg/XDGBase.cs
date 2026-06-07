using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.registry;
using Sharpland.assembly.xdg;

namespace Sharpland.xdg;

/// <summary>
/// XDG base object ping callback
/// </summary>
/// <param name="data">Event data</param>
/// <param name="xdgBase">XDG base object</param>
/// <param name="serial">Event serial value</param>
/// <typeparam name="K">Type of data used in the event</typeparam>
public delegate void XDGBasePingCallback<K>(K data, XDGBase<K> xdgBase, uint serial) where K : unmanaged;



/// <summary>
/// XDG base object
/// </summary>
/// <typeparam name="K">Type of data to use in object events</typeparam>
public class XDGBase<K> : assembly.xdg.surface.XDGBase, IWaylandListener<XDG.XDGBaseListener, XDGBasePingCallback<K>> where K : unmanaged {
    /// <summary>
    /// Creates XDG base object
    /// </summary>
    /// <param name="registry">Wayland registry</param>
    /// <param name="name">Base name</param>
    /// <param name="version">Base version</param>
    public XDGBase(WaylandRegistry registry, uint name, uint version) : base(registry.Bind(XDGInterface.Base(), name, version)) {}



    public unsafe void AddListener<T>(XDGBasePingCallback<K> listener, ref T data) where T : unmanaged {
        IWaylandListener<XDG.XDGBaseListener, XDGBasePingCallback<K>> instance = this;

        if(!instance.HasListener()) {
            XDG.XDGBaseListener native = new() {
                Ping = &Ping
            };
            WaylandListenerObject<XDG.XDGBaseListener, XDGBasePingCallback<K>> obj = new(native);
            instance.SetNativeListener(obj, ref data);
        }

        fixed(T *ptr = &data) {
            WaylandListenerObject<XDG.XDGBaseListener, XDGBasePingCallback<K>> obj = (WaylandListenerObject<XDG.XDGBaseListener, XDGBasePingCallback<K>>)Listener!;
            obj.Events += listener;
        }
    }





    /// <summary>
    /// Ping callback handler
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="bs">Base instance</param>
    /// <param name="serial">Serial number</param>
    static unsafe void Ping(void *data, IntPtr bs, uint serial) {
        K *ptr = (K*)data;
        XDGBase<K> instance = GetInstanceOf<XDGBase<K>>(bs);
        ((WaylandListenerObject<XDG.XDGBaseListener, XDGBasePingCallback<K>>)instance.Listener!).Events?.Invoke(*ptr, instance, serial);
    }
}
