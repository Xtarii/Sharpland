using System.Reflection;

namespace Sharpland.assembly.wayland.listener;

/// <summary>
/// Wayland listener object
/// <para/>
/// This extends the <inheritdoc cref="IWaylandListener{L}"/>
/// </summary>
/// <param name="instance">Native wayland object instance</param>
public abstract class WaylandListener<L>(IntPtr instance) : WaylandObject(instance), IWaylandListener<L> where L : unmanaged {
    /// <summary>
    /// Listener object
    /// </summary>
    protected internal WaylandListenerObject<L>? Listener { get; private set; }



    unsafe void IWaylandListener<L>.SetNativeListener<K, T>(L listener, ref T data) {
        fixed(T *ptr = &data) {
            ConstructorInfo? constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, [typeof(L), typeof(void*)]) ?? throw new ArgumentException("Type does not contain any constructor with valid argument - should be one \"void*\" argument");
            K obj = (K)constructor.Invoke([listener, new IntPtr(ptr)]);
            Listener = obj;

            AddListener(Listener.GetNativeListener(), ptr);
        }
    }

    bool IWaylandListener<L>.HasListener() => Listener != null;



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
}
