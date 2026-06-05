using System.Runtime.InteropServices;
using Sharpland.wayland.registry;

namespace Sharpland.wayland.renderer;

/// <summary>
/// Wayland compositor object wrapper
/// </summary>
internal class WaylandCompositor : WaylandObject {
    /// <summary>
    /// Creates Wayland compositor object
    /// </summary>
    /// <param name="instance">Wayland object instance</param>
    private WaylandCompositor(IntPtr instance) : base(instance) {
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create Wayland compositor.");
    }



    protected override void OnDispose() { /* Do nothing */ }





    /// <summary>
    /// Creates a Wayland compositor object
    /// </summary>
    /// <param name="registry">Wayland registry instance</param>
    /// <param name="name">Compositor name</param>
    /// <param name="version">Compositor version</param>
    /// <typeparam name="T">Type of data to use in registry</typeparam>
    /// <returns>Wayland compositor instance</returns>
    internal static WaylandCompositor Create<T>(WaylandRegistry<T> registry, uint name, uint version) where T : unmanaged {
        IntPtr @interface = WaylandInterface.Compositor();
        IntPtr Instance = registry.Bind(@interface, name, version);
        return new(Instance);
    }
}
