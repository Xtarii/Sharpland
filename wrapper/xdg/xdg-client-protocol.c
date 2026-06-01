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
