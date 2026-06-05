using System.Runtime.InteropServices;

namespace Sharpland.assembly.wayland.buffer;

/// <summary>
/// Wayland buffer object wrapper
/// </summary>
internal partial class WaylandBuffer : WaylandObject {
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_wl_buffer_add_listener(IntPtr buffer, Wayland.BufferListener *listener, void *data);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_wl_buffer_destroy(IntPtr buffer);





    /// <summary>
    /// Creates a managed wayland buffer instance
    /// </summary>
    /// <param name="instance">Wayland buffer instance</param>
    internal WaylandBuffer(IntPtr instance) : base(instance) {}



    /// <summary>
    /// Adds listener to wayland buffer events
    /// <para/>
    /// The <paramref name="data"/> parameter specifies
    /// the data used in the event and is transmitted
    /// together when an event is invoked.
    /// </summary>
    /// <param name="listener">Listener object</param>
    /// <param name="data">Data to use in the event</param>
    /// <returns>Add listener status</returns>
    public unsafe int AddListener(Wayland.BufferListener *listener, void *data) {
        int res = wrapper_wl_buffer_add_listener(Instance, listener, data);
        return res;
    }



    protected override void OnDispose() => wrapper_wl_buffer_destroy(Instance);
}
