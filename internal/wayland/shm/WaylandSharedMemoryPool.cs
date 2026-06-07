using System.Reflection;
using System.Runtime.InteropServices;
using Sharpland.assembly.wayland.buffer;
using Sharpland.wayland.enums;

namespace Sharpland.assembly.wayland.shm;

/// <summary>
/// Wayland shared memory pool object wrapper
/// </summary>
internal partial class WaylandSharedMemoryPool : IDisposable {
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_shm_create_pool(IntPtr shm, int file, ulong size);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial IntPtr wrapper_wl_shm_pool_create_buffer(IntPtr pool, int offset, int width, int height, int stride, uint format);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial void wrapper_wl_shm_pool_destroy(IntPtr pool);





    /// <summary>
    /// Shared memory pool instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates a Wayland shared memory pool object
    /// </summary>
    /// <param name="shm">Shared memory object</param>
    /// <param name="file">File descriptor</param>
    /// <param name="size">Pool size</param>
    internal WaylandSharedMemoryPool(WaylandSharedMemory shm, int file, ulong size) {
        Instance = wrapper_wl_shm_create_pool(shm.Instance, file, size);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create shared memory pool.");
    }

    /// <summary>
    /// Creates a shared memory pool buffer object
    /// </summary>
    /// <param name="offset">Buffer offset</param>
    /// <param name="width">Buffer width</param>
    /// <param name="height">Buffer height</param>
    /// <param name="stride">Buffer stride</param>
    /// <param name="format">Buffer format</param>
    /// <typeparam name="T">Type of buffer to create</typeparam>
    /// <returns>Wayland buffer object</returns>
    internal T CreateBuffer<T>(int offset, int width, int height, int stride, SharedMemoryFormat format) where T : WaylandBuffer {
        IntPtr buffer = wrapper_wl_shm_pool_create_buffer(Instance, offset, width, height, stride, (uint)format);
        if(buffer == IntPtr.Zero)
            throw new ExternalException("Failed to create shared memory pool buffer.");

        ConstructorInfo? constructor = typeof(T).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, [typeof(IntPtr)]) ?? throw new ArgumentException("Invalid buffer constructor. A valid constructor for the buffer was not found.");
        T obj = (T)constructor.Invoke([buffer]);
        return obj;
    }



    public void Dispose() => wrapper_wl_shm_pool_destroy(Instance);
}
