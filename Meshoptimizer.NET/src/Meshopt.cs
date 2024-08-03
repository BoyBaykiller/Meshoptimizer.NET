using System.Runtime.InteropServices;

namespace Meshoptimizer
{
    public static unsafe partial class Meshopt
    {
        private const string LIBRARY_NAME = "meshoptimizer";

        /// <summary>
        /// Meshlet is a small mesh cluster (subset) that consists of:<br></br>
        /// * triangles, an 8-bit micro triangle (index) buffer, that for each triangle specifies three local vertices to use;<br></br>
        /// * vertices, a 32-bit vertex indirection buffer, that for each local vertex specifies which mesh vertex to fetch vertex attributes from.<para/>
        /// For efficiency, meshlet triangles and vertices are packed into two large arrays; this structure contains offsets and counts to access the data.
        /// </summary>
        public struct Meshlet
        {
            /* offsets within meshlet_vertices and meshlet_triangles arrays with meshlet data */
            public uint VertexOffset;
            public uint TriangleOffset;

            /* number of vertices and triangles used in the meshlet; data is stored in consecutive range defined by offset and count */
            public uint VertexCount;
            public uint TriangleCount;
        };

        /// <summary>
        /// Vertex attribute stream<para/>
        /// Each element takes size bytes, beginning at data, with stride controlling the spacing between successive elements (stride >= size).
        /// </summary>
        public struct Stream
        {
            public void* Data;
            public nuint Size;
            public nuint Stride;
        };

        /// <summary>
        /// Meshlet builder<para/>
        /// Splits the mesh into a set of meshlets where each meshlet has a micro index buffer indexing into meshlet vertices that refer to the original vertex buffer.
        /// The resulting data can be used to render meshes using NVidia programmable mesh shading pipeline, or in other cluster-based renderers.
        /// When targeting mesh shading hardware, for maximum efficiency meshlets should be further optimized using meshopt_optimizeMeshlet.
        /// When using buildMeshlets, vertex positions need to be provided to minimize the size of the resulting clusters.
        /// When using buildMeshletsScan, for maximum efficiency the index buffer being converted has to be optimized for vertex cache first.
        /// </summary>
        /// <param name="meshlets">Must contain enough space for all meshlets, worst case size can be computed with <seealso cref="BuildMeshletsBound(nuint, nuint, nuint)"/></param>
        /// <param name="meshletVertices">Must contain enough space for all meshlets, worst case size is equal to max_meshlets * max_vertices</param>
        /// <param name="meshletTriangles">Must contain enough space for all meshlets, worst case size is equal to max_meshlets * max_triangles * 3</param>
        /// <param name="indices"></param>
        /// <param name="indexCount"></param>
        /// <param name="vertexPositions">Should have float3 position in the first 12 bytes of each vertex</param>
        /// <param name="vertexCount"></param>
        /// <param name="vertexPositionsStride"></param>
        /// <param name="maxVertices">Must not exceed implementation limits (max_vertices &lt;= 255 - not 256!)</param>
        /// <param name="maxTriangles">Must not exceed implementation limits (max_triangles &lt;= 512; max_triangles must be divisible by 4)</param>
        /// <param name="coneWeight">Should be set to 0 when cone culling is not used, and a value between 0 and 1 otherwise to balance between cluster size and cone culling efficiency</param>
        /// <returns></returns>
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

        /// <inheritdoc cref="BuildMeshlets(ref Meshlet, in uint, in byte, in uint, nuint, in float, nuint, nuint, nuint, nuint, float)"/>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_buildMeshletsBound")]
        public static partial nuint BuildMeshletsBound(nuint indexCount, nuint maxVertices, nuint maxTriangles);

        /// <summary>
        /// Experimental: Meshlet optimizer<para/>
        /// Reorders meshlet vertices and triangles to maximize locality to improve rasterizer throughput
        /// </summary>
        /// <param name="meshletVertices">Must refer to meshlet index data; when buildMeshlets* is used, needs to be computed from meshlet's vertex_offset</param>
        /// <param name="meshletTriangles">Must refer to meshlet triangle data; when buildMeshlets* is used, needs to be computed from meshlet's triangle_offset</param>
        /// <param name="triangleCount">Must not exceed implementation limits (triangle_count &lt;= 512)</param>
        /// <param name="vertexCount">Must not exceed implementation limits (vertex_count &lt;= 255 - not 256!)</param>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeMeshlet")]
        public static partial void OptimizeMeshlet(ref uint meshletVertices, ref byte meshletTriangles, nuint triangleCount, nuint vertexCount);

