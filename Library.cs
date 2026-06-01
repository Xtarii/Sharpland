using System.Runtime.InteropServices;

namespace Sharpland;

public class Sharpland {

    private GCHandle instance;
    private WaylandDisplay display;
    private WaylandRegistry registry;
    private IntPtr compositor;



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
        }

        display.RoundTrip();
    }

    public Sharpland() : this(null) {}



    public void Destroy() {
        display.Dispose();
    }



    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        IntPtr ptr = new(data);
        Sharpland? instance = (Sharpland?)GCHandle.FromIntPtr(ptr).Target;
        string? @interface = Marshal.PtrToStringAnsi(i);

        if(@interface == null || instance == null) return;



        if(@interface == "wl_compositor") {
            IntPtr comp = instance.registry.Bind(WaylandInterface.Compositor(), name, 1);
            instance.compositor = comp;
        } else {
            Console.WriteLine($"UNSET INTERFACE: {@interface}");
        }
    }
    static unsafe void Remove(void *data, IntPtr registry, uint name) {}
}
