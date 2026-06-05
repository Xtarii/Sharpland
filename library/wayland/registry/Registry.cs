using System.Runtime.InteropServices;
using Sharpland.assembly.wayland;
using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.registry;
using Sharpland.assembly.wayland.renderer;

namespace Sharpland.wayland;

/// <summary>
/// Registry object
/// <para/>
/// When a client creates a registry object, the registry object will
/// emit a global event for each global currently in the registry.
/// Globals come and go as a result of device or monitor switching,
/// reconfiguration or other events, and the registry will send out
/// <c>global</c> and <c>global remove</c> events to keep the client
/// up to date with the changes. To mark the end of the initial burst
/// of events, the client can use the <see cref="Display.Sync()"/>
/// request immediately after calling <see cref="Display.GetRegistry{T}(ref T)"/>.
/// </summary>
/// <typeparam name="K">
/// Type of data to use in the registry global event
/// listener. This data will be sent with each event.
/// </typeparam>
public class Registry<K> : WaylandRegistry, IWaylandListener<Registry<K>.Callback, Registry<K>.RemoveCallback> where K : unmanaged {
    /// <summary>
    /// This is called for each global registry event invoked
    /// by <c>Wayland</c> on the registry object.
    /// </summary>
    public delegate void Callback(K data, Registry<K> registry, uint name, string @interface, uint version);

    /// <summary>
    /// This is called for each global remove registry event
    /// invoked by <c>Wayland</c> on the registry object.
    /// </summary>
    public delegate void RemoveCallback(K data, Registry<K> registry, uint name);





    /// <summary>
    /// Creates registry object
    /// </summary>
    /// <param name="display">Wayland display object</param>
    /// <param name="data">Data to use in the registry events</param>
    internal Registry(WaylandDisplay display, ref K data) : base(display) {
        unsafe {
            Wayland.RegistryListener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            AddListener<K, Callback, RemoveCallback>(listener, ref data);
        }
    }



    public unsafe void AddListener<T>(Callback first, RemoveCallback second, ref T data) where T : unmanaged {
        fixed(T *ptr = &data) {
            WaylandListenerObject<Wayland.RegistryListener, Callback, RemoveCallback> obj = Listener<Callback, RemoveCallback>((uint)ptr);
            obj.Events += first;
            obj.SecondaryEvents += second;
        }
    }





    /// <summary>
    /// Global registry event listener
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="registry">Registry instance</param>
    /// <param name="name">Interface name</param>
    /// <param name="i">Interface</param>
    /// <param name="version">Interface version</param>
    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        Registry<K> instance = GetInstanceOf<Registry<K>>(registry);
        string @interface = Marshal.PtrToStringAnsi(i)!;

        K *ptr = (K*)data;
        Callback? events = instance.Listener<Callback, RemoveCallback>((uint)ptr).Events;
        events?.Invoke(*ptr, instance, name, @interface, version);
    }

    /// <summary>
    /// Remove registry event listener
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="registry">Registry instance</param>
    /// <param name="name">Interface name</param>
    static unsafe void Remove(void *data, IntPtr registry, uint name) {
        Registry<K> instance = GetInstanceOf<Registry<K>>(registry);

        K *ptr = (K*)data;
        RemoveCallback? events = instance.Listener<Callback, RemoveCallback>((uint)ptr).SecondaryEvents;
        events?.Invoke(*ptr, instance, name);
    }
}
