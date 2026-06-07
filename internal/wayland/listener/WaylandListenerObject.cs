namespace Sharpland.assembly.wayland.listener;

/// <summary>
/// Wayland listener object
/// <para/>
/// Used by <see cref="WaylandListener{L}"/> as a listener object
/// for the <c>C# listener</c> and holds the native listener
/// objects used by <c>Wayland</c> for event handling and any
/// <c>C# event listener</c> used in C# for event handling.
/// </summary>
/// <typeparam name="L">Native listener object type</typeparam>
public unsafe abstract class WaylandListenerObject<L>(L listener) where L : unmanaged {
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
public class WaylandListenerObject<L, T>(L listener) : WaylandListenerObject<L>(listener) where L : unmanaged where T : Delegate {
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
public class WaylandListenerObject<L, T, K>(L listener) : WaylandListenerObject<L>(listener) where L : unmanaged where T : Delegate where K : Delegate {
    /// <summary>
    /// Main event listeners
    /// </summary>
    internal T? First;

    /// <summary>
    /// Secondary event listeners
    /// </summary>
    internal K? Secondary;
}
