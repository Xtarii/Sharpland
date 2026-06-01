using System.Runtime.InteropServices;
using Sharpland.wayland;

namespace Sharpland.xdg;

internal class XDGBase {
    /// <summary>
    /// XDG base object instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates a XDG base object
    /// </summary>
    /// <param name="registry">Wayland registry</param>
    /// <param name="name">XDG interface name</param>
    /// <param name="version">XDG interface version</param>
    internal XDGBase(WaylandRegistry registry, uint name, uint version) {
        Instance = registry.Bind(XDGInterface.Base(), name, version);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create XDG base interface.");
    }
}
