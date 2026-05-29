using System.Runtime.InteropServices;

namespace Sharpland;

/// <summary>
/// Wayland surface object wrapper
/// </summary>
internal partial class WaylandSurface {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_compositor_create_surface(IntPtr compositor);





    /// <summary>
    /// Wayland surface object instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    internal WaylandSurface() {
        // Create surface
    }
}
