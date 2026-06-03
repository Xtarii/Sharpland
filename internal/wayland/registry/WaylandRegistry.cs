using System.Runtime.InteropServices;
using Sharpland.wayland.listener;
using Sharpland.wayland.renderer;

namespace Sharpland.wayland.registry;

/// <summary>
/// Wayland registry object wrapper
/// </summary>
internal partial class WaylandRegistry : WaylandListener<Wayland.RegistryListener> {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_display_get_registry(IntPtr display);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_wl_registry_add_listener(IntPtr registry, Wayland.RegistryListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_registry_bind(IntPtr wl_registry, uint name, IntPtr @interface, uint version);



    public unsafe delegate void RegistryCallback(void *data, WaylandRegistry registry, uint name, string @interface, uint version);
    public unsafe delegate void RegistryRemoveCallback(void *data, WaylandRegistry registry, uint name);
    private event RegistryCallback? onGlobal;
    private event RegistryRemoveCallback? onRemove;





    /// <summary>
    /// Creates registry instance
    /// <para/>
    /// The <paramref name="display"/> is the parent display of
    /// the registry object.
    /// </summary>
    /// <param name="display">Wayland display</param>
    /// <exception cref="ExternalException">
    /// This is thrown if there was an error getting the
    /// wayland registry from the display object.
    /// </exception>
    public WaylandRegistry(WaylandDisplay display) : base(wrapper_wl_display_get_registry(display.Instance)) {
        if(Instance == IntPtr.Zero)
            throw new ExternalException($"Failed to get registry from display: {display}");
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





    public unsafe void AddListener(RegistryCallback onGlobal, RegistryRemoveCallback onRemove, void *data) {
        if(NativeListenersCount <= 0) {
            Wayland.RegistryListener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            AddListener(listener, data);
        }

        this.onGlobal += onGlobal;
        this.onRemove += onRemove;
    }

    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        WaylandRegistry instance = GetInstanceOf<WaylandRegistry>(registry);
        string @interface = Marshal.PtrToStringAnsi(i)!;
        instance.onGlobal?.Invoke(data, instance, name, @interface, version);
    }

    static unsafe void Remove(void *data, IntPtr registry, uint name) {
        WaylandRegistry instance = GetInstanceOf<WaylandRegistry>(registry);
        instance.onRemove?.Invoke(data, instance, name);
    }





    protected internal sealed unsafe override void AddListener(Wayland.RegistryListener *listener, void *data) {
        int res = wrapper_wl_registry_add_listener(Instance, listener, data);
        if(res < 0)
            throw new AccessViolationException("Could not add listener to Wayland object.");
    }

    protected override void OnDispose() { /* Do nothing as there is no dispose object */ }
}
