using System.Runtime.InteropServices;
using Sharpland.wayland.registry;

namespace Sharpland.wayland.renderer;

/// <summary>
/// Wayland display object wrapper
/// <para/>
/// A wrapper for the native wayland
/// display object.
/// </summary>
internal partial class WaylandDisplay : IDisposable {
    [LibraryImport(Wayland.LIBRARY, StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr wl_display_connect(string? name);
    [LibraryImport(Wayland.LIBRARY)]
    private static partial void wl_display_disconnect(IntPtr display);
    [LibraryImport(Wayland.LIBRARY)]
    private static partial int wl_display_roundtrip(IntPtr display);
    [LibraryImport(Wayland.LIBRARY)]
    private static partial int wl_display_flush(IntPtr display);
    [LibraryImport(Wayland.LIBRARY)]
    private static partial int wl_display_dispatch(IntPtr display);





    /// <summary>
    /// Wayland display instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates a wayland display instance
    /// <para/>
    /// The <paramref name="name"/> argument is the name of the Wayland display,
    /// typically <c>wayland-0</c>.
    /// If <c>null</c> is provided instead wayland will try to use
    /// <c>$WAYLAND_DISPLAY</c> environment variable if it is set.
    /// Wayland will then try to connect to <c>$XDG_RUNTIME_DIR/$WAYLAND_DISPLAY</c>.
    /// If that failed <c>$XDG_RUNTIME_DIR/wayland-0</c> is used instead.
    /// If both attempts fail wayland won't create a display and
    /// and error is thrown.
    /// </summary>
    /// <param name="name">Wayland display name</param>
    /// <exception cref="ExternalException">
    /// This is thrown if there was an error creating the display. Typically this
    /// is because of an invalid <paramref name="name"/> being passed.
    /// </exception>
    internal WaylandDisplay(string? name) {
        Instance = wl_display_connect(name);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to setup a Wayland connection.");
    }



    public void Dispose() {
        wl_display_disconnect(Instance);
    }



    /// <summary>
    /// Gets wayland registry object from this display
    /// </summary>
    /// <param name="data">Data to use in the registry events</param>
    /// <typeparam name="T">Type of data to use in the registry events</typeparam>
    /// <returns>Wayland registry object</returns>
    public WaylandRegistry<T> GetRegistry<T>(ref T data) where T : unmanaged => new(this, ref data);

    /// <summary>
    /// Blocks until the server process all currently issued
    /// requests and sends out pending events on all event queues.
    /// </summary>
    /// <returns>The number of dispatched events</returns>
    /// <exception cref="ExternalException">
    /// This gets thrown if the display round trip failed.
    /// </exception>
    public int RoundTrip() {
        int res = wl_display_roundtrip(Instance);
        if(res < 0) throw new ExternalException("Display round trip failed.");
        return res;
    }

    /// <summary>
    /// Sends data to the compositor
    /// </summary>
    /// <returns>Flush status</returns>
    public int Flush() {
        int res = wl_display_flush(Instance);
        return res;
    }

    /// <summary>
    /// Dispatch the display’s main event queue.
    /// <para/>
    /// If the main event queue is empty, this function blocks until
    /// there are events to be read from the display file descriptor.
    /// Events are read and queued on the appropriate event queues.
    /// Finally, events on the main event queue are dispatched.
    /// </summary>
    /// <returns>The number of dispatched events on success</returns>
    public int Dispatch() {
        int res = wl_display_dispatch(Instance);
        if(res < 0)
            throw new ExternalException("Failed to dispatch main event queue.");
        return res;
    }
}
