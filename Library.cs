using System.Runtime.InteropServices;

namespace Sharpland;

public class Sharpland {
    private WaylandDisplay display;
    private WaylandRegistry registry;



    struct Window {
        public int width, height;
    }



    public Sharpland(string? name) {
        display = new(name);
        registry = display.GetRegistry();

        Window window = new();

        // Adds listener
        unsafe {
            Wayland.Listener listener = new() {
                Global = &Global,
                GlobalRemove = &Remove
            };
            registry.AddListener(&listener, window);
        }

        display.RoundTrip();
    }

    public Sharpland() : this(null) {}



    public void Destroy() => display.Dispose();



    static unsafe void Global(void *data, IntPtr registry, uint name, IntPtr @interface, uint version) {
        string i = Marshal.PtrToStringAnsi(@interface) ?? "<none>";
        Console.WriteLine($"INFO {name} : {i} : {version}");

        Window *window = (Window*)data;
        window->width += (int)name;
        window->height += (int)version;

        Console.WriteLine(window->width + " : " + window->height);
    }
    static unsafe void Remove(void *data, IntPtr registry, uint name) {}
}
