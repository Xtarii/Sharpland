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
    /// Dictionary of listener objects
    /// <para/>
    /// Stored after instance data sent
    /// with the event by <c>Wayland</c>
    /// </summary>
    protected internal readonly Dictionary<uint, WaylandListenerObject<L>> Listeners = [];



    public unsafe int AddListener<T, D>(L listener, ref D data) where T : WaylandListenerObject<L> where D : unmanaged {
        fixed(void *d = &data) {
            if(!Listeners.ContainsKey((uint)d)) {
                ConstructorInfo? constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, [typeof(void*)]) ?? throw new ArgumentException("Type does not contain any constructor with valid argument - should be one \"void*\" argument");
                T obj = (T)constructor.Invoke([new IntPtr(d)]);
                Listeners.Add((uint)d, obj);
            }

            int id = Listeners[(uint)d].AddNativeListener(listener);
            AddListener(Listeners[(uint)d].GetNativeListener(id), d);
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
}
