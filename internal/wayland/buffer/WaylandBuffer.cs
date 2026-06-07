using System.Runtime.InteropServices;
using Sharpland.assembly.wayland.listener;

namespace Sharpland.assembly.wayland.buffer;

/// <summary>
/// Wayland buffer object wrapper
/// </summary>
public partial class WaylandBuffer : WaylandListener<Wayland.BufferListener> {
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_wl_buffer_add_listener(IntPtr buffer, Wayland.BufferListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_wl_buffer_destroy(IntPtr buffer);





    /// <summary>
    /// Creates a managed wayland buffer instance
    /// </summary>
    /// <param name="instance">Wayland buffer instance</param>
    internal WaylandBuffer(IntPtr instance) : base(instance) {}



    protected internal override unsafe void AddListener(Wayland.BufferListener *listener, void *data) {
        int res = wrapper_wl_buffer_add_listener(Instance, listener, data);
        if(res < 0)
            throw new ExternalException("Failed to set buffer listener object.");
    }



    protected override void OnDispose() => wrapper_wl_buffer_destroy(Instance);
}
