using System.Runtime.InteropServices;

namespace Sharpland.xdg;

/// <summary>
/// Wayland XDG wrapper
/// <para/>
/// XDG provides ways to render windows
/// and popups in a desktop environment
/// </summary>
internal static class XDG {
    /// <summary>
    /// XDG surface object event listener
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct XDGSurfaceListener {
        /// <summary>
        /// suggest a surface change
        /// <para/>
        /// The configure event marks the end of a configure sequence. A
        /// configure sequence is a set of one or more events configuring
        /// the state of the <see cref="XDGSurface"/>, including the final
        /// <c>XDGSurface configure</c> event.
        /// <para/>
        /// Where applicable, <c>XDGSurface</c> surface roles will during a
        /// configure sequence extend this event as a latched state sent as
        /// events before the <c>XDGSurface configure</c> event. Such events
        /// should be considered to make up a set of atomically applied
        /// configuration states, where the <c>XDGSurface configure</c> commits
        /// the accumulated state.
        /// <para/>
        /// Clients should arrange their surface for the new states, and
        /// then send an ack_configure request with the serial sent in this
        /// configure event at some point before committing the new surface.
        /// <para/>
        /// If the client receives multiple configure events before it can
        /// respond to one, it is free to discard all but the last event it
        /// received.
        /// <para/>
        /// <c>Serial</c> - serial of the configure event.
        /// </summary>
        internal unsafe delegate*<void*, IntPtr, uint, void> Configure;
    }

    /// <summary>
    /// XDG base object event listener
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct XDGBaseListener {
        /// <summary>
        /// check if the client is alive
        /// <para/>
        /// The ping event asks the client if it's still alive. Pass the
        /// serial specified in the event back to the compositor by sending
        /// a "pong" request back with the specified serial. See
        /// <c>XDGBase pong</c>.
        /// <para/>
        /// Compositors can use this to determine if the client is still
        /// alive. It's unspecified what will happen if the client doesn't
        /// respond to the ping request, or in what timeframe. Clients
        /// should try to respond in a reasonable amount of time.
        /// <para/>
        /// A compositor is free to ping in any way it wants, but a client
        /// must always respond to any <c>XDGBase</c> object it created.
        /// <para/>
        /// <c>Serial</c> - pass this to the pong request
        /// </summary>
        internal unsafe delegate*<void*, IntPtr, uint, void> Ping;
    }
}
