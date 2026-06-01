files=xdg/xdg.c wrapper.c

build: xdg/* wrapper.c
	gcc -shared -fPIC -lwayland-client -o ../lib/libwayland-wrapper.so $(files)
