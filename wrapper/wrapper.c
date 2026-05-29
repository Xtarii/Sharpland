#include <wayland-client-protocol.h>

//
// Wayland interface wrapper
//
// Provides getters for each interface
//

const struct wl_interface * wrapper_wl_compositor_interface() { return &wl_compositor_interface; }



//
// Registry function wrapper
//

struct wl_registry * wrapper_wl_display_get_registry(struct wl_display *wl_display) { return wl_display_get_registry(wl_display); }
int wrapper_wl_registry_add_listener(struct wl_registry *wl_registry, const struct wl_registry_listener *listener, void *data) { return wl_registry_add_listener(wl_registry, listener, data); }
void * wrapper_wl_registry_bind(struct wl_registry *wl_registry, uint32_t name, const struct wl_interface *interface, uint32_t version) { return wl_registry_bind(wl_registry, name, interface, version); }



//
// Surface function wrapper
//

struct wl_surface * wrapper_wl_compositor_create_surface(struct wl_compositor *wl_compositor) { return wl_compositor_create_surface(wl_compositor); }
