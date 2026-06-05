using Sharpland.assembly.wayland.renderer;

namespace Sharpland.wayland.surface;

/// <summary>
/// Wayland surface object
/// </summary>
public class Surface : WaylandSurface {
    /// <summary>
    /// Creates a new Wayland surface object
    /// </summary>
    /// <param name="compositor">Wayland compositor</param>
    public Surface(WaylandCompositor compositor) : base(compositor) {}
}
