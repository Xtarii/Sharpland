using Sharpland.assembly.wayland;
using Sharpland.assembly.wayland.buffer;
using Sharpland.assembly.wayland.listener;

namespace Sharpland.wayland.buffers;

/// <summary>
/// Buffer destroy callback
/// <para/>
/// This is called when the buffer should be
/// destroyed.
/// </summary>
/// <param name="data">Event data</param>
/// <param name="buffer">Buffer object</param>
/// <typeparam name="K">Type of data used in the event</typeparam>
public delegate void BufferRemoveCallback<K>(K data, Buffer<K> buffer) where K : unmanaged;



/// <summary>
/// Wayland buffer object
/// </summary>
/// <typeparam name="K">Type of data used in buffer events</typeparam>
public class Buffer<K> : WaylandBuffer, IWaylandListener<Wayland.BufferListener, BufferRemoveCallback<K>> where K : unmanaged {
    /// <summary>
    /// Creates Wayland buffer object
    /// </summary>
    /// <param name="instance">Buffer instance</param>
    public Buffer(IntPtr instance) : base(instance) {}



    public unsafe void AddListener<T>(BufferRemoveCallback<K> listener, ref T data) where T : unmanaged {
        IWaylandListener<Wayland.BufferListener, BufferRemoveCallback<K>> instance = this;

        if(!instance.HasListener()) {
            Wayland.BufferListener native = new() {
                Release = &Release
            };
            WaylandListenerObject<Wayland.BufferListener, BufferRemoveCallback<K>> obj = new(native);
            instance.SetNativeListener(obj, ref data);
        }

        fixed(T *ptr = &data) {
            WaylandListenerObject<Wayland.BufferListener, BufferRemoveCallback<K>> obj = (WaylandListenerObject<Wayland.BufferListener, BufferRemoveCallback<K>>)Listener!;
            obj.Events += listener;
        }
    }





    /// <summary>
    /// Release callback handler method
    /// </summary>
    /// <param name="data">Data sent with each event</param>
    /// <param name="buffer">Buffer instance</param>
    static unsafe void Release(void *data, IntPtr buffer) {
        K *ptr = (K*)data;
        Buffer<K> instance = GetInstanceOf<Buffer<K>>(buffer);
        ((WaylandListenerObject<Wayland.BufferListener, BufferRemoveCallback<K>>)instance.Listener!).Events?.Invoke(*ptr, instance);
    }
}
