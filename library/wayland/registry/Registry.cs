using System.Runtime.InteropServices;
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
/// <typeparam name="K">Data to use in registry events</typeparam>
public class Registry<K> : WaylandRegistry, IWaylandListener<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>> where K : unmanaged {
    /// <summary>
    /// Creates base registry object
    /// </summary>
    /// <param name="display">Wayland display object</param>
    internal Registry(WaylandDisplay display) : base(display) {}



    public unsafe void AddListener<T>(RegistryGlobal<K> first, RegistryGlobalRemove<K> second, ref T data) where T : unmanaged {
        IWaylandListener<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>> instance = this;

        if(!instance.HasListener()) {
            Wayland.RegistryListener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>> obj = new(listener);
            instance.SetNativeListener(obj, ref data);
        }

        fixed(T *ptr = &data) {
            WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>> obj = (WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>>)Listener!;
            obj.First += first;
            obj.Secondary += second;
        }
    }





    /// <summary>
    /// Global registry event listener
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="registry">Registry instance</param>
    /// <param name="name">Interface name</param>
    /// <param name="i">Interface</param>
    /// <param name="version">Interface version</param>
    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        Registry<K> instance = GetInstanceOf<Registry<K>>(registry);
        string @interface = Marshal.PtrToStringAnsi(i)!;
        K *ptr = (K*)data;
        ((WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>>)instance.Listener!).First?.Invoke(*ptr, instance, name, @interface, version);
    }

    /// <summary>
    /// Remove registry event listener
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="registry">Registry instance</param>
    /// <param name="name">Interface name</param>
    static unsafe void Remove(void *data, IntPtr registry, uint name) {
        Registry<K> instance = GetInstanceOf<Registry<K>>(registry);
        K *ptr = (K*)data;
        ((WaylandListenerObject<Wayland.RegistryListener, RegistryGlobal<K>, RegistryGlobalRemove<K>>)instance.Listener!).Secondary?.Invoke(*ptr, instance, name);
    }
}
