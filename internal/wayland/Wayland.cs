using System.Runtime.InteropServices;

namespace Sharpland.assembly.wayland;

/// <summary>
/// Wayland global configs
/// </summary>
public static class Wayland {
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
    public unsafe struct RegistryListener {
        /// <summary>
        /// Announce global object
        /// <para/>
        /// Notify the client of global objects.
        /// </summary>
        internal delegate*<void*, IntPtr, uint, IntPtr, uint, void> Global;

        /// <summary>
        /// Announce removal of global object
        /// <para/>
        /// Notify the client of removed global objects.
        /// </summary>
        internal delegate*<void*, IntPtr, uint, void> GlobalRemove;
    }

    /// <summary>
    /// Wayland buffer event listener object
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferListener {
        /// <summary>
        /// compositor releases buffer
        /// <para/>
        /// Sent when this <c>WaylandBuffer</c> is no longer used by the compositor.
        /// <para/>
        /// For more information on when release events may or may not be
        /// sent, and what consequences it has, please see the description
        /// of <see cref="WaylandSurface.Attach(nint, int, int)"/>.
        /// <para/>
        /// If a client receives a release event before the frame callback
        /// requested in the same <see cref="WaylandSurface.Commit"/> that
        /// attaches this <c>WaylandBuffer</c> to a surface, then the client is
        /// immediately free to reuse the buffer and its backing storage,
        /// and does not need a second buffer for the next surface content
        /// update. Typically this is possible, when the compositor maintains
        /// a copy of the <c>WaylandSurface</c> contents, e.g. as a GL texture.
        /// This is an important optimization for GL(ES) compositors
        /// with <c>WaylandSharedMemory</c> clients.
        /// </summary>
        internal unsafe delegate*<void*, IntPtr, void> Release;
    }
}
