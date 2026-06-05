namespace Sharpland.wayland.listener;

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
internal unsafe abstract class WaylandListenerObject<L>(void *data) where L : unmanaged {
    /// <summary>
    /// Instance data that is sent with each event
    /// </summary>
    internal void *Data { get; } = data;



    /// <summary>
    /// List of native listeners that this object holds
    /// </summary>
    private readonly List<L> _nativeListeners = [];



    /// <summary>
    /// Adds a native Wayland listener
    /// </summary>
    /// <param name="listener">Listener object</param>
    /// <returns>The added listener ID</returns>
    internal int AddNativeListener(L listener) {
        int id = _nativeListeners.Count;
        _nativeListeners.Add(listener);
        return id;
    }

    /// <summary>
    /// Gets the native listener with ID as a pointer
    /// </summary>
    /// <param name="id">Listener object ID</param>
    /// <returns>Pointer to the specified listener</returns>
    internal L * GetNativeListener(int id) {
        fixed(L *ptr = &_nativeListeners.ToArray()[id]) {
            return ptr;
        }
    }
}



/// <inheritdoc cref="WaylandListenerObject{L}"/>
/// <typeparam name="T"><c>C# event listener</c> object type</typeparam>
internal unsafe class WaylandListenerObject<L, T>(void *data) : WaylandListenerObject<L>(data) where L : unmanaged where T : Delegate {
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
internal unsafe class WaylandListenerObject<L, T, K>(void *data) : WaylandListenerObject<L, T>(data) where L : unmanaged where T : Delegate where K : Delegate {
    /// <summary>
    /// <c>C# listeners</c> for the underlying <c>Wayland</c> events
    /// </summary>
    internal K? SecondaryEvents;
}
