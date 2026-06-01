namespace Sharpland.wayland.enums;

/**
 * pixel formats
 *
 * This describes the memory layout of an individual pixel.
 *
 * All renderers should support argb8888 and xrgb8888 but any other
 * formats are optional and may not be supported by the particular
 * renderer in use.
 *
 * The drm format codes match the macros defined in drm_fourcc.h, except
 * argb8888 and xrgb8888. The formats actually supported by the compositor
 * will be reported by the format event.
 *
 * For all wl_shm formats and unless specified in another protocol
 * extension, pre-multiplied alpha is used for pixel values.
 */
internal enum SharedMemoryFormat {
	/**
	 * 32-bit ARGB format, [31:0] A:R:G:B 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_ARGB8888 = 0,
	/**
	 * 32-bit RGB format, [31:0] x:R:G:B 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_XRGB8888 = 1,
	/**
	 * 8-bit color index format, [7:0] C
	 */
	WL_SHM_FORMAT_C8 = 0x20203843,
	/**
	 * 8-bit RGB format, [7:0] R:G:B 3:3:2
	 */
	WL_SHM_FORMAT_RGB332 = 0x38424752,
	/**
	 * 8-bit BGR format, [7:0] B:G:R 2:3:3
	 */
	WL_SHM_FORMAT_BGR233 = 0x38524742,
	/**
	 * 16-bit xRGB format, [15:0] x:R:G:B 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_XRGB4444 = 0x32315258,
	/**
	 * 16-bit xBGR format, [15:0] x:B:G:R 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_XBGR4444 = 0x32314258,
	/**
	 * 16-bit RGBx format, [15:0] R:G:B:x 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_RGBX4444 = 0x32315852,
	/**
	 * 16-bit BGRx format, [15:0] B:G:R:x 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_BGRX4444 = 0x32315842,
	/**
	 * 16-bit ARGB format, [15:0] A:R:G:B 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_ARGB4444 = 0x32315241,
	/**
	 * 16-bit ABGR format, [15:0] A:B:G:R 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_ABGR4444 = 0x32314241,
	/**
	 * 16-bit RBGA format, [15:0] R:G:B:A 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_RGBA4444 = 0x32314152,
	/**
	 * 16-bit BGRA format, [15:0] B:G:R:A 4:4:4:4 little endian
	 */
	WL_SHM_FORMAT_BGRA4444 = 0x32314142,
	/**
	 * 16-bit xRGB format, [15:0] x:R:G:B 1:5:5:5 little endian
	 */
	WL_SHM_FORMAT_XRGB1555 = 0x35315258,
	/**
	 * 16-bit xBGR 1555 format, [15:0] x:B:G:R 1:5:5:5 little endian
	 */
	WL_SHM_FORMAT_XBGR1555 = 0x35314258,
	/**
	 * 16-bit RGBx 5551 format, [15:0] R:G:B:x 5:5:5:1 little endian
	 */
	WL_SHM_FORMAT_RGBX5551 = 0x35315852,
	/**
	 * 16-bit BGRx 5551 format, [15:0] B:G:R:x 5:5:5:1 little endian
	 */
	WL_SHM_FORMAT_BGRX5551 = 0x35315842,
	/**
	 * 16-bit ARGB 1555 format, [15:0] A:R:G:B 1:5:5:5 little endian
	 */
	WL_SHM_FORMAT_ARGB1555 = 0x35315241,
	/**
	 * 16-bit ABGR 1555 format, [15:0] A:B:G:R 1:5:5:5 little endian
	 */
	WL_SHM_FORMAT_ABGR1555 = 0x35314241,
	/**
	 * 16-bit RGBA 5551 format, [15:0] R:G:B:A 5:5:5:1 little endian
	 */
	WL_SHM_FORMAT_RGBA5551 = 0x35314152,
	/**
	 * 16-bit BGRA 5551 format, [15:0] B:G:R:A 5:5:5:1 little endian
	 */
	WL_SHM_FORMAT_BGRA5551 = 0x35314142,
	/**
	 * 16-bit RGB 565 format, [15:0] R:G:B 5:6:5 little endian
	 */
	WL_SHM_FORMAT_RGB565 = 0x36314752,
	/**
	 * 16-bit BGR 565 format, [15:0] B:G:R 5:6:5 little endian
	 */
	WL_SHM_FORMAT_BGR565 = 0x36314742,
	/**
	 * 24-bit RGB format, [23:0] R:G:B little endian
	 */
	WL_SHM_FORMAT_RGB888 = 0x34324752,
	/**
	 * 24-bit BGR format, [23:0] B:G:R little endian
	 */
	WL_SHM_FORMAT_BGR888 = 0x34324742,
	/**
	 * 32-bit xBGR format, [31:0] x:B:G:R 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_XBGR8888 = 0x34324258,
	/**
	 * 32-bit RGBx format, [31:0] R:G:B:x 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_RGBX8888 = 0x34325852,
	/**
	 * 32-bit BGRx format, [31:0] B:G:R:x 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_BGRX8888 = 0x34325842,
	/**
	 * 32-bit ABGR format, [31:0] A:B:G:R 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_ABGR8888 = 0x34324241,
	/**
	 * 32-bit RGBA format, [31:0] R:G:B:A 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_RGBA8888 = 0x34324152,
	/**
	 * 32-bit BGRA format, [31:0] B:G:R:A 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_BGRA8888 = 0x34324142,
	/**
	 * 32-bit xRGB format, [31:0] x:R:G:B 2:10:10:10 little endian
	 */
	WL_SHM_FORMAT_XRGB2101010 = 0x30335258,
	/**
	 * 32-bit xBGR format, [31:0] x:B:G:R 2:10:10:10 little endian
	 */
	WL_SHM_FORMAT_XBGR2101010 = 0x30334258,
	/**
	 * 32-bit RGBx format, [31:0] R:G:B:x 10:10:10:2 little endian
	 */
	WL_SHM_FORMAT_RGBX1010102 = 0x30335852,
	/**
	 * 32-bit BGRx format, [31:0] B:G:R:x 10:10:10:2 little endian
	 */
	WL_SHM_FORMAT_BGRX1010102 = 0x30335842,
	/**
	 * 32-bit ARGB format, [31:0] A:R:G:B 2:10:10:10 little endian
	 */
	WL_SHM_FORMAT_ARGB2101010 = 0x30335241,
	/**
	 * 32-bit ABGR format, [31:0] A:B:G:R 2:10:10:10 little endian
	 */
	WL_SHM_FORMAT_ABGR2101010 = 0x30334241,
	/**
	 * 32-bit RGBA format, [31:0] R:G:B:A 10:10:10:2 little endian
	 */
	WL_SHM_FORMAT_RGBA1010102 = 0x30334152,
	/**
	 * 32-bit BGRA format, [31:0] B:G:R:A 10:10:10:2 little endian
	 */
	WL_SHM_FORMAT_BGRA1010102 = 0x30334142,
	/**
	 * packed YCbCr format, [31:0] Cr0:Y1:Cb0:Y0 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_YUYV = 0x56595559,
	/**
	 * packed YCbCr format, [31:0] Cb0:Y1:Cr0:Y0 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_YVYU = 0x55595659,
	/**
	 * packed YCbCr format, [31:0] Y1:Cr0:Y0:Cb0 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_UYVY = 0x59565955,
	/**
	 * packed YCbCr format, [31:0] Y1:Cb0:Y0:Cr0 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_VYUY = 0x59555956,
	/**
	 * packed AYCbCr format, [31:0] A:Y:Cb:Cr 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_AYUV = 0x56555941,
	/**
	 * 2 plane YCbCr Cr:Cb format, 2x2 subsampled Cr:Cb plane
	 */
	WL_SHM_FORMAT_NV12 = 0x3231564e,
	/**
	 * 2 plane YCbCr Cb:Cr format, 2x2 subsampled Cb:Cr plane
	 */
	WL_SHM_FORMAT_NV21 = 0x3132564e,
	/**
	 * 2 plane YCbCr Cr:Cb format, 2x1 subsampled Cr:Cb plane
	 */
	WL_SHM_FORMAT_NV16 = 0x3631564e,
	/**
	 * 2 plane YCbCr Cb:Cr format, 2x1 subsampled Cb:Cr plane
	 */
	WL_SHM_FORMAT_NV61 = 0x3136564e,
	/**
	 * 3 plane YCbCr format, 4x4 subsampled Cb (1) and Cr (2) planes
	 */
	WL_SHM_FORMAT_YUV410 = 0x39565559,
	/**
	 * 3 plane YCbCr format, 4x4 subsampled Cr (1) and Cb (2) planes
	 */
	WL_SHM_FORMAT_YVU410 = 0x39555659,
	/**
	 * 3 plane YCbCr format, 4x1 subsampled Cb (1) and Cr (2) planes
	 */
	WL_SHM_FORMAT_YUV411 = 0x31315559,
	/**
	 * 3 plane YCbCr format, 4x1 subsampled Cr (1) and Cb (2) planes
	 */
	WL_SHM_FORMAT_YVU411 = 0x31315659,
	/**
	 * 3 plane YCbCr format, 2x2 subsampled Cb (1) and Cr (2) planes
	 */
	WL_SHM_FORMAT_YUV420 = 0x32315559,
	/**
	 * 3 plane YCbCr format, 2x2 subsampled Cr (1) and Cb (2) planes
	 */
	WL_SHM_FORMAT_YVU420 = 0x32315659,
	/**
	 * 3 plane YCbCr format, 2x1 subsampled Cb (1) and Cr (2) planes
	 */
	WL_SHM_FORMAT_YUV422 = 0x36315559,
	/**
	 * 3 plane YCbCr format, 2x1 subsampled Cr (1) and Cb (2) planes
	 */
	WL_SHM_FORMAT_YVU422 = 0x36315659,
	/**
	 * 3 plane YCbCr format, non-subsampled Cb (1) and Cr (2) planes
	 */
	WL_SHM_FORMAT_YUV444 = 0x34325559,
	/**
	 * 3 plane YCbCr format, non-subsampled Cr (1) and Cb (2) planes
	 */
	WL_SHM_FORMAT_YVU444 = 0x34325659,
	/**
	 * [7:0] R
	 */
	WL_SHM_FORMAT_R8 = 0x20203852,
	/**
	 * [15:0] R little endian
	 */
	WL_SHM_FORMAT_R16 = 0x20363152,
	/**
	 * [15:0] R:G 8:8 little endian
	 */
	WL_SHM_FORMAT_RG88 = 0x38384752,
	/**
	 * [15:0] G:R 8:8 little endian
	 */
	WL_SHM_FORMAT_GR88 = 0x38385247,
	/**
	 * [31:0] R:G 16:16 little endian
	 */
	WL_SHM_FORMAT_RG1616 = 0x32334752,
	/**
	 * [31:0] G:R 16:16 little endian
	 */
	WL_SHM_FORMAT_GR1616 = 0x32335247,
	/**
	 * [63:0] x:R:G:B 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_XRGB16161616F = 0x48345258,
	/**
	 * [63:0] x:B:G:R 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_XBGR16161616F = 0x48344258,
	/**
	 * [63:0] A:R:G:B 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_ARGB16161616F = 0x48345241,
	/**
	 * [63:0] A:B:G:R 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_ABGR16161616F = 0x48344241,
	/**
	 * [31:0] X:Y:Cb:Cr 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_XYUV8888 = 0x56555958,
	/**
	 * [23:0] Cr:Cb:Y 8:8:8 little endian
	 */
	WL_SHM_FORMAT_VUY888 = 0x34325556,
	/**
	 * Y followed by U then V, 10:10:10. Non-linear modifier only
	 */
	WL_SHM_FORMAT_VUY101010 = 0x30335556,
	/**
	 * [63:0] Cr0:0:Y1:0:Cb0:0:Y0:0 10:6:10:6:10:6:10:6 little endian per 2 Y pixels
	 */
	WL_SHM_FORMAT_Y210 = 0x30313259,
	/**
	 * [63:0] Cr0:0:Y1:0:Cb0:0:Y0:0 12:4:12:4:12:4:12:4 little endian per 2 Y pixels
	 */
	WL_SHM_FORMAT_Y212 = 0x32313259,
	/**
	 * [63:0] Cr0:Y1:Cb0:Y0 16:16:16:16 little endian per 2 Y pixels
	 */
	WL_SHM_FORMAT_Y216 = 0x36313259,
	/**
	 * [31:0] A:Cr:Y:Cb 2:10:10:10 little endian
	 */
	WL_SHM_FORMAT_Y410 = 0x30313459,
	/**
	 * [63:0] A:0:Cr:0:Y:0:Cb:0 12:4:12:4:12:4:12:4 little endian
	 */
	WL_SHM_FORMAT_Y412 = 0x32313459,
	/**
	 * [63:0] A:Cr:Y:Cb 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_Y416 = 0x36313459,
	/**
	 * [31:0] X:Cr:Y:Cb 2:10:10:10 little endian
	 */
	WL_SHM_FORMAT_XVYU2101010 = 0x30335658,
	/**
	 * [63:0] X:0:Cr:0:Y:0:Cb:0 12:4:12:4:12:4:12:4 little endian
	 */
	WL_SHM_FORMAT_XVYU12_16161616 = 0x36335658,
	/**
	 * [63:0] X:Cr:Y:Cb 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_XVYU16161616 = 0x38345658,
	/**
	 * [63:0]   A3:A2:Y3:0:Cr0:0:Y2:0:A1:A0:Y1:0:Cb0:0:Y0:0  1:1:8:2:8:2:8:2:1:1:8:2:8:2:8:2 little endian
	 */
	WL_SHM_FORMAT_Y0L0 = 0x304c3059,
	/**
	 * [63:0]   X3:X2:Y3:0:Cr0:0:Y2:0:X1:X0:Y1:0:Cb0:0:Y0:0  1:1:8:2:8:2:8:2:1:1:8:2:8:2:8:2 little endian
	 */
	WL_SHM_FORMAT_X0L0 = 0x304c3058,
	/**
	 * [63:0]   A3:A2:Y3:Cr0:Y2:A1:A0:Y1:Cb0:Y0  1:1:10:10:10:1:1:10:10:10 little endian
	 */
	WL_SHM_FORMAT_Y0L2 = 0x324c3059,
	/**
	 * [63:0]   X3:X2:Y3:Cr0:Y2:X1:X0:Y1:Cb0:Y0  1:1:10:10:10:1:1:10:10:10 little endian
	 */
	WL_SHM_FORMAT_X0L2 = 0x324c3058,
	WL_SHM_FORMAT_YUV420_8BIT = 0x38305559,
	WL_SHM_FORMAT_YUV420_10BIT = 0x30315559,
	WL_SHM_FORMAT_XRGB8888_A8 = 0x38415258,
	WL_SHM_FORMAT_XBGR8888_A8 = 0x38414258,
	WL_SHM_FORMAT_RGBX8888_A8 = 0x38415852,
	WL_SHM_FORMAT_BGRX8888_A8 = 0x38415842,
	WL_SHM_FORMAT_RGB888_A8 = 0x38413852,
	WL_SHM_FORMAT_BGR888_A8 = 0x38413842,
	WL_SHM_FORMAT_RGB565_A8 = 0x38413552,
	WL_SHM_FORMAT_BGR565_A8 = 0x38413542,
	/**
	 * non-subsampled Cr:Cb plane
	 */
	WL_SHM_FORMAT_NV24 = 0x3432564e,
	/**
	 * non-subsampled Cb:Cr plane
	 */
	WL_SHM_FORMAT_NV42 = 0x3234564e,
	/**
	 * 2x1 subsampled Cr:Cb plane, 10 bit per channel
	 */
	WL_SHM_FORMAT_P210 = 0x30313250,
	/**
	 * 2x2 subsampled Cr:Cb plane 10 bits per channel
	 */
	WL_SHM_FORMAT_P010 = 0x30313050,
	/**
	 * 2x2 subsampled Cr:Cb plane 12 bits per channel
	 */
	WL_SHM_FORMAT_P012 = 0x32313050,
	/**
	 * 2x2 subsampled Cr:Cb plane 16 bits per channel
	 */
	WL_SHM_FORMAT_P016 = 0x36313050,
	/**
	 * [63:0] A:x:B:x:G:x:R:x 10:6:10:6:10:6:10:6 little endian
	 */
	WL_SHM_FORMAT_AXBXGXRX106106106106 = 0x30314241,
	/**
	 * 2x2 subsampled Cr:Cb plane
	 */
	WL_SHM_FORMAT_NV15 = 0x3531564e,
	WL_SHM_FORMAT_Q410 = 0x30313451,
	WL_SHM_FORMAT_Q401 = 0x31303451,
	/**
	 * [63:0] x:R:G:B 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_XRGB16161616 = 0x38345258,
	/**
	 * [63:0] x:B:G:R 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_XBGR16161616 = 0x38344258,
	/**
	 * [63:0] A:R:G:B 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_ARGB16161616 = 0x38345241,
	/**
	 * [63:0] A:B:G:R 16:16:16:16 little endian
	 */
	WL_SHM_FORMAT_ABGR16161616 = 0x38344241,
	/**
	 * [7:0] C0:C1:C2:C3:C4:C5:C6:C7 1:1:1:1:1:1:1:1 eight pixels/byte
	 */
	WL_SHM_FORMAT_C1 = 0x20203143,
	/**
	 * [7:0] C0:C1:C2:C3 2:2:2:2 four pixels/byte
	 */
	WL_SHM_FORMAT_C2 = 0x20203243,
	/**
	 * [7:0] C0:C1 4:4 two pixels/byte
	 */
	WL_SHM_FORMAT_C4 = 0x20203443,
	/**
	 * [7:0] D0:D1:D2:D3:D4:D5:D6:D7 1:1:1:1:1:1:1:1 eight pixels/byte
	 */
	WL_SHM_FORMAT_D1 = 0x20203144,
	/**
	 * [7:0] D0:D1:D2:D3 2:2:2:2 four pixels/byte
	 */
	WL_SHM_FORMAT_D2 = 0x20203244,
	/**
	 * [7:0] D0:D1 4:4 two pixels/byte
	 */
	WL_SHM_FORMAT_D4 = 0x20203444,
	/**
	 * [7:0] D
	 */
	WL_SHM_FORMAT_D8 = 0x20203844,
	/**
	 * [7:0] R0:R1:R2:R3:R4:R5:R6:R7 1:1:1:1:1:1:1:1 eight pixels/byte
	 */
	WL_SHM_FORMAT_R1 = 0x20203152,
	/**
	 * [7:0] R0:R1:R2:R3 2:2:2:2 four pixels/byte
	 */
	WL_SHM_FORMAT_R2 = 0x20203252,
	/**
	 * [7:0] R0:R1 4:4 two pixels/byte
	 */
	WL_SHM_FORMAT_R4 = 0x20203452,
	/**
	 * [15:0] x:R 6:10 little endian
	 */
	WL_SHM_FORMAT_R10 = 0x20303152,
	/**
	 * [15:0] x:R 4:12 little endian
	 */
	WL_SHM_FORMAT_R12 = 0x20323152,
	/**
	 * [31:0] A:Cr:Cb:Y 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_AVUY8888 = 0x59555641,
	/**
	 * [31:0] X:Cr:Cb:Y 8:8:8:8 little endian
	 */
	WL_SHM_FORMAT_XVUY8888 = 0x59555658,
	/**
	 * 2x2 subsampled Cr:Cb plane 10 bits per channel packed
	 */
	WL_SHM_FORMAT_P030 = 0x30333050,
}
