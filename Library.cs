using System.Runtime.InteropServices;
using Sharpland.wayland;
using Sharpland.wayland.enums;
using Sharpland.xdg;

namespace Sharpland;

public class Sharpland {

    const int width = 1920, height = 1080;
    const int stride = width * 4;
    const int shm_pool_size = height * stride * 2;



    private GCHandle instance;



    private WaylandDisplay display;
    public int Dispatch() => display.Dispatch();



    private WaylandRegistry registry;

    private XDGSurface surface;

    private WaylandCompositor? compositor;
    private WaylandSharedMemory sharedMemory = null!;

    private WaylandBuffer buffer = null!;
    private Wayland.BufferListener bufferListener;

    private XDGBase @base = null!;

    private XDG.XDGBaseListener baseListener;
    private XDG.XDGSurfaceListener surfaceListener;

    private XDGTopLevel topLevel;



    public Sharpland(string? name) {
        instance = GCHandle.Alloc(this);
        display = new(name);
        registry = display.GetRegistry();



        // Adds listener
        unsafe {
            Wayland.RegistryListener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            registry.AddListener(&listener, GCHandle.ToIntPtr(instance).ToPointer());

            display.RoundTrip();
            if(compositor == null || sharedMemory == null)
                throw new Exception("No compositor or SHM");



            surfaceListener = new() {
                Configure = &Configure
            };

            fixed(XDG.XDGSurfaceListener * sl = &surfaceListener) {
                surface = new(compositor, @base);
                surface.AddListener(sl, GCHandle.ToIntPtr(instance).ToPointer());
            }
        }

        topLevel = new(surface) {
            Title = "Test Window"
        };
        surface.Commit();
    }

    public Sharpland() : this(null) {}





#region SHM Allocation
    public int OpenSHM() {
        if(sharedMemory == null) throw new Exception("No memory object created.");

        int rt = 100;
        do {
            rt--;

            int fd = sharedMemory.Open("/wl_shm-SLTEST");
            if(fd >= 0) {
                sharedMemory.Unlink("/wl_shm-SLTEST");
                return fd;
            }

        } while(rt > 0);
        throw new Exception("Could not open shared memory.");
    }

    public int AllocSHM(ulong size) {
        if(sharedMemory == null) throw new Exception("No memory object created.");
        int fd = OpenSHM();

        int ret;
        do {
            ret = sharedMemory.FileTruncate(fd, size);
        } while(ret < 0);

        if(ret < 0) {
            sharedMemory.Close(fd);
            throw new Exception("Failed to truncate memory.");
        }

        return fd;
    }
#endregion





    public void Destroy() {
        surface.Dispose();
        display.Dispose();
        instance.Free();
    }



    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        IntPtr ptr = new(data);
        Sharpland? instance = (Sharpland?)GCHandle.FromIntPtr(ptr).Target;
        string? @interface = Marshal.PtrToStringAnsi(i);

        if(@interface == null || instance == null) return;



        if(@interface == "wl_compositor") {
            instance.compositor = new(instance.registry, name, 1);
        } else if(@interface == "wl_shm") {
            instance.sharedMemory = new(instance.registry, name, 1);

        } else if(@interface == "xdg_wm_base") {
            instance.@base = new(instance.registry, name, 1);

            // Add listener
            instance.baseListener = new() { Ping = &Ping };
            fixed(XDG.XDGBaseListener *listener = &instance.baseListener) {
                instance.@base.AddListener(listener, data);
            }

        } else {
            Console.WriteLine($"UNSET INTERFACE: {@interface}");
        }
    }
    static unsafe void Remove(void *data, IntPtr registry, uint name) {}



    static unsafe void Ping(void *data, IntPtr @base, uint serial) {
        IntPtr ptr = new(data);
        Sharpland? instance = (Sharpland?)GCHandle.FromIntPtr(ptr).Target;
        if(instance == null) return;



        instance.@base.Pong(serial);
    }



    static unsafe void Configure(void *data, IntPtr surface, uint serial) {
        IntPtr ptr = new(data);
        Sharpland? instance = (Sharpland?)GCHandle.FromIntPtr(ptr).Target;
        if(instance == null) return;



        instance.surface.AckConfigure(serial);

        instance.buffer = DrawFrame(instance, data);
        instance.surface.Attach(instance.buffer.Instance, 0, 0);
        instance.surface.Commit();
    }





    internal unsafe static WaylandBuffer DrawFrame(Sharpland instance, void *d) {
        const int width = 640, height = 480;
        uint stride = width * 4;
        uint size = stride * height;

        int fd = instance.AllocSHM(size);
        uint * data = (uint*)instance.sharedMemory.Map(fd, size);

        WaylandSharedMemoryPool pool = new(instance.sharedMemory, fd, size);
        WaylandBuffer buffer = pool.CreateBuffer(0, width, height, (int)stride, SharedMemoryFormat.WL_SHM_FORMAT_XRGB8888);
        pool.Dispose();
        instance.sharedMemory.Close(fd);

        for(int y = 0; y < height; ++y) {
            for(int x = 0; x < width; ++x) {
                if ((x + y / 8 * 8) % 16 < 8)
                    data[y * width + x] = 0xFF666666;
                else
                    data[y * width + x] = 0xFFEEEEEE;
            }
        }

        instance.sharedMemory.MunMap(data, (int)size);

        instance.bufferListener = new() {
            Release = &DestroyBuffer
        };

        fixed(Wayland.BufferListener *listener = &instance.bufferListener) {
            buffer.AddListener(listener, d);
        }

        return buffer;
    }



    public static unsafe void DestroyBuffer(void *data, IntPtr buffer) {
        IntPtr ptr = new(data);
        Sharpland? instance = (Sharpland?)GCHandle.FromIntPtr(ptr).Target;
        if(instance == null) return;

        instance.buffer.Dispose();
    }
}
