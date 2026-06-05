using System.Runtime.InteropServices;
using Sharpland.assembly.wayland.registry;

namespace Sharpland.assembly.wayland.renderer;

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
    /// <returns>Wayland compositor instance</returns>
    internal static WaylandCompositor Create(WaylandRegistry registry, uint name, uint version) {
        IntPtr @interface = WaylandInterface.Compositor();
        IntPtr Instance = registry.Bind(@interface, name, version);
        return new(Instance);
    }
}
