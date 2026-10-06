namespace Sharpland.wayland.registry.events;

/// <summary>
/// Registry event
/// <para/>
/// This is used in the registry events
/// and specifies the event data and
/// other data sent by <c>Wayland</c>
/// </summary>
public sealed class RegistryEvent {
    /// <summary>
    /// Shared event data
    /// </summary>
    private unsafe void * _data;

    /// <summary>
    /// Event interface type
    /// </summary>
    public readonly string Interface;

    /// <summary>
    /// Event interface name
    /// </summary>
    public readonly uint Name;

    /// <summary>
    /// Event interface version
    /// </summary>
    public readonly uint Version;

    /// <summary>
    /// Event registry
    /// <para/>
    /// The registry that this
    /// event was fired on.
    /// </summary>
    public Registry Registry { get; private set; }



    /// <summary>
    /// Creates registry event object
    /// </summary>
    /// <param name="i">Interface type</param>
    /// <param name="name">Interface name</param>
    /// <param name="version">Interface version</param>
    /// <param name="data">Event data</param>
    /// <param name="registry">Wayland registry</param>
    internal unsafe RegistryEvent(string i, uint name, uint version, void *data, Registry registry) {
        Interface = i;
        Name = name;
        Version = version;
        _data = data;
        Registry = registry;
    }



    /// <summary>
    /// Gets event data pointer
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    /// <returns>Pointer to data</returns>
    public unsafe T* GetDataPointer<T>() where T : unmanaged {
        return (T*)_data;
    }

    /// <summary>
    /// Gets event data by reference
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    /// <returns>Event data</returns>
    public unsafe ref T GetData<T>() where T : unmanaged {
        ref T ptr = ref (*(T*)_data);
        return ref ptr;
    }
}
