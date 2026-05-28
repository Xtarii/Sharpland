using System.Runtime.InteropServices;

namespace Sharpland;

/// <summary>
/// Wayland display object
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





    /// <summary>
    /// Display native instance
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
    public WaylandRegistry GetRegistry() => new(this);

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
}
