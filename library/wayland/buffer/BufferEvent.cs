using Sharpland.assembly.wayland.listener;

namespace Sharpland.wayland.buffers.events;

/// <summary>
/// Wayland buffer event object
/// </summary>
public sealed class BufferEvent : WaylandEventObject<Buffer> {
    /// <summary>
    /// Creates buffer event data object
    /// </summary>
    /// <param name="buffer">Wayland buffer</param>
    /// <param name="data">Event data</param>
    internal unsafe BufferEvent(Buffer buffer, void * data) : base(buffer, data) {
    }
}
