namespace Sharpland.wayland;

/// <summary>
/// Wayland object
/// <para/>
/// A Wayland object is an object that has
/// a native pointer to it's real instance
/// which Wayland uses to communicate.
/// </summary>
internal abstract class WaylandObject : IDisposable {
    /// <summary>
    /// Wayland instance pointer
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates Wayland object instance
    /// </summary>
    /// <param name="instance">Native wayland instance</param>
    internal WaylandObject(IntPtr instance) {
        Instance = instance;
        if(instance != IntPtr.Zero) _registry.Add(instance, this);
    }



    /// <summary>
    /// This is called when the object is being disposed of.
    /// </summary>
    protected abstract void OnDispose();



    public void Dispose() {
        OnDispose();
        if(Instance == IntPtr.Zero) return;
        _registry.Remove(Instance);
    }





    /// <summary>
    /// Wayland object registry
    /// </summary>
    private static Dictionary<IntPtr, object> _registry = [];

    /// <summary>
    /// Gets the instance of a object based on the pointer
    /// <para/>
    /// Get the <c>C# wrapper object</c> of the native
    /// Wayland instance object.
    /// </summary>
    /// <param name="instance">Object native instance</param>
    /// <typeparam name="T">Type of object expected</typeparam>
    /// <returns>The expected object as type of <typeparamref name="T"/></returns>
    protected internal static T GetInstanceOf<T>(IntPtr instance) {
        object obj = _registry[instance];
        return (T)obj;
    }
}
