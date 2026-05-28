# Sharpland

C# wrapper of wayland



### Build of wrapper

The wrapper requires the following library
files in a directory called `lib`,
| Library           |
| -------           |
| wayland-client    |
| wayland-wrapper   |

The `wayland wrapper` library is
built from the **wrapper** directory,
```bash
cd wrapper
make build # Places the library file in the lib directory
```

To package the C# library,
```bash
dotnet build
```
