using Sharpland.assembly.wayland.renderer;
using Sharpland.wayland;
using Sharpland.wayland.registry;

namespace Sharpland.window;

/// <summary>
/// Wayland window
/// </summary>
public class WaylandWindow : IDisposable {
    /// <summary>
    /// Window display
    /// </summary>
    public Display Display { get; private set; }

    /// <summary>
    /// Window registry
    /// </summary>
    public Registry Registry { get; private set; }

    /// <summary>
    /// Window compositor
    /// </summary>
    public WaylandCompositor Compositor { get; private set; } = null!;



    public WaylandWindow() {
        Display = new();
        Registry = Display.GetRegistry();
    }



    /// <inheritdoc/>
    public void Dispose() {
        Display.Dispose();
    }
}
