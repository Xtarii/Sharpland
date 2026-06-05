using System.Reflection;

namespace Sharpland.assembly.wayland.listener;

/// <summary>
/// Wayland listener object
/// <para/>
/// Wayland objects that uses events
/// for communication should extend
/// this abstract class.
/// <para/>
/// <typeparamref name="L"/> is the type of listener that Wayland uses to communicate.
/// </summary>
/// <param name="instance">Native wayland object instance</param>
/// <typeparam name="L">Native listener structure type</typeparam>
public abstract class WaylandListener<L>(IntPtr instance) : WaylandObject(instance) where L : unmanaged {
    /// <summary>
    /// Dictionary of listener objects
    /// <para/>
    /// Stored after instance data sent
    /// with the event by <c>Wayland</c>
    /// </summary>
    private readonly Dictionary<uint, WaylandListenerObject<L>> _listeners = [];



    /// <summary>
    /// <inheritdoc cref="AddListener{T, D}(L, ref D)"/>
    /// <para/>
    /// Adds a listener object for a <c>Wayland event</c>
    /// that is invoked on this object.
    /// And adds <c>C# event listeners</c> of type
    /// <typeparamref name="T"/> used in C# when
    /// the event is invoked in <c>Wayland</c>.
    /// </summary>
    /// <param name="listener">Wayland listener object</param>
    /// <param name="data">Data instance to send with each event</param>
    /// <typeparam name="D">Type of data to send with the event</typeparam>
    /// <typeparam name="T">Type of callback used for the event</typeparam>
    protected int AddListener<D, T>(L listener, ref D data) where T : Delegate where D : unmanaged {
        int res = AddListener<WaylandListenerObject<L, T>, D>(listener, ref data);
        return res;
    }

    /// <summary>
    /// <inheritdoc cref="AddListener{D, T}(L, ref D)"/>
    /// </summary>
    /// <typeparam name="K">Type of callback used for the secondary event</typeparam>
    protected int AddListener<D, T, K>(L listener, ref D data) where T : Delegate where K : Delegate where D : unmanaged {
        int res = AddListener<WaylandListenerObject<L, T, K>, D>(listener, ref data);
        return res;
    }

    /// <summary>
    /// Adds listener for <c>Wayland</c> object
    /// <para/>
    /// This allows one and the same listener to hold
    /// many different listeners that is registered on
    /// the same instance data - <c>Use at own risk.</c>
    /// The listeners are sorted after the instance data
    /// used in the events. Any new data to send with a
    /// event is stored separately and only called when
    /// that event is invoked. Thus the same object can
    /// hold events that send a <c>Surface</c> and another
    /// that sends a <c>Window</c> and keep them separated.
    /// <para/>
    /// The <paramref name="data"/> is the data to use in
    /// the event and it is sent with each event invoke.
    /// <para/>
    /// The ID of the native listener is then sent back
    /// and can be used to get the object later.
    /// </summary>
    /// <param name="listener">Wayland listener object</param>
    /// <param name="data">Data instance to send with each event</param>
    /// <typeparam name="T">Type of Wayland listener object to use</typeparam>
    /// <typeparam name="D">Type of data to send with the event</typeparam>
    /// <returns>Native listener object ID</returns>
    private unsafe int AddListener<T, D>(L listener, ref D data) where T : WaylandListenerObject<L> where D : unmanaged {
        fixed(void *d = &data) {
            if(!_listeners.ContainsKey((uint)d)) {
                ConstructorInfo? constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, [typeof(void*)]);
                if(constructor == null)
                    throw new ArgumentException("Type does not contain any constructor with valid argument - should be one \"void*\" argument");
                T obj = (T)constructor.Invoke([new IntPtr(d)]);
                _listeners.Add((uint)d, obj);
            }

            int id = _listeners[(uint)d].AddNativeListener(listener);
            AddListener(_listeners[(uint)d].GetNativeListener(id), d);
            return id;
        }
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





    /// <summary>
    /// Gets listener object
    /// </summary>
    /// <param name="id">Instance data used in the events</param>
    /// <typeparam name="T">Type of event listener</typeparam>
    /// <returns>Listener object</returns>
    protected internal WaylandListenerObject<L, T> Listener<T>(uint id) where T : Delegate => (WaylandListenerObject<L, T>)_listeners[id];

    /// <summary>
    /// <inheritdoc cref="Listener{T}(uint)"/>
    /// </summary>
    /// <typeparam name="K">Type of secondary event listener</typeparam>
    protected internal WaylandListenerObject<L, T, K> Listener<T, K>(uint id) where T : Delegate where K : Delegate => (WaylandListenerObject<L, T, K>)_listeners[id];
}





/// <inheritdoc cref="WaylandListener{L}(IntPtr)"/>
/// <typeparam name="E">Type of event callback</typeparam>
public interface IWaylandListener<E> where E : Delegate {
    /// <summary>
    /// Adds listener to this <c>Wayland object</c>
    /// <para/>
    /// A callback is invoked when <c>Wayland</c> invokes a
    /// callback on this object. The <typeparamref name="T"/> specifies
    /// what data is expected to be used in each event.
    /// </summary>
    /// <param name="listener">Event listener callback</param>
    /// <param name="data">Data to use in the event</param>
    /// <typeparam name="T">Type of data</typeparam>
    public abstract void AddListener<T>(E listener, ref T data) where T : unmanaged;
}

/// <inheritdoc cref="IWaylandListener{E}"/>
/// <typeparam name="K">Type of secondary event listener</typeparam>
public interface IWaylandListener<E, K> where E : Delegate where K : Delegate {
    /// <summary>
    /// <inheritdoc cref="IWaylandListener{E}.AddListener{T}(E, ref T)"/>
    /// <para/>
    /// The secondary event may be used as a cleanup event
    /// by <c>Wayland</c> in some cases. But is is not
    /// guaranteed and for more information see the
    /// callback description.
    /// </summary>
    /// <param name="first">Main event listener</param>
    /// <param name="second">Secondary event listener</param>
    /// <param name="data">Data to use in the event</param>
    /// <typeparam name="T">Type of data</typeparam>
    public abstract void AddListener<T>(E first, K second, ref T data) where T : unmanaged;
}
