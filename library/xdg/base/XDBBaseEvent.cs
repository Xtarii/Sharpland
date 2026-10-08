using Sharpland.assembly.wayland.listener;

namespace Sharpland.xdg.events;

/// <summary>
/// XDG base object event data
/// </summary>
public sealed class XDGBaseEvent : WaylandEventObject<XDGBase> {
    /// <summary>
    /// Event serial number
    /// </summary>
    public readonly uint Serial;



    /// <summary>
    /// Creates a XDG base event object
    /// </summary>
    /// <param name="base">XDG base</param>
    /// <param name="serial">Event serial number</param>
    /// <param name="data">Event data</param>
    internal unsafe XDGBaseEvent(XDGBase @base, uint serial, void * data) : base(@base, data) {
        Serial = serial;
    }



    /// <summary>
    /// Event pont method
    /// <para/>
    /// This does the same thing as
    /// <see cref="assembly.xdg.surface.NativeXDGBase.Pong(uint)"/>
    /// or the same as calling
    /// <para/>
    /// <code>
    ///     e.Owner(e.Serial); // In XDGBase event handler
    /// </code>
    /// </summary>
    public void Pong() => Owner.Pong(Serial);
}
