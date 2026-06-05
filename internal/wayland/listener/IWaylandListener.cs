namespace Sharpland.assembly.wayland.listener;

/// <summary>
/// Wayland listener interface
/// <para/>
/// Wayland objects that uses events
/// for communication should extend
/// this abstract class.
/// <para/>
/// <typeparamref name="L"/> is the type of listener that Wayland uses to communicate.
/// </summary>
/// <typeparam name="L">Native listener structure type</typeparam>
public interface IWaylandListener<L> where L : unmanaged {
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
    public int AddListener<T, D>(L listener, ref D data) where T : WaylandListenerObject<L> where D : unmanaged;
}





/// <inheritdoc cref="IWaylandListener{L}"/>
/// <typeparam name="E">Type of event callback</typeparam>
public interface IWaylandListener<L, E> : IWaylandListener<L> where L : unmanaged where E : Delegate {
    /// <summary>
    /// <inheritdoc cref="IWaylandListener{L}.AddListener{T, D}(L, ref D)"/>
    /// </summary>
    /// <param name="listener">Wayland listener object</param>
    /// <param name="data">Data instance to send with each event</param>
    /// <typeparam name="T">Type of data to send with the event</typeparam>
    public int AddListener<T>(L listener, ref T data) where T : unmanaged;

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

    /// <summary>
    /// Gets listener object
    /// </summary>
    /// <param name="id">Instance data used in the events</param>
    /// <returns>Listener object</returns>
    protected internal WaylandListenerObject<L, E> Listener(uint id);
}



/// <inheritdoc cref="IWaylandListener{L, E}"/>
/// <typeparam name="K">Type of secondary event listener</typeparam>
public interface IWaylandListener<L, E, K> : IWaylandListener<L> where L : unmanaged where E : Delegate where K : Delegate {
    /// <summary>
    /// <inheritdoc cref="IWaylandListener{L}.AddListener{T, D}(L, ref D)"/>
    /// </summary>
    /// <param name="listener">Wayland listener object</param>
    /// <param name="data">Data instance to send with each event</param>
    /// <typeparam name="T">Type of data to send with the event</typeparam>
    public int AddListener<T>(L listener, ref T data) where T : unmanaged;

    /// <summary>
    /// <inheritdoc cref="IWaylandListener{L, E}.AddListener{T}(E, ref T)"/>
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

    /// <inheritdoc cref="IWaylandListener{L, T}.Listener(uint)"/>
    protected internal WaylandListenerObject<L, E, K> Listener(uint id);
}
