using System.Runtime.InteropServices;

namespace Sharpland.wayland;

/// <summary>
/// Wayland global configs
/// </summary>
internal static class Wayland {
    /// <summary>
    /// Wayland library path
    /// </summary>
    internal const string LIBRARY = "lib/libwayland-client";

    /// <summary>
    /// Wayland C# wrapper library path
    /// </summary>
    internal const string WRAPPER = "lib/libwayland-wrapper";





    /// <summary>
    /// Wayland registry listener object
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct Listener {
        /// <summary>
        /// Announce global object
        /// <para/>
        /// Notify the client of global objects.
        /// </summary>
        public delegate*<void*, IntPtr, uint, IntPtr, uint, void> Global;

        /// <summary>
        /// Announce removal of global object
        /// <para/>
        /// Notify the client of removed global objects.
        /// </summary>
        public delegate*<void*, IntPtr, uint, void> GlobalRemove;
    }
}
