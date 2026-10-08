using Sharpland.assembly.wayland.listener;

namespace Sharpland.xdg.events;

/// <summary>
/// XDG surface object event data
/// </summary>
public sealed class XDGSurfaceEvent : WaylandEventObject<XDGSurface> {
    /// <summary>
    /// Event serial number
    /// </summary>
    public readonly uint Serial;



    /// <summary>
    /// Creates XDG surface event object
    /// </summary>
    /// <param name="surface">XDG surface instance</param>
    /// <param name="serial">Event serial number</param>
    /// <param name="data">Event data</param>
    internal unsafe XDGSurfaceEvent(XDGSurface surface, uint serial, void * data) : base(surface, data) {
        Serial = serial;
    }
}
