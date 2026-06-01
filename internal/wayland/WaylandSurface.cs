using System.Runtime.InteropServices;

namespace Sharpland.wayland;

/// <summary>
/// Wayland surface object wrapper
/// </summary>
internal abstract partial class WaylandSurface : IDisposable {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_compositor_create_surface(IntPtr compositor);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_wl_surface_destroy(IntPtr surface);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_wl_surface_attach(IntPtr surface, IntPtr buffer, int x, int y);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_wl_surface_damage(IntPtr surface, int x, int y, int width, int height);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_wl_surface_commit(IntPtr surface);





    /// <summary>
    /// Wayland surface object instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates Wayland surface object
    /// </summary>
    /// <param name="compositor">Wayland compositor object</param>
    internal WaylandSurface(WaylandCompositor compositor) {
        Instance = wrapper_wl_compositor_create_surface(compositor.Instance);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create Wayland surface.");
    }





    public void Dispose() => wrapper_wl_surface_destroy(Instance);



    /// <summary>
    /// Commits changes of the surface to wayland
    /// </summary>
    public void Commit() => wrapper_wl_surface_commit(Instance);

    /// <summary>
    /// Damages the surface
    /// <para/>
    /// This does <c>NOT</c> remove or delete
    /// the surface. Rather it marks it for
    /// redrawing.
    /// </summary>
    /// <param name="x">Starting X position to redraw</param>
    /// <param name="y">Starting Y position to redraw</param>
    /// <param name="width">Width of redraw area</param>
    /// <param name="height">Height of redraw area</param>
    public void Damage(int x = 0, int y = 0, int width = int.MaxValue, int height = int.MaxValue) => wrapper_wl_surface_damage(Instance, x, y, width, height);

    /// <summary>
    /// Attaches a buffer to this Wayland surface
    /// <para/>
    /// The <c>x</c> and <c>y</c> arguments specify the location of the new pending
    /// buffer's upper left corner, relative to the current buffer's upper
    /// left corner, in surface-local coordinates. In other words, the
    /// x and y, combined with the new surface size define in which
    /// directions the surface's size changes. Setting anything other than 0
    /// as x and y arguments is discouraged, and should instead be replaced
    /// with using the separate <c>Surface Offset</c> request.
    /// </summary>
    /// <param name="buffer">Buffer object pointer</param>
    /// <param name="x">X coordinates</param>
    /// <param name="y">Y coordinates</param>
    public void Attach(IntPtr buffer, int x, int y) => wrapper_wl_surface_attach(Instance, buffer, x, y);
}
