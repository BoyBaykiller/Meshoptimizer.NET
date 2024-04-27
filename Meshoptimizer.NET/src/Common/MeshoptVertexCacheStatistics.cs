using System.Runtime.InteropServices;

namespace Meshoptimizer;

[StructLayout(LayoutKind.Sequential)]
public struct MeshoptVertexCacheStatistics
{
    public uint VerticesTransformed;
    public uint WarpsExecuted;
    public float Acmr; /* transformed vertices / triangle count; best case 0.5, worst case 3.0, optimum depends on topology */
    public float Atvr; /* transformed vertices / vertex count; best case 1.0, worst case 6.0, optimum is 1.0 (each vertex is transformed once) */

    public MeshoptVertexCacheStatistics(uint verticesTransformed, uint warpsExecuted, float acmr, float atvr)
    {
        VerticesTransformed = verticesTransformed;
        WarpsExecuted = warpsExecuted;
        Acmr = acmr;
        Atvr = atvr;
    }
}
