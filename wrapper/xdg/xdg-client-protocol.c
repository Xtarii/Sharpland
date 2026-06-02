#include "xdg-client-protocol.h"

//
// XDG interface wrappers
//

const struct wl_interface * wrapper_xdg_wm_base_interface() { return &xdg_wm_base_interface; }
const struct wl_interface * wrapper_xdg_decoration_manager_interface() { return &xdg_decoration_manager_interface; }



//
// XDG function wrapper
//

struct xdg_surface * wrapper_xdg_wm_base_get_xdg_surface(struct xdg_wm_base *xdg_wm_base, struct wl_surface *wl_surface) { return xdg_wm_base_get_xdg_surface(xdg_wm_base, wl_surface); }
int wrapper_xdg_surface_add_listener(struct xdg_surface *xdg_surface, const struct xdg_surface_listener *listener, void *data) { return xdg_surface_add_listener(xdg_surface, listener, data); }
int wrapper_xdg_wm_base_add_listener(struct xdg_wm_base *xdg_wm_base, const struct xdg_wm_base_listener *listener, void *data) { return xdg_wm_base_add_listener(xdg_wm_base, listener, data); }
void wrapper_xdg_wm_base_pong(struct xdg_wm_base *xdg_wm_base, uint32_t serial) { xdg_wm_base_pong(xdg_wm_base, serial); }
void wrapper_xdg_surface_ack_configure(struct xdg_surface *xdg_surface, uint32_t serial) { xdg_surface_ack_configure(xdg_surface, serial); }
