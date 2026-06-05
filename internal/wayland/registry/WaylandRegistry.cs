using System.Runtime.InteropServices;
using Sharpland.wayland.listener;
using Sharpland.wayland.renderer;

namespace Sharpland.wayland.registry;

/// <summary>
/// Wayland registry object wrapper
/// </summary>
/// <typeparam name="T">Type of data to use in registry events</typeparam>
internal partial class WaylandRegistry<T> : WaylandListener<Wayland.RegistryListener, WaylandRegistry<T>.Callback, WaylandRegistry<T>.RemoveCallback> where T : unmanaged {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_display_get_registry(IntPtr display);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_wl_registry_add_listener(IntPtr registry, Wayland.RegistryListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_registry_bind(IntPtr wl_registry, uint name, IntPtr @interface, uint version);



    /// <summary>
    /// Registry callback for registry events
    /// <para/>
    /// This is called for events invoked on
    /// a registry object.
    /// </summary>
    public delegate void Callback(T data, WaylandRegistry<T> registry, uint name, string @interface, uint version);

    /// <summary>
    /// Registry remove callback
    /// <para/>
    /// This is called when a registry is
    /// being removed.
    /// </summary>
    public delegate void RemoveCallback(T data, WaylandRegistry<T> registry, uint name);





    /// <summary>
    /// Creates registry instance
    /// <para/>
    /// The <paramref name="display"/> is the parent display of
    /// the registry object.
    /// </summary>
    /// <param name="display">Wayland display</param>
    /// <param name="data">Data to use in registry events</param>
    /// <exception cref="ExternalException">
    /// This is thrown if there was an error getting the
    /// wayland registry from the display object.
    /// </exception>
    public WaylandRegistry(WaylandDisplay display, ref T data) : base(wrapper_wl_display_get_registry(display.Instance)) {
        if(Instance == IntPtr.Zero)
            throw new ExternalException($"Failed to get registry from display: {display}");

        unsafe {
            Wayland.RegistryListener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            AddListener<T, Callback, RemoveCallback>(listener, ref data);
        }
    }



    /// <summary>
    /// Binds a new, client-created object to the server using the
    /// specified name as the identifier.
    /// </summary>
    /// <param name="interface">Type of object to bind</param>
    /// <param name="name">Object identifier</param>
    /// <param name="version">Object version</param>
    public IntPtr Bind(IntPtr @interface, uint name, uint version) {
        return wrapper_wl_registry_bind(Instance, name, @interface, version);
    }



    protected internal sealed unsafe override void AddListener(Wayland.RegistryListener *listener, void *data) {
        int res = wrapper_wl_registry_add_listener(Instance, listener, data);
        if(res < 0)
            throw new AccessViolationException("Could not add listener to Wayland object.");
    }

    public override unsafe void AddListener<D>(Callback first, RemoveCallback second, ref D data) {
        fixed(D *ptr = &data) {
            WaylandListenerObject<Wayland.RegistryListener, Callback, RemoveCallback> obj = Listener<Callback, RemoveCallback>((uint)ptr);
            obj.Events += first;
            obj.SecondaryEvents += second;
        }
    }

    protected override void OnDispose() { /* Do nothing as there is no dispose object */ }





    /// <summary>
    /// Global registry event listener
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="registry">Registry instance</param>
    /// <param name="name">Interface name</param>
    /// <param name="i">Interface</param>
    /// <param name="version">Interface version</param>
    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        WaylandRegistry<T> instance = GetInstanceOf<WaylandRegistry<T>>(registry);
        string @interface = Marshal.PtrToStringAnsi(i)!;

        T *ptr = (T*)data;
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
        WaylandRegistry<T> instance = GetInstanceOf<WaylandRegistry<T>>(registry);

        T *ptr = (T*)data;
        RemoveCallback? events = instance.Listener<Callback, RemoveCallback>((uint)ptr).SecondaryEvents;
        events?.Invoke(*ptr, instance, name);
    }
}
