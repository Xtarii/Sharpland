using Sharpland.assembly.wayland.renderer;

namespace Sharpland.wayland;

/// <summary>
/// Display object
/// <para/>
/// A Wayland display object. This is
/// used to manage windows and surfaces.
/// <para/>
/// Before any window can be made or any
/// surface created a display needs to be
/// created that can parent any component
/// and underlying objects.
/// </summary>
public class Display : WaylandDisplay {
    /// <summary>
    /// Creates display object with name
    /// </summary>
    /// <param name="name">Display name</param>
    public Display(string name) : base(name) {}

    /// <summary>
    /// Creates display object
    /// </summary>
    public Display() : base(null) {}



    /// <summary>
    /// Gets wayland registry object from this display
    /// </summary>
    /// <param name="data">Data to use in the registry events</param>
    /// <typeparam name="T">Type of data to use in the registry events</typeparam>
    /// <returns>Wayland registry object</returns>
    public Registry<T> GetRegistry<T>(ref T data) where T : unmanaged => new(this, ref data);
}
