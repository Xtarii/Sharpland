using System.Runtime.InteropServices;
using Sharpland.wayland;

namespace Sharpland.xdg;

/// <summary>
/// Wayland XDG interface wrapper
/// <para/>
/// Provides getters for XDG interfaces.
/// </summary>
internal static partial class XDGInterface {
    /// <summary>
    /// XDG base interface
    /// <para/>
    /// The <c>XDGBase</c> interface is exposed as a global object enabling clients
    /// to turn their <c>wayland surfaces</c> into windows in a desktop environment.
    /// It defines the basic functionality needed for clients and the compositor to
    /// create windows that can be dragged, resized, maximized, etc, as well as
    /// creating transient windows such as popup menus.
    /// </summary>
    [LibraryImport(Wayland.WRAPPER, EntryPoint = "wrapper_xdg_wm_base_interface")]
    internal static partial IntPtr Base();

    /// <summary>
    /// XDG decoration manager interface
    /// <para/>
    /// This interface allows a compositor to announce support for server-side
    /// decorations.
    /// <para/>
    /// A window decoration is a set of window controls as deemed appropriate by
    /// the party managing them, such as user interface components used to move,
    /// resize and change a window's state.
    /// <para/>
    /// A client can use this protocol to request being decorated by a supporting
    /// compositor. If compositor and client do not negotiate the use of a
    /// server-side decoration using this protocol, clients continue to self-decorate
    /// as they see fit.
    /// </summary>
    [LibraryImport(Wayland.WRAPPER, EntryPoint = "wrapper_xdg_decoration_manager_interface")]
    internal static partial IntPtr DecorationManager();
}
