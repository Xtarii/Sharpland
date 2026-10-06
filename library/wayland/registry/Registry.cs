using System.Runtime.InteropServices;
using Sharpland.assembly.wayland;
using Sharpland.assembly.wayland.listener;
using Sharpland.assembly.wayland.registry;
using Sharpland.assembly.wayland.renderer;
using Sharpland.wayland.registry.events;

namespace Sharpland.wayland.registry;

/// <summary>
/// Registry event handler
/// <para/>
/// A global event is called for each global registry event invoked
/// by <c>Wayland</c> on the registry object.
/// <para/>
/// And a global remove is called for each global remove registry event
/// invoked by <c>Wayland</c> on the registry object.
/// </summary>
/// <param name="e">Registry event</param>
public delegate void RegistryEventHandler(RegistryEvent e);



/// <summary>
/// Wayland registry base object
/// <para/>
/// Extends the assembly registry wrapper
/// for added <c>C#</c> functionality.
/// <para/>
/// Wayland calls on event handlers registered
/// on registries. See <see cref="RegistryEventHandler"/>
/// for the events invoked by <c>wayland</c> and when.
/// </summary>
public class Registry : WaylandRegistry, IWaylandListener<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler> {
    /// <summary>
    /// Creates base registry object
    /// </summary>
    /// <param name="display">Wayland display object</param>
    internal Registry(WaylandDisplay display) : base(display) {}



    /// <inheritdoc/>
    public void AddListener<T>(RegistryEventHandler first, RegistryEventHandler second, ref T data) where T : unmanaged {
        IWaylandListener<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler> instance = this;

        if(GetWaylandListener(out var listener)) {
            instance.SetNativeListener(listener, ref data);
        }

        listener.First += first;
        listener.Secondary += second;
    }

    /// <inheritdoc/>
    public void AddListener(RegistryEventHandler first, RegistryEventHandler second) {
        IWaylandListener<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler> instance = this;

        if(GetWaylandListener(out var listener)) {
            instance.SetNativeListener(listener);
        }

        listener.First += first;
        listener.Secondary += second;
    }



    /// <summary>
    /// Gets the registry event listener
    /// </summary>
    /// <param name="listener">Status to indicate if a new listener was created</param>
    /// <returns>Registry event listener</returns>
    private unsafe bool GetWaylandListener(out WaylandListenerObject<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler> listener) {
        IWaylandListener<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler> instance = this;

        if(!instance.HasListener()) {
            Wayland.RegistryListener regListener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            listener = new(regListener);
            return true;
        }

        listener = (WaylandListenerObject<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler>)Listener!;
        return false;
    }





    /// <summary>
    /// Global registry event listener
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="registry">Registry instance</param>
    /// <param name="name">Interface name</param>
    /// <param name="i">Interface</param>
    /// <param name="version">Interface version</param>
    private static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        Registry instance = GetInstanceOf<Registry>(registry);
        string @interface = Marshal.PtrToStringAnsi(i)!;
        RegistryEvent e = new(@interface, name, version, data, instance);
        ((WaylandListenerObject<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler>)instance.Listener!).First?.Invoke(e);
    }

    /// <summary>
    /// Remove registry event listener
    /// </summary>
    /// <param name="data">Event data</param>
    /// <param name="registry">Registry instance</param>
    /// <param name="name">Interface name</param>
    private static unsafe void Remove(void *data, IntPtr registry, uint name) {
        Registry instance = GetInstanceOf<Registry>(registry);
        RegistryEvent e = new("", name, 0, data, instance);
        ((WaylandListenerObject<Wayland.RegistryListener, RegistryEventHandler, RegistryEventHandler>)instance.Listener!).Secondary?.Invoke(e);
    }
}
