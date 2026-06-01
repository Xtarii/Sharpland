using System.Runtime.InteropServices;

namespace Sharpland;

/// <summary>
/// Wayland seat object wrapper
/// </summary>
internal partial class WaylandSeat {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial int wrapper_wl_seat_add_listener(IntPtr seat, IntPtr listener, IntPtr data);
}
