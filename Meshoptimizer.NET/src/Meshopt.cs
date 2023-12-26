using System.Runtime.InteropServices;

namespace Meshoptimizer
{
    public static unsafe partial class Meshopt
    {
        private const string LIBRARY_NAME = "meshoptimizer";

        public struct Meshlet
        {
            /* offsets within meshlet_vertices and meshlet_triangles arrays with meshlet data */
            public uint VertexOffset;
            public uint TriangleOffset;

            /* number of vertices and triangles used in the meshlet; data is stored in consecutive range defined by offset and count */
            public uint VertexCount;
            public uint TriangleCount;
        };

        public struct Stream
        {
            public void* Data;
            public nuint Size;
            public nuint Stride;
        };

        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_buildMeshlets")]
        public static partial nuint BuildMeshlets(ref Meshlet meshlets,
            in uint meshletVertices,
            in byte meshletTriangles,
            in uint indices,
            nuint indexCount,
            in float vertexPositions,
            nuint vertexCount,
            nuint vertexPositionsStride,
            nuint maxVertices,
            nuint maxTriangles,
            float coneWeight);


        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_buildMeshletsBound")]
        public static partial nuint BuildMeshletsBound(nuint indexCount, nuint maxVertices, nuint maxTriangles);

        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateVertexRemapMulti")]
        public static partial nuint GenerateVertexRemapMulti(ref uint destination, in uint indices, nuint index_count, nuint vertex_count, in Stream streams, nuint stream_count);

        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_remapIndexBuffer")]
        public static partial void RemapIndexBuffer(ref uint destination, in uint indices, nuint index_count, in uint remap);

        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_remapVertexBuffer")]
        public static partial void RemapVertexBuffer(void* destination, void* vertices, nuint vertex_count, nuint vertex_size, in uint remap);

        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexCache")]
        public static partial void OptimizeVertexCache(ref uint destination, in uint indices, nuint index_count, nuint vertex_count);

        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexFetchRemap")]
        public static partial nuint OptimizeVertexFetchRemap(ref uint destination, in uint indices, nuint index_count, nuint vertex_count);

    }
}
