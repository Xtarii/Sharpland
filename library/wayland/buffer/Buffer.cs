using Sharpland.assembly.wayland;
using Sharpland.assembly.wayland.buffer;
using Sharpland.assembly.wayland.listener;
using Sharpland.wayland.buffers.events;

namespace Sharpland.wayland.buffers;

/// <summary>
/// Buffer destroy callback
/// <para/>
/// This is called when the buffer should be
/// destroyed.
/// </summary>
/// <param name="e">Event data</param>
public delegate void BufferEventHandler(BufferEvent e);



/// <summary>
/// Wayland buffer object
/// </summary>
public class Buffer : WaylandBuffer, IWaylandListener<Wayland.BufferListener, BufferEventHandler> {
    /// <summary>
    /// Creates Wayland buffer object
    /// </summary>
    /// <param name="instance">Buffer instance</param>
    public Buffer(IntPtr instance) : base(instance) {}



    /// <inheritdoc/>
    public void AddListener<T>(BufferEventHandler listener, ref T data) where T : unmanaged {
        IWaylandListener<Wayland.BufferListener, BufferEventHandler> instance = this;

        if(instance.GetWaylandListener<WaylandListenerObject<Wayland.BufferListener, BufferEventHandler>>(out var obj)) {
            instance.SetNativeListener(obj, ref data);
        }

        obj.Events += listener;
    }

    /// <inheritdoc/>
    public void AddListener(BufferEventHandler listener) {
        IWaylandListener<Wayland.BufferListener, BufferEventHandler> instance = this;

        if(instance.GetWaylandListener<WaylandListenerObject<Wayland.BufferListener, BufferEventHandler>>(out var obj)) {
            instance.SetNativeListener(obj);
        }

        obj.Events += listener;
    }

    /// <inheritdoc/>
    protected internal unsafe override WaylandListenerObject<Wayland.BufferListener> CreateListener() {
        Wayland.BufferListener listener = new() {
            Release = &Release
        };
        return new WaylandListenerObject<Wayland.BufferListener, BufferEventHandler>(listener);
    }





    /// <summary>
    /// Release callback handler method
    /// </summary>
    /// <param name="data">Data sent with each event</param>
    /// <param name="buffer">Buffer instance</param>
    static unsafe void Release(void *data, IntPtr buffer) {
        Buffer instance = GetInstanceOf<Buffer>(buffer);
        BufferEvent e = new(instance, data);
        ((WaylandListenerObject<Wayland.BufferListener, BufferEventHandler>)instance.Listener!).Events?.Invoke(e);
    }
}
