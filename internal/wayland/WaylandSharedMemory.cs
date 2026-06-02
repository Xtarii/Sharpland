using System.Runtime.InteropServices;
using Sharpland.wayland.enums;

namespace Sharpland.wayland;

/// <summary>
/// Wayland shared memory object wrapper
/// </summary>
internal partial class WaylandSharedMemory {
    [LibraryImport(Wayland.WRAPPER, StringMarshalling = StringMarshalling.Utf8)]
    private static partial int wrapper_shm_open(string name);
    [LibraryImport(Wayland.WRAPPER, StringMarshalling = StringMarshalling.Utf8)]
    private static partial int wrapper_shm_unlink(string name);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial int wrapper_ftruncate(int file, ulong size);
    [LibraryImport(Wayland.WRAPPER)]
    private static partial int wrapper_close(int file);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial void * wrapper_mmap(int file, ulong size);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial void * wrapper_memset(void * src, int c, ulong n);
    [LibraryImport(Wayland.WRAPPER)]
    private static unsafe partial int wrapper_munmap(void * src, int len);





    /// <summary>
    /// Wayland shared memory object instance
    /// </summary>
    internal IntPtr Instance { get; private set; }



    /// <summary>
    /// Creates shared memory object for Wayland
    /// </summary>
    /// <param name="registry">Wayland registry object</param>
    /// <param name="name">Shared memory interface name</param>
    /// <param name="version">Shared memory interface version</param>
    internal WaylandSharedMemory(WaylandRegistry registry, uint name, uint version) {
        IntPtr @interface = WaylandInterface.SHM();
        Instance = registry.Bind(@interface, name, version);
        if(Instance == IntPtr.Zero)
            throw new ExternalException("Failed to create shared memory for Wayland.");
    }





    /// <summary>
    /// Opens a shared memory object
    /// </summary>
    /// <param name="name">Object name</param>
    /// <returns>Shared memory object file descriptor</returns>
    internal int Open(string name) {
        int fd = wrapper_shm_open(name);
        if(fd < 0)
            throw new AccessViolationException($"Could not open shared memory {name}");
        return fd;
    }

    /// <summary>
    /// Unlinks a shared memory object
    /// </summary>
    /// <param name="name">Object name</param>
    /// <returns>Unlink status</returns>
    internal int Unlink(string name) {
        int res = wrapper_shm_unlink(name);
        if(res < 0)
            throw new AccessViolationException($"Could not unlink shared memory {name}");
        return res;
    }

    /// <summary>
    /// Truncates shared memory object
    /// </summary>
    /// <param name="file">Shared memory object file descriptor</param>
    /// <param name="size">Size to truncate</param>
    /// <returns>Truncated status</returns>
    internal int FileTruncate(int file, ulong size) {
        int res = wrapper_ftruncate(file, size);
        if(res < 0)
            throw new AccessViolationException("Could not truncate shared memory.");
        return res;
    }

    /// <summary>
    /// Closes shared memory object file descriptor
    /// </summary>
    /// <param name="file">Shared memory object file descriptor</param>
    /// <returns>Closing status</returns>
    internal int Close(int file) {
        int res = wrapper_close(file);
        if(res < 0)
            throw new AccessViolationException("Could not close shared memory.");
        return res;
    }

    /// <summary>
    /// Maps memory for a shared memory object
    /// </summary>
    /// <param name="file">Shared memory object file descriptor</param>
    /// <param name="size">Size of memory to map</param>
    /// <returns>A pointer to the mapped memory</returns>
    internal unsafe void * Map(int file, ulong size) {
        void * res = wrapper_mmap(file, size);
        if(res == null)
            throw new AccessViolationException("Failed to memory map.");
        return res;
    }

    /// <summary>
    /// Deallocate any mapping for the region starting
    /// at <c>src</c> and extending <c>length</c> bytes.
    /// <para/>
    /// Returns 0 if successful, -1 for errors.
    /// </summary>
    /// <param name="src">Source address</param>
    /// <param name="length">Length to deallocate in bytes</param>
    /// <returns>Deallocation status</returns>
    internal unsafe int MunMap(void *src, int length) {
        int res = wrapper_munmap(src, length);
        if(res < 0)
            throw new AccessViolationException("Failed to deallocate memory.");
        return res;
    }

    /// <summary>
    /// Set <c>n</c> bytes in <c>src</c> to <c>c</c>
    /// </summary>
    /// <param name="src">Source pointer</param>
    /// <param name="c">Constant to fill memory with</param>
    /// <param name="n">Amount of bytes to set to <c>c</c></param>
    /// <returns>Result</returns>
    internal unsafe void * SetMemory(void *src, int c, ulong n) => wrapper_memset(src, c, n);
}
