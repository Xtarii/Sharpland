#include "xdg.h"

WL_PRIVATE const struct wl_interface xdg_wm_base_interface = {
	"xdg_wm_base", 4,
	4, xdg_wm_base_requests,
	1, xdg_wm_base_events,
};

WL_PRIVATE const struct wl_interface xdg_positioner_interface = {
	"xdg_positioner", 4,
	10, xdg_positioner_requests,
	0, NULL,
};

WL_PRIVATE const struct wl_interface xdg_surface_interface = {
	"xdg_surface", 4,
	5, xdg_surface_requests,
	1, xdg_surface_events,
};

WL_PRIVATE const struct wl_interface xdg_toplevel_interface = {
	"xdg_toplevel", 4,
	14, xdg_toplevel_requests,
	3, xdg_toplevel_events,
};

WL_PRIVATE const struct wl_interface xdg_popup_interface = {
	"xdg_popup", 4,
	3, xdg_popup_requests,
	3, xdg_popup_events,
};



WL_PRIVATE const struct wl_interface xdg_decoration_manager_interface = {
	"zxdg_decoration_manager_v1", 1,
	2, xdg_decoration_manager_requests,
	0, NULL,
};

WL_PRIVATE const struct wl_interface xdg_toplevel_decoration_interface = {
	"zxdg_toplevel_decoration_v1", 1,
	3, xdg_toplevel_decoration_requests,
	1, xdg_toplevel_decoration_events,
};
