#include "xdg/xdg.h"
#include <fcntl.h>
#include <stdint.h>
#include <sys/mman.h>
#include <memory.h>
#include <unistd.h>
#include <wayland-client-protocol.h>

//
// Wayland interface wrapper
//
// Provides getters for each interface
//

const struct wl_interface * wrapper_wl_compositor_interface() { return &wl_compositor_interface; }
const struct wl_interface * wrapper_wl_subcompositor_interface() { return &wl_subcompositor_interface; }
const struct wl_interface * wrapper_wl_seat_interface() { return &wl_seat_interface; }
const struct wl_interface * wrapper_wl_shm_interface() { return &wl_shm_interface; }



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
void wrapper_wl_surface_destroy(struct wl_surface *wl_surface) { wl_surface_destroy(wl_surface); }

void wrapper_wl_surface_attach(struct wl_surface *wl_surface, struct wl_buffer *wl_buffer, int32_t x, int32_t y) { wl_surface_attach(wl_surface, wl_buffer, x, y); }
void wrapper_wl_surface_damage(struct wl_surface *wl_surface, int32_t x, int32_t y, int32_t width, int32_t height) { wl_surface_damage(wl_surface, x, y, width, height); }
void wrapper_wl_surface_commit(struct wl_surface *wl_surface) { wl_surface_commit(wl_surface); }



//
// Seat function wrapper
//

int wrapper_wl_seat_add_listener(struct wl_seat *wl_seat, const struct wl_seat_listener *listener, void *data) { return wl_seat_add_listener(wl_seat, listener, data); }



//
// Shared memory function wrapper
//

int wrapper_shm_open(const char *name) { return shm_open(name, O_RDWR | O_CREAT | O_EXCL, 0600); }
int wrapper_shm_unlink(const char *name) { return shm_unlink(name); }
int wrapper_ftruncate(int file, unsigned long size) { return ftruncate(file, size); }
int wrapper_close(int file) { return close(file); }
void * wrapper_mmap(int file, unsigned long size) { return mmap(NULL, size, PROT_READ | PROT_WRITE, MAP_SHARED, file, 0); }
void * wrapper_memset(void * src, int c, unsigned long n) { return memset(src, c, n); }

struct wl_shm_pool * wrapper_wl_shm_create_pool(struct wl_shm *wl_shm, int fd, unsigned long size) { return wl_shm_create_pool(wl_shm, fd, size); }
struct wl_buffer * wrapper_wl_shm_pool_create_buffer(struct wl_shm_pool *wl_shm_pool, int32_t offset, int32_t width, int32_t height, int32_t stride, uint32_t format) { return wl_shm_pool_create_buffer(wl_shm_pool, offset, width, height, stride, format); }
