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
    private WaylandRegistry registry;

    private XDGSurface surface;

    private WaylandCompositor? compositor;
    private WaylandSharedMemory? sharedMemory;
    private IntPtr shmPool;
    private IntPtr buffer;

    private XDGBase @base = null!;



    public Sharpland(string? name) {
        instance = GCHandle.Alloc(this);
        display = new(name);
        registry = display.GetRegistry();



        // Adds listener
        unsafe {
            Wayland.Listener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            registry.AddListener(&listener, GCHandle.ToIntPtr(instance).ToPointer());

            display.RoundTrip();
            if(compositor == null || sharedMemory == null)
                throw new Exception("No compositor or SHM");

            surface = new(compositor, @base);
            Console.WriteLine("Surface created!");

            int fd = AllocSHM(shm_pool_size);
            void * pool = sharedMemory.Map(fd, shm_pool_size);
            shmPool = sharedMemory.CreatePool(fd, shm_pool_size);

            int index = 0;
            int offset = height * stride * index;
            buffer = sharedMemory.CreateBuffer(shmPool, offset, width, height, stride, SharedMemoryFormat.WL_SHM_FORMAT_XRGB8888);
        }

        surface.Attach(buffer, 0, 0);
        surface.Damage();
        surface.Commit();
    }

    public Sharpland() : this(null) {}





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

        return ret;
    }





    public void Destroy() {
        surface.Dispose();
        display.Dispose();
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

        } else {
            Console.WriteLine($"UNSET INTERFACE: {@interface}");
        }
    }
    static unsafe void Remove(void *data, IntPtr registry, uint name) {}
}
