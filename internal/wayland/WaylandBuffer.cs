using System.Runtime.InteropServices;

namespace Sharpland.wayland;

/// <summary>
/// Wayland buffer object wrapper
/// </summary>
internal partial class WaylandBuffer {
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_wl_buffer_add_listener(IntPtr buffer, IntPtr listener, void *data);





    /// <summary>
    /// Wayland buffer object instance
    /// </summary>
    internal IntPtr Instance { get; private set; }
}
