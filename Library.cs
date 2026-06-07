using System.Runtime.InteropServices;
using Sharpland.assembly.wayland.renderer;
using Sharpland.assembly.wayland.shm;
using Sharpland.assembly.xdg;
using Sharpland.assembly.xdg.surface;
using Sharpland.wayland;
using Sharpland.wayland.buffers;
using Sharpland.wayland.enums;
using Sharpland.wayland.registry;
using Sharpland.wayland.surface;
using Sharpland.xdg;

namespace Sharpland;

public class Sharpland {

    const int width = 1920, height = 1080;
    const int stride = width * 4;
    const int shm_pool_size = height * stride * 2;



    private GCHandle instance;



    private Display display;
    public int Dispatch() => display.Dispatch();



    private Registry<GCHandle> registry;



    private XDGSurface<GCHandle> surface;

    private WaylandCompositor compositor = null!;
    private WaylandSharedMemory sharedMemory = null!;

    private Buffer<GCHandle> buffer = null!;

    private XDGBase<GCHandle> @base = null!;

    private XDGTopLevel topLevel;



    public Sharpland() {
        instance = GCHandle.Alloc(this);
        display = new();

        registry = display.GetRegistry<GCHandle>();
        registry.AddListener(Global, Remove, ref instance);
        display.RoundTrip();

        if(compositor == null || sharedMemory == null)
            throw new Exception("No compositor or SHM");

        surface = new(new Surface(compositor), @base);
        surface.AddListener(Configure, ref instance);

        topLevel = new(surface) {
            Title = "Test Window"
        };
        surface.Surface.Commit();
    }





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



    static void Global(GCHandle data, Registry<GCHandle> registry, uint name, string @interface, uint version) {
        Sharpland? instance = (Sharpland?)data.Target;
        if(instance == null) return;


        if(@interface == "wl_compositor") {
            instance.compositor = WaylandCompositor.Create(instance.registry, name, 1);
        } else if(@interface == "wl_shm") {
            instance.sharedMemory = WaylandSharedMemory.Create(instance.registry, name, 1);

        } else if(@interface == "xdg_wm_base") {
            instance.@base = new(instance.registry, name, 1);
            instance.@base.AddListener(Ping, ref instance.instance);

        } else {
            Console.WriteLine($"UNSET INTERFACE: {@interface}");
        }
    }

    static void Remove(GCHandle data, Registry<GCHandle> registry, uint name) {}



    static void Ping(GCHandle data, XDGBase<GCHandle> @base, uint serial) {
        Sharpland? instance = (Sharpland?)data.Target;
        if(instance == null) return;

        instance.@base.Pong(serial);
    }



    static void Configure(GCHandle data, XDGSurface<GCHandle> surface, uint serial) {
        Sharpland? instance = (Sharpland?)data.Target;
        if(instance == null) return;


        instance.surface.AckConfigure(serial);

        instance.buffer = DrawFrame(instance);
        instance.surface.Surface.Attach(instance.buffer.Instance, 0, 0);
        instance.surface.Surface.Commit();
    }





    internal unsafe static Buffer<GCHandle> DrawFrame(Sharpland instance) {
        const int width = 640, height = 480;
        uint stride = width * 4;
        uint size = stride * height;

        int fd = instance.AllocSHM(size);
        uint * data = (uint*)instance.sharedMemory.Map(fd, size);

        WaylandSharedMemoryPool pool = new(instance.sharedMemory, fd, size);
        Buffer<GCHandle> buffer = pool.CreateBuffer<Buffer<GCHandle>>(0, width, height, (int)stride, SharedMemoryFormat.WL_SHM_FORMAT_XRGB8888);
        pool.Dispose();
        instance.sharedMemory.Close(fd);

        for(int y = 0; y < height; ++y) {
            for(int x = 0; x < width; ++x) {
                data[y * width + x] = 0xFF333343;
            }
        }

        instance.sharedMemory.MunMap(data, (int)size);

        buffer.AddListener(DestroyBuffer, ref instance.instance);

        return buffer;
    }



    public static void DestroyBuffer(GCHandle data, Buffer<GCHandle> buffer) {
        Sharpland? instance = (Sharpland?)data.Target;
        if(instance == null) return;

        instance.buffer.Dispose();
    }
}
