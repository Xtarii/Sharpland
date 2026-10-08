using Sharpland.assembly.wayland.listener;

namespace Sharpland.wayland.registry.events;

/// <summary>
/// Registry event
/// <para/>
/// This is used in the registry events
/// and specifies the event data and
/// other data sent by <c>Wayland</c>
/// </summary>
public sealed class RegistryEvent : WaylandEventObject<Registry> {
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
    /// Creates registry event object
    /// </summary>
    /// <param name="i">Interface type</param>
    /// <param name="name">Interface name</param>
    /// <param name="version">Interface version</param>
    /// <param name="data">Event data</param>
    /// <param name="registry">Wayland registry</param>
    internal unsafe RegistryEvent(string i, uint name, uint version, void *data, Registry registry) : base(registry, data) {
        Interface = i;
        Name = name;
        Version = version;
    }
}
