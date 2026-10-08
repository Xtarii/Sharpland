namespace Sharpland.assembly.wayland.listener;

/// <summary>
/// Wayland event object
/// <para/>
/// This allows wayland events to
/// send data of unknown type to any
/// listener registered to that event.
/// <para/>
/// All event objects should extend this
/// abstract event object.
/// </summary>
/// <typeparam name="T">Instance object type</typeparam>
public abstract class WaylandEventObject<T> {
    /// <summary>
    /// Raw data stored as a array of bytes
    /// </summary>
    private unsafe readonly void * _raw;

    /// <summary>
    /// Event owner
    /// <para/>
    /// The instance of the object that this
    /// event was fired for.
    /// </summary>
    public T Owner { get; private set; }



    /// <summary>
    /// Creates wayland event object
    /// </summary>
    /// <param name="owner">Instance object</param>
    /// <param name="data">Raw event data</param>
    internal unsafe WaylandEventObject(T owner, void * data) {
        _raw = data;
        Owner = owner;
    }



    /// <summary>
    /// Gets event data pointer
    /// </summary>
    /// <typeparam name="K">Type of data</typeparam>
    /// <returns>Pointer to data</returns>
    public unsafe K* GetDataPointer<K>() where K : unmanaged {
        return (K*)_raw;
    }

    /// <summary>
    /// Gets event data by reference
    /// </summary>
    /// <typeparam name="K">Type of data</typeparam>
    /// <returns>Event data</returns>
    public unsafe ref K GetData<K>() where K : unmanaged {
        ref K ptr = ref (*(K*)_raw);
        return ref ptr;
    }
}
