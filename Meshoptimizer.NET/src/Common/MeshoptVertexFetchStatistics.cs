using System.Runtime.InteropServices;

namespace Meshoptimizer;

[StructLayout(LayoutKind.Sequential)]
public struct MeshoptVertexFetchStatistics
{
    public uint BytesFetched;
    /* fetched bytes / vertex buffer size; best case 1.0 (each byte is fetched once) */
    public float Overfetch;

    public MeshoptVertexFetchStatistics(uint bytesFetched, float overfetch)
    {
        BytesFetched = bytesFetched;
        Overfetch = overfetch;
    }
}
