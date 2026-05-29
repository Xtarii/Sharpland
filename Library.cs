using System.Runtime.InteropServices;

namespace Sharpland;

public class Sharpland {

    private static Sharpland? Instance;

    private WaylandDisplay display;
    private WaylandRegistry registry;
    private IntPtr compositor;



    public Sharpland(string? name) {
        Instance = this;
        display = new(name);
        registry = display.GetRegistry();



        // Adds listener
        unsafe {
            Wayland.Listener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            registry.AddListener(&listener, 0);
        }

        display.RoundTrip();
    }

    public Sharpland() : this(null) {}



    public void Destroy() {
        display.Dispose();
    }



    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr i, uint version) {
        string? @interface = Marshal.PtrToStringAnsi(i);
        if(@interface == null) return;



        if(@interface == "wl_compositor") {
            Instance?.compositor = Instance.registry.Bind(WaylandInterface.Compositor(), name, 1);
        }
    }
    static unsafe void Remove(void *data, IntPtr registry, uint name) {}
}
