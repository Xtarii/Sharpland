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





    /// <summary>
    /// Wayland registry instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



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
    public WaylandRegistry(WaylandDisplay display) {
        Instance = wrapper_wl_display_get_registry(display.Instance);
        if(Instance == IntPtr.Zero)
            throw new ExternalException($"Failed to get registry from display: {display}");
    }





    protected unsafe override void AddListener(Wayland.RegistryListener *listener, void *data) {
        int res = wrapper_wl_registry_add_listener(Instance, listener, data);
        if(res < 0)
            throw new AccessViolationException("Could not add listener to Wayland object.");
    }


    /// <inheritdoc cref="AddListener(Wayland.RegistryListener*, void*)"/>
    internal unsafe void AddListener(Wayland.RegistryListener *l, void *d, bool t)
        => AddListener(l, d);



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
}
