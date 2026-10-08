using Sharpland.assembly.wayland.renderer;
using Sharpland.assembly.wayland.shm;
using Sharpland.assembly.xdg.surface;
using Sharpland.wayland.buffers.events;
using Sharpland.wayland.enums;
using Sharpland.wayland.registry.events;
using Sharpland.wayland.surface;
using Sharpland.window;
using Sharpland.xdg;
using Sharpland.xdg.events;

namespace Sharpland;

public class Sharpland {

    const int width = 1920, height = 1080;
    const int stride = width * 4;
    const int shm_pool_size = height * stride * 2;



    // private GCHandle instance;

    private Sharpland instance;


    private WaylandWindow window;
    public int Dispatch() => window.Display.Dispatch();


    long test = 0;



    private XDGSurface surface;

    private WaylandCompositor compositor = null!;
    private WaylandSharedMemory sharedMemory = null!;

    private wayland.buffers.Buffer buffer = null!;

    private XDGBase @base = null!;

    private XDGTopLevel topLevel;



    public Sharpland() {
        instance = this;

        window = new();
        window.Registry.AddListener(GlobalHandler, RemoveHandler, ref test);
        window.Display.RoundTrip();

        if(compositor == null || sharedMemory == null)
            throw new Exception("No compositor or SHM");

        surface = new(new Surface(compositor), @base);
        surface.AddListener(Configure);

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
        window.Dispose();
    }



    void GlobalHandler(RegistryEvent data) {

        // DEBUG

        Console.WriteLine(data.GetData<long>());
        data.GetData<long>() += 5;  // Adds to data for next call



        // Setup

        if(data.Interface == "wl_compositor") {
            compositor = WaylandCompositor.Create(window.Registry, data.Name, 1);

        } else if(data.Interface == "wl_shm") {
            sharedMemory = WaylandSharedMemory.Create(window.Registry, data.Name, 1);

        } else if(data.Interface == "xdg_wm_base") {
            @base = new(window.Registry, data.Name, 1);
            @base.AddListener(Ping);

        } else {
            Console.WriteLine($"UNSET INTERFACE: {data.Interface}");
        }
    }

    void RemoveHandler(RegistryEvent data) { /* Do nothing */ }

    void Ping(XDGBaseEvent e) => e.Pong();

    void Configure(XDGSurfaceEvent e) {
        e.Owner.AckConfigure(e.Serial);

        buffer = DrawFrame(instance);
        surface.Surface.Attach(buffer.Instance, 0, 0);
        surface.Surface.Commit();
    }

    void DestroyBuffer(BufferEvent e) => e.Owner.Dispose();





    internal unsafe wayland.buffers.Buffer DrawFrame(Sharpland instance) {
        const int width = 640, height = 480;
        uint stride = width * 4;
        uint size = stride * height;

        int fd = instance.AllocSHM(size);
        uint * data = (uint*)instance.sharedMemory.Map(fd, size);

        WaylandSharedMemoryPool pool = new(instance.sharedMemory, fd, size);
        wayland.buffers.Buffer buffer = pool.CreateBuffer<wayland.buffers.Buffer>(0, width, height, (int)stride, SharedMemoryFormat.WL_SHM_FORMAT_XRGB8888);
        pool.Dispose();
        instance.sharedMemory.Close(fd);

        for(int y = 0; y < height; ++y) {
            for(int x = 0; x < width; ++x) {
                data[y * width + x] = 0xFF333343;
            }
        }

        instance.sharedMemory.MunMap(data, (int)size);

        buffer.AddListener(DestroyBuffer);

        return buffer;
    }
}
