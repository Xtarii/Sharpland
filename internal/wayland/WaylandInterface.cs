using System.Runtime.InteropServices;

namespace Sharpland;

/// <summary>
/// Wayland interface wrapper
/// <para/>
/// Provides getters for wayland interfaces.
/// </summary>
internal static partial class WaylandInterface {
    /// <summary>
    /// Wayland compositor interface
    /// </summary>
    [LibraryImport(Wayland.WRAPPER, EntryPoint = "wrapper_wl_compositor_interface")]
    internal static partial IntPtr Compositor();
}
