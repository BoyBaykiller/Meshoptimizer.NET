using System.Runtime.InteropServices;

namespace Meshoptimizer;

[StructLayout(LayoutKind.Sequential)]
public struct MeshoptMeshlet
{
    /* offsets within meshlet_vertices and meshlet_triangles arrays with meshlet data */
    public uint VertexOffset;
    public uint TriangleOffset;

    /* number of vertices and triangles used in the meshlet; data is stored in consecutive range defined by offset and count */
    public uint VertexCount;
    public uint TriangleCount;

    public MeshoptMeshlet(uint vertexOffset, uint triangleOffset, uint vertexCount, uint triangleCount)
    {
        VertexOffset = vertexOffset;
        TriangleOffset = triangleOffset;
        VertexCount = vertexCount;
        TriangleCount = triangleCount;
    }
};
