namespace Sharpland.wayland.listener;

/// <summary>
/// Wayland listener object
/// <para/>
/// Wayland objects that uses events
/// for communication should extend
/// this abstract class.
/// </summary>
/// <param name="instance">Native wayland object instance</param>
internal abstract class WaylandListener<L>(IntPtr instance) : WaylandObject(instance) where L : unmanaged {
    /// <summary>
    /// List of object listeners
    /// </summary>
    private readonly List<L> _listeners = [];



    /// <inheritdoc cref="AddListener(L*, void*)"/>
    /// <returns>The id of the listener object in the reference list</returns>
    protected internal unsafe int AddListener(L listener, void *data) {
        int id = _listeners.Count;
        _listeners.Add(listener);

        fixed(L *ptr = &_listeners.ToArray()[id]) {
            AddListener(ptr, data);
        }

        return id;
    }



    /// <summary>
    /// Adds a listener to this Wayland object
    /// <para/>
    /// This will add a listener to the Wayland object
    /// allowing it to listen and respond to events that
    /// is sent by Wayland to this object.
    /// <para/>
    /// The <paramref name="data"/> is the data to send
    /// between events. This can be a pointer to a window
    /// instance for easy access to instance data.
    /// <para/>
    /// For the <c>data</c> to be used correctly is should
    /// not be managed by the <c>C# Garbage Collector</c>
    /// or alternatively it should be created as a class
    /// object instance and not local to a function as
    /// the function will clean it up upon exiting.
    /// </summary>
    /// <param name="listener">Wayland listener object</param>
    /// <param name="data">Data to send between events</param>
    protected internal abstract unsafe void AddListener(L *listener, void *data);
}
