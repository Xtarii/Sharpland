files=xdg/xdg.c xdg/xdg-client-protocol.c wrapper.c

build: xdg/* wrapper.c
	gcc -shared -fPIC -lwayland-client -o ../lib/libwayland-wrapper.so $(files)
