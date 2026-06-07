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
    /// Sets native <c>Wayland</c> listener object
    /// </summary>
    /// <param name="listener">Wayland listener object</param>
    /// <param name="data">Data to use in the events</param>
    /// <typeparam name="K">Type of listener to use for events</typeparam>
    /// <typeparam name="T">Type of data to use in the events</typeparam>
    protected internal void SetNativeListener<K, T>(L listener, ref T data) where K : WaylandListenerObject<L> where T : unmanaged;

    /// <summary>
    /// Simple check if object has a <c>Wayland</c> listener
    /// object registered
    /// </summary>
    /// <returns>
    /// <c>True</c> if a listener is registered
    /// else <c>False</c>
    /// </returns>
    protected internal bool HasListener();
}





/// <inheritdoc cref="IWaylandListener{L}"/>
/// <typeparam name="E">Type of event callback</typeparam>
public interface IWaylandListener<L, E> : IWaylandListener<L> where L : unmanaged where E : Delegate {
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
    public void AddListener<T>(E listener, ref T data) where T : unmanaged;
}



/// <inheritdoc cref="IWaylandListener{L, E}"/>
/// <typeparam name="K">Type of secondary event listener</typeparam>
public interface IWaylandListener<L, E, K> : IWaylandListener<L> where L : unmanaged where E : Delegate where K : Delegate {
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
    public void AddListener<T>(E first, K second, ref T data) where T : unmanaged;
}
