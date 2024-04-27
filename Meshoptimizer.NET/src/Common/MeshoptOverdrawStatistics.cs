using System.Runtime.InteropServices;

namespace Meshoptimizer;

[StructLayout(LayoutKind.Sequential)]
public struct MeshoptOverdrawStatistics
{
    public uint PixelsCovered;
    public uint PixelsShaded;
    /* shaded pixels / covered pixels; best case 1.0 */
    public float Overdraw;

    public MeshoptOverdrawStatistics(uint pixels_covered, uint pixels_shaded, float overdraw)
    {
        PixelsCovered = pixels_covered;
        PixelsShaded = pixels_shaded;
        Overdraw = overdraw;
    }
}
