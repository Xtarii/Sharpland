using System.Runtime.InteropServices;

namespace Sharpland.assembly.wayland;

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

    /// <summary>
    /// Wayland sub-compositor interface
    /// </summary>
    [LibraryImport(Wayland.WRAPPER, EntryPoint = "wrapper_wl_subcompositor_interface")]
    internal static partial IntPtr SubCompositor();

    /// <summary>
    /// Wayland seat interface
    /// </summary>
    [LibraryImport(Wayland.WRAPPER, EntryPoint = "wrapper_wl_seat_interface")]
    internal static partial IntPtr Seat();

    /// <summary>
    /// Wayland shared memory interface
    /// </summary>
    [LibraryImport(Wayland.WRAPPER, EntryPoint = "wrapper_wl_shm_interface")]
    internal static partial IntPtr SHM();
}
