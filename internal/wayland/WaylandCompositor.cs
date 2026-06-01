using System.Runtime.InteropServices;

namespace Sharpland.wayland;

/// <summary>
/// Wayland compositor object wrapper
/// </summary>
internal class WaylandCompositor {
    /// <summary>
    /// Wayland compositor instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates Wayland compositor object
    /// </summary>
    /// <param name="registry">Wayland registry object</param>
    /// <param name="name">Compositor interface name</param>
    /// <param name="version">Compositor interface version</param>
    internal WaylandCompositor(WaylandRegistry registry, uint name, uint version) {
        IntPtr @interface = WaylandInterface.Compositor();
        Instance = registry.Bind(@interface, name, version);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create Wayland compositor.");
    }
}
