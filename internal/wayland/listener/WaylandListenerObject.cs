namespace Sharpland.assembly.wayland.listener;

/// <summary>
/// Wayland listener object
/// <para/>
/// Used by <see cref="WaylandListener{L}"/> as a listener object
/// for the <c>C# listener</c> and holds the native listener
/// objects used by <c>Wayland</c> for event handling and any
/// <c>C# event listener</c> used in C# for event handling.
/// </summary>
/// <param name="data">Data to send with each event</param>
/// <typeparam name="L">Native listener object type</typeparam>
public unsafe abstract class WaylandListenerObject<L>(L listener, void *data) where L : unmanaged {
    /// <summary>
    /// Instance data that is sent with each event
    /// </summary>
    internal void *Data { get; } = data;



    /// <summary>
    /// List of native listeners that this object holds
    /// </summary>
    private readonly L _listener = listener;



    /// <summary>
    /// Gets the native listener as a pointer
    /// </summary>
    /// <returns>Pointer to the native listener</returns>
    internal L * GetNativeListener() {
        fixed(L *ptr = &_listener) {
            return ptr;
        }
    }
}



/// <inheritdoc cref="WaylandListenerObject{L}"/>
/// <typeparam name="T"><c>C# event listener</c> object type</typeparam>
public unsafe class WaylandListenerObject<L, T>(L listener, void *data) : WaylandListenerObject<L>(listener, data) where L : unmanaged where T : Delegate {
    /// <summary>
    /// <c>C# listeners</c> for the underlying <c>Wayland</c> events.
    /// </summary>
    internal T? Events;
}

/// <summary>
/// <inheritdoc cref="WaylandListenerObject{L, T}"/>
/// <para/>
/// This is used by listeners that uses more than one
/// listener object for communication
/// </summary>
/// <typeparam name="K"><c>C# event listener</c> object type for secondary events</typeparam>
public unsafe class WaylandListenerObject<L, T, K>(L listener, void *data) : WaylandListenerObject<L>(listener, data) where L : unmanaged where T : Delegate where K : Delegate {
    /// <summary>
    /// Main event listeners
    /// </summary>
    internal T? First;

    /// <summary>
    /// Secondary event listeners
    /// </summary>
    internal K? Secondary;
}
