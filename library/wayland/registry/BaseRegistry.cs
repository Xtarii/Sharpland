using Sharpland.assembly.wayland;
using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.registry;
using Sharpland.assembly.wayland.renderer;

namespace Sharpland.wayland.registry;

/// <summary>
/// This is called for each global registry event invoked
/// by <c>Wayland</c> on the registry object.
/// </summary>
public delegate void RegistryGlobal<K>(K data, Registry<K> registry, uint name, string @interface, uint version) where K : unmanaged;

/// <summary>
/// This is called for each global remove registry event
/// invoked by <c>Wayland</c> on the registry object.
/// </summary>
public delegate void RegistryGlobalRemove<K>(K data, Registry<K> registry, uint name) where K : unmanaged;



/// <summary>
/// Wayland registry base object
/// <para/>
/// Extends the assembly registry wrapper
/// for added <c>C#</c> functionality.
/// </summary>
public abstract class BaseRegistry<K> : WaylandRegistry, IWaylandListener<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>> where K : unmanaged {
    /// <summary>
    /// Creates base registry object
    /// </summary>
    /// <param name="display">Wayland display object</param>
    protected BaseRegistry(WaylandDisplay display) : base(display) {}



    public int AddListener<T>(Wayland.RegistryListener listener, ref T data) where T : unmanaged {
        int res = AddListener<WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>>, T>(listener, ref data);
        return res;
    }

    public unsafe void AddListener<T>(RegistryGlobal<K> first, RegistryGlobalRemove<K> second, ref T data) where T : unmanaged {
        fixed(T *ptr = &data) {
            WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>> obj = Listener((uint)ptr);
            obj.Events += first;
            obj.SecondaryEvents += second;
        }
    }

    public WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>> Listener(uint id) => (WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>>)Listeners[id];
}
