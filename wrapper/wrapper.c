#include <wayland-client-protocol.h>

struct wl_registry * wrapper_wl_display_get_registry(struct wl_display *wl_display) {
    return wl_display_get_registry(wl_display);
}

int wrapper_wl_registry_add_listener(struct wl_registry *wl_registry, const struct wl_registry_listener *listener, void *data) {
    return wl_registry_add_listener(wl_registry, listener, data);
}