        /// <summary>
        /// Generates a vertex remap table from multiple vertex streams and an optional index buffer and returns number of unique vertices.<para/>
        /// As a result, all vertices that are binary equivalent map to the same (new) location, with no gaps in the resulting sequence.
        /// Resulting remap table maps old vertices to new vertices and can be used in <seealso cref="RemapIndexBuffer(ref uint, in uint, nuint, in uint)"/> or <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/>.
        /// To remap vertex buffers, you will need to call <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/> for each vertex stream.
        /// Note that binary equivalence considers all size bytes in each stream, including padding which should be zero-initialized.
        /// </summary>
        /// <param name="destination">Must contain enough space for the resulting remap table (<paramref name="vertexCount"/> elements)</param>
        /// <param name="indices">Can be null if the input is unindexed</param>
        /// <param name="indexCount"></param>
        /// <param name="vertexCount"></param>
        /// <param name="streams"></param>
        /// <param name="streamCount">Must be &lt;= 16</param>
        /// <returns></returns>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateVertexRemapMulti")]
        public static partial nuint GenerateVertexRemapMulti(ref uint destination, in uint indices, nuint indexCount, nuint vertexCount, in Stream streams, nuint streamCount);

        /// <summary>
        /// Generate index buffer from the source index buffer and remap table generated by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in Stream, nuint)"/>
        /// </summary>
        /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="indexCount"/> elements)</param>
        /// <param name="indices">Can be null if the input is unindexed</param>
        /// <param name="indexCount"></param>
        /// <param name="remap"></param>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_remapIndexBuffer")]
        public static partial void RemapIndexBuffer(ref uint destination, in uint indices, nuint indexCount, in uint remap);

        /// <summary>
        /// Generates vertex buffer from the source vertex buffer and remap table generated by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in Stream, nuint)"/>
        /// </summary>
        /// <param name="destination">Must contain enough space for the resulting vertex buffer (unique_vertex_count elements, returned by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in Stream, nuint)"/>)</param>
        /// <param name="vertices"></param>
        /// <param name="vertexCount">Should be the initial vertex count and not the value returned by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in Stream, nuint)"/></param>
        /// <param name="vertexSize"></param>
        /// <param name="remap"></param>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_remapVertexBuffer")]
        public static partial void RemapVertexBuffer(void* destination, void* vertices, nuint vertexCount, nuint vertexSize, in uint remap);

        /// <summary>
        /// Vertex transform cache optimizer.<para/>
        /// Reorders indices to reduce the number of GPU vertex shader invocations.
        /// If index buffer contains multiple ranges for multiple draw calls, this functions needs to be called on each range individually.
        /// </summary>
        /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="indexCount"/> elements)</param>
        /// <param name="indices"></param>
        /// <param name="indexCount"></param>
        /// <param name="vertexCount"></param>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexCache")]
        public static partial void OptimizeVertexCache(ref uint destination, in uint indices, nuint indexCount, nuint vertexCount);

        /// <summary>
        /// Vertex fetch cache optimizer.<para/>
        /// Generates vertex remap to reduce the amount of GPU memory fetches during vertex processing.
        /// Returns the number of unique vertices, which is the same as input vertex count unless some vertices are unused.
        /// The resulting remap table should be used to reorder vertex/index buffers using <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/> or <seealso cref="RemapIndexBuffer(ref uint, in uint, nuint, in uint)"/>
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="indices"></param>
        /// <param name="indexCount"></param>
        /// <param name="vertexCount"></param>
        /// <returns></returns>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexFetchRemap")]
        public static partial nuint OptimizeVertexFetchRemap(ref uint destination, in uint indices, nuint indexCount, nuint vertexCount);

        /// <summary>
        /// Mesh stripifier <para/>
        /// Converts a previously vertex cache optimized triangle list to triangle strip, stitching strips using restart index or degenerate triangles.
        /// Returns the number of indices in the resulting strip, with destination containing new index data.
        /// For maximum efficiency the index buffer being converted has to be optimized for vertex cache first.
        /// Using restart indices can result in ~10% smaller index buffers, but on some GPUs restart indices may result in decreased performance.
        /// </summary>
        /// <param name="destination">Must contain enough space for the target index buffer, worst case can be computed with <seealso cref="StripifyBound(nuint)"/></param>
        /// <param name="indices"></param>
        /// <param name="indexCount"></param>
        /// <param name="vertexCount"></param>
        /// <param name="restartIndex">Should be 0xffff or 0xffffffff depending on index size, or 0 to use degenerate triangles</param>
        /// <returns></returns>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_stripify")]
        public static partial nuint Stripify(ref uint destination, in uint indices, nuint indexCount, nuint vertexCount, uint restartIndex);

        /// <inheritdoc cref="Stripify(ref uint, in uint, nuint, nuint, uint)"/>
        [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_stripifyBound")]
        public static partial nuint StripifyBound(nuint indexCount);
    }
}
