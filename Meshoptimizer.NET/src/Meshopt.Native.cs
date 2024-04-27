using System.Runtime.InteropServices;

namespace Meshoptimizer;

public static unsafe partial class Meshopt
{
    private const string LIBRARY_NAME = "meshoptimizer";

    #region Generate

    /// <summary>
    /// Generates a vertex remap table from the vertex buffer and an optional index buffer and returns number of unique vertices
    /// As a result, all vertices that are binary equivalent map to the same(new) location, with no gaps in the resulting sequence.
    /// Resulting remap table maps old vertices to new vertices and can be used in 
    /// <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/> and <seealso cref="RemapIndexBuffer(ref uint, in uint, nuint, in uint)"/>.
    /// Note that binary equivalence considers all vertex_size bytes, including padding which should be zero-initialized.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting remap table (<paramref name="vertex_count"/> elements)</param>
    /// <param name="indices">Address for index data, can be <seealso cref="IntPtr.Zero"/> if the input is unindexed.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertices">Address for vertex data.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <param name="vertex_size">The size of each vertex data in bytes.</param>
    /// <returns>New vertices mapped.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateVertexRemap")]
    public static partial nuint GenerateVertexRemap(ref uint destination, in uint indices, nuint index_count, void* vertices, nuint vertex_count, nuint vertex_size);

    /// <inheritdoc cref="GenerateVertexRemap(ref uint, in uint, nuint, void*, nuint, nuint)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateVertexRemap")]
    public static partial nuint GenerateVertexRemap(ref uint destination, in IntPtr indices, nuint index_count, void* vertices, nuint vertex_count, nuint vertex_size);

    /// <summary>
    /// Generates a vertex remap table from multiple vertex streams and an optional index buffer and returns number of unique vertices.<para/>
    /// As a result, all vertices that are binary equivalent map to the same(new) location, with no gaps in the resulting sequence.
    /// Resulting remap table maps old vertices to new vertices and can be used in <seealso cref="RemapIndexBuffer(ref uint, in uint, nuint, in uint)"/>/<seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/>.
    /// To remap vertex buffers, you will need to call <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/> for each vertex stream.
    /// Note that binary equivalence considers all size bytes in each stream, including padding which should be zero-initialized.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting remap table (<paramref name="vertex_count"/> elements).</param>
    /// <param name="indices">Address for index data, can be <seealso cref="IntPtr.Zero"/> if the input is unindexed.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <param name="streams"></param>
    /// <param name="stream_count">Must be &lt;= 16.</param>
    /// <returns>New vertices mapped.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateVertexRemapMulti")]
    public static partial nuint GenerateVertexRemapMulti(ref uint destination, in uint indices, nuint index_count, nuint vertex_count, in MeshoptStream streams, nuint stream_count);

    /// <inheritdoc cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateVertexRemapMulti")]
    public static partial nuint GenerateVertexRemapMulti(ref uint destination, in IntPtr indices, nuint index_count, nuint vertex_count, in MeshoptStream streams, nuint stream_count);


    /// <summary>
    /// Generate index buffer that can be used for more efficient rendering when only a subset of the vertex attributes is necessary
    /// all vertices that are binary equivalent (wrt first vertex_size bytes) map to the first vertex in the original vertex buffer.
    /// This makes it possible to use the index buffer for Z pre-pass or shadowmap rendering, while using the original index buffer for regular rendering.
    /// Note that binary equivalence considers all vertex_size bytes, including padding which should be zero-initialized.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements)./></param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertices">Address for vertex data.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <param name="vertex_size">The size of each vertex data in bytes.</param>
    /// <param name="vertex_stride"></param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateShadowIndexBuffer")]
    public static partial void GenerateShadowIndexBuffer(ref uint destination, in uint indices, nuint index_count, void* vertices, nuint vertex_count, nuint vertex_size, nuint vertex_stride);

    /// <summary>
    /// Generate index buffer that can be used for more efficient rendering when only a subset of the vertex attributes is necessary
    /// all vertices that are binary equivalent (wrt specified streams) map to the first vertex in the original vertex buffer.
    /// This makes it possible to use the index buffer for Z pre-pass or shadowmap rendering, while using the original index buffer for regular rendering.
    /// Note that binary equivalence considers all size bytes in each stream, including padding which should be zero-initialized.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements)./></param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <param name="streams"></param>
    /// <param name="stream_count">Must be &lt;= 16</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateShadowIndexBufferMulti")]
    public static partial void GenerateShadowIndexBufferMulti(ref uint destination, in uint indices, nuint index_count, nuint vertex_count, in MeshoptStream streams, nuint stream_count);

    /// <summary>
    /// Generate index buffer that can be used as a geometry shader input with triangle adjacency topology
    /// Each triangle is converted into a 6-vertex patch with the following layout:
    /// <para>0, 2, 4: original triangle vertices 1, 3, 5: vertices adjacent to edges 02, 24 and 40</para>
    /// </summary>
    /// <remarks>
    /// The resulting patch can be rendered with geometry shaders using e.g. 
    /// <list type="bullet">
    /// <item>OpenGL: GL_TRIANGLES_ADJACENCY</item>
    /// <item>Vulkan: VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST_WITH_ADJACENCY</item>
    /// <item>DirectX: D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST_ADJ</item>
    /// </list>
    /// This can be used to implement algorithms like silhouette detection/expansion and other forms of GS-driven rendering.
    /// </remarks>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> * 2 elements).</param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count">The size of each vertex data in bytes.</param>
    /// <param name="vertex_positions_stride"></param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateAdjacencyIndexBuffer")]
    public static partial void GenerateAdjacencyIndexBuffer(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);

    /// <summary>
    /// Generate index buffer that can be used for PN-AEN tessellation with crack-free displacement
    /// each triangle is converted into a 12-vertex patch with the following layout:
    /// <list type="bullet">
    /// <item>0, 1, 2: original triangle vertices</item>
    /// <item>3, 4: opposing edge for edge 0, 1</item>
    /// <item>5, 6: opposing edge for edge 1, 2</item>
    /// <item>7, 8: opposing edge for edge 2, 0</item>
    /// <item>9, 10, 11: dominant vertices for corners 0, 1, 2</item>
    /// </list>
    /// The resulting patch can be rendered with hardware tessellation using PN-AEN and displacement mapping.
    /// See "Tessellation on Any Budget" (John McDonald, GDC 2011) for implementation details.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> * 4 elements).</param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <param name="vertex_positions_stride"></param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_generateTessellationIndexBuffer")]
    public static partial void GenerateTessellationIndexBuffer(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);
    #endregion

    #region Remap

    /// <summary>
    /// Generates vertex buffer from the source vertex buffer and remap table generated by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting vertex buffer (unique_vertex_count elements, returned by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>).</param>
    /// <param name="vertices">Address for vertex data.</param>
    /// <param name="vertex_count">Should be the initial vertex count and not the value returned by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>.</param>
    /// <param name="vertex_size">The size of each vertex data in bytes.</param>
    /// <param name="remap">Address of the data to be remapped.</param>    
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_remapVertexBuffer")]
    public static partial void RemapVertexBuffer(void* destination, void* vertices, nuint vertex_count, nuint vertex_size, in uint remap);

    /// <summary>
    /// Generate index buffer from the source index buffer and remap table generated by <seealso cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements)</param>
    /// <param name="indices">Address for index data, can be <seealso cref="IntPtr.Zero"/> if the input is unindexed.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="remap">Address of the data to be remapped.</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_remapIndexBuffer")]
    public static partial void RemapIndexBuffer(ref uint destination, in uint indices, nuint index_count, in uint remap);

    /// <inheritdoc cref="RemapIndexBuffer(ref uint, in uint, nuint, in uint)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_remapIndexBuffer")]
    public static partial void RemapIndexBuffer(ref uint destination, in IntPtr indices, nuint index_count, in uint remap);
    #endregion

    #region Optimize
    /// <summary>
    /// Vertex transform cache optimizer.<para/>
    /// Reorders indices to reduce the number of GPU vertex shader invocations if index buffer contains 
    /// multiple ranges for multiple draw calls, this functions needs to be called on each range individually.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements).</param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexCache")]
    public static partial void OptimizeVertexCache(ref uint destination, in uint indices, nuint index_count, nuint vertex_count);

    /// <summary>
    /// Vertex transform cache optimizer for strip-like caches
    /// produces inferior results to <seealso cref="OptimizeVertexCache(ref uint, in uint, nuint, nuint)"/> from the GPU vertex cache perspective However,
    /// the resulting index order is more optimal if the goal is to reduce the triangle strip length or improve compression efficiency.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements).</param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexCacheStrip")]
    public static partial void OptimizeVertexCacheStrip(ref uint destination, in uint indices, nuint index_count, nuint vertex_count);

    /// <summary>
    /// Vertex transform cache optimizer for FIFO caches
    /// reorders indices to reduce the number of GPU vertex shader invocations
    /// generally takes ~3x less time to optimize meshes but produces inferior results compared to <seealso cref="OptimizeVertexCache(ref uint, in uint, nuint, nuint)"/>.
    /// if index buffer contains multiple ranges for multiple draw calls, this function needs to be called on each range individually.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements).</param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <param name="cache_size">Should be less than the actual GPU cache size to avoid cache thrashing</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexCacheFifo")]
    public static partial void OptimizeVertexCacheFifo(ref uint destination, in uint indices, nuint index_count, nuint vertex_count, uint cache_size);

    /// <summary>
    /// Reorders indices to reduce the number of GPU vertex shader invocations and the pixel overdraw
    /// If index buffer contains multiple ranges for multiple draw calls, this function needs to be called on each range individually.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements).</param>
    /// <param name="indices">Must contain index data that is the result of <seealso cref="OptimizeVertexCache(ref uint, in uint, nuint, nuint)"/> (<c>not</c> the original mesh indices!)</param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="threshold">Indicates how much the overdraw optimizer can degrade vertex cache efficiency (1.05 = up to 5%) to reduce overdraw more efficiently</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeOverdraw")]
    public static partial void OptimizeOverdraw(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, float threshold);


    /// <summary>
    /// Vertex fetch cache optimizer.<para/>
    /// Reorders vertices and changes indices to reduce the amount of GPU memory fetches during vertex processing.
    /// Returns the number of unique vertices, which is the same as input vertex count unless some vertices are unused.
    /// This function works for a single vertex stream; for multiple vertex streams, <seealso cref="OptimizeVertexFetchRemap(ref uint, in uint, nuint, nuint)"/> + <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/> for each stream.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting remap table (<paramref name="vertex_count"/> elements).</param>
    /// <param name="indices">Used both as an input and as an output index buffer.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertices">Address for vertex data.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <param name="vertex_size">The size of each vertex data in bytes.</param>
    /// <returns>Number of unique vertices.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexFetch")]
    public static partial nuint OptimizeVertexFetch(void* destination, ref uint indices, nuint index_count, void* vertices, nuint vertex_count, nuint vertex_size);

    /// <summary>
    /// Vertex fetch cache optimizer.<para/>
    /// Generates vertex remap to reduce the amount of GPU memory fetches during vertex processing
    /// returns the number of unique vertices, which is the same as input vertex count unless some vertices are unused
    /// the resulting remap table should be used to reorder vertex/index buffers using <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/>/<seealso cref="RemapIndexBuffer(ref uint, in nint, nuint, in uint)"/>.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting remap table (<paramref name="vertex_count"/> elements)</param>
    /// <param name="indices">Address for index data.</param>
    /// <param name="index_count">The Number of indexes.</param>
    /// <param name="vertex_count">The number of vertices.</param>
    /// <returns>Number of unique vertices.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeVertexFetchRemap")]
    public static partial nuint OptimizeVertexFetchRemap(ref uint destination, in uint indices, nuint index_count, nuint vertex_count);
    #endregion

    #region Encodes | Decodes Vertex and Index
    /// <summary>
    /// Index buffer encoder.<para/>
    /// Encodes index data into an array of bytes that is generally much smaller (<c>&lt; 1.5 bytes/triangle</c>) and compresses better (<c>&lt; 1 bytes/triangle</c>) compared to the original.
    /// Input index buffer must represent a triangle list.
    /// </summary>
    /// <param name="buffer">Must contain enough space for the encoded index buffer (use <seealso cref="EncodeIndexBufferBound(nuint, nuint)"/> to compute worst case size).</param>
    /// <param name="buffer_size"></param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <returns>Returns encoded data size on success, 0 on error; the only error condition is if the buffer doesn't have enough space.
    /// For maximum efficiency, the index buffer being encoded has to be optimized for vertex cache and vertex fetch first.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeIndexBuffer")]
    public static partial nuint EncodeIndexBuffer(ref byte buffer, nuint buffer_size, in uint indices, nuint index_count);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeIndexBufferBound")]
    public static partial nuint EncodeIndexBufferBound(nuint index_count, nuint vertex_count);

    /// <summary>
    /// Set index encoder format version.
    /// </summary>
    /// <param name="version">Must specify the data format version to encode; valid values are 0 (decodable by all library versions) and 1 (decodable by 0.14+)</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeIndexVersion")]
    public static partial void EncodeIndexVersion(int version);

    /// <summary>
    /// Index buffer decoder.<para/>
    /// Decodes index data from an array of bytes generated by <seealso cref="EncodeIndexBuffer(ref byte, nuint, in uint, nuint)"/>.
    /// The decoder is safe to use for untrusted input, but it may produce garbage data (e.g., out of range indices).
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements)</param>
    /// <param name="index_count"></param>
    /// <param name="index_size"></param>
    /// <param name="buffer"></param>
    /// <param name="buffer_size"></param>
    /// <returns>Returns 0 if decoding was successful, and an error code otherwise.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_decodeIndexBuffer")]
    public static partial int DecodeIndexBuffer(void* destination, nuint index_count, nuint index_size, in byte buffer, nuint buffer_size);

    /// <summary>
    /// Index sequence encoder.<para/>
    /// Encodes index sequence into an array of bytes that is generally smaller and compresses better compared to the original.
    /// Input index sequence can represent arbitrary topology; for triangle lists, meshopt_encodeIndexBuffer is likely to be better.
    /// </summary>
    /// <param name="buffer">Must contain enough space for the encoded index sequence (use <seealso cref="EncodeIndexSequenceBound(nuint, nuint)"/> to compute worst case size)</param>
    /// <param name="buffer_size"></param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <returns>Returns encoded data size on success, 0 on error; the only error condition is if the buffer doesn't have enough space.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeIndexSequence")]
    public static partial nuint EncodeIndexSequence(ref byte buffer, nuint buffer_size, in uint indices, nuint index_count);

    /// <summary>
    /// </summary>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeIndexSequenceBound")]
    public static partial nuint EncodeIndexSequenceBound(nuint index_count, nuint vertex_count);

    /// <summary>
    /// Index sequence decoder.<para/>
    /// Decodes index data from an array of bytes generated by <seealso cref="EncodeIndexSequence(ref byte, nuint, in uint, nuint)"/>.
    /// The decoder is safe to use for untrusted input, but it may produce garbage data (e.g., out of range indices).
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements)</param>
    /// <param name="index_count"></param>
    /// <param name="index_size"></param>
    /// <param name="buffer"></param>
    /// <param name="buffer_size"></param>
    /// <returns>Returns 0 if decoding was successful, and an error code otherwise.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_decodeIndexSequence")]
    public static partial int DecodeIndexSequence(void* destination, nuint index_count, nuint index_size, in byte buffer, nuint buffer_size);

    /// <summary>
    /// Vertex buffer encoder.<para/>
    /// Encodes vertex data into an array of bytes that is generally smaller and compresses better compared to the original.
    /// Note that all vertex_size bytes of each vertex are encoded verbatim, including padding which should be zero-initialized.
    /// </summary>
    /// <param name="buffer">Must contain enough space for the encoded vertex buffer (use <seealso cref="EncodeVertexBufferBound(nuint, nuint)"/> to compute worst case size)</param>
    /// <param name="buffer_size"></param>
    /// <param name="vertices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_size"></param>
    /// <returns>Returns encoded data size on success, 0 on error; the only error condition is if the buffer doesn't have enough space.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeVertexBuffer")]
    public static partial nuint EncodeVertexBuffer(ref byte buffer, nuint buffer_size, void* vertices, nuint vertex_count, nuint vertex_size);

    /// <summary>
    /// This function works for a single vertex stream; for multiple vertex streams, call <see cref="EncodeVertexBuffer(ref byte, nuint, void*, nuint, nuint)"/> for each stream.
    /// </summary>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_size"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeVertexBufferBound")]
    public static partial nuint EncodeVertexBufferBound(nuint vertex_count, nuint vertex_size);

    /// <summary>
    /// Set vertex encoder format version.
    /// </summary>
    /// <param name="version">Must specify the data format version to encode; valid values are 0 (decodable by all library versions)</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeVertexVersion")]
    public static partial void EncodeVertexVersion(int version);

    /// <summary>
    /// Vertex buffer decoder.<para/>
    /// Decodes vertex data from an array of bytes generated by <seealso cref="EncodeVertexBuffer(ref byte, nuint, void*, nuint, nuint)"/>.
    /// The decoder is safe to use for untrusted input, but it may produce garbage data.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting vertex buffer (<paramref name="vertex_count"/> * <paramref name="vertex_size"/> bytes)</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_size"></param>
    /// <param name="buffer"></param>
    /// <param name="buffer_size"></param>
    /// <returns>Returns 0 if decoding was successful, and an error code otherwise.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_decodeVertexBuffer")]
    public static partial int DecodeVertexBuffer(void* destination, nuint vertex_count, nuint vertex_size, in byte buffer, nuint buffer_size);
    #endregion

    #region Decode | Encode Filter
    /// <summary>
    /// Can be used to filter the output of <seealso cref="DecodeVertexBuffer(void*, nuint, nuint, in byte, nuint)"/> in-place.<para/>
    /// Decodes octahedral encoding of a unit vector with K-bit (K &lt;= 16) signed X/Y as an input; Z must store 1.0f.
    /// Each component is stored as an 8-bit or 16-bit normalized integer
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="count"></param>
    /// <param name="stride">Must be equal to 4 or 8. W is preserved as is.</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_decodeFilterOct")]
    public static partial void DecodeFilterOct(void* buffer, nuint count, nuint stride);

    /// <summary>
    /// Can be used to filter the output of <seealso cref="DecodeVertexBuffer(void*, nuint, nuint, in byte, nuint)"/> in-place.<para/>
    /// Decodes 3-component quaternion encoding with K-bit (4 &lt;= K &lt;= 16) component encoding and a 2-bit component index indicating which component to reconstruct.
    /// Each component is stored as an 16-bit integer.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="count"></param>
    /// <param name="stride">Must be equal to 8.</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_decodeFilterQuat")]
    public static partial void DecodeFilterQuat(void* buffer, nuint count, nuint stride);

    /// <summary>
    /// Can be used to filter the output of <seealso cref="DecodeVertexBuffer(void*, nuint, nuint, in byte, nuint)"/> in-place.<para/>
    /// Decodes exponential encoding of floating-point data with 8-bit exponent and 24-bit integer mantissa as 2^E*M.
    /// Each 32-bit component is decoded in isolation
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="count"></param>
    /// <param name="stride">Must be divisible by 4.</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_decodeFilterExp")]
    public static partial void DecodeFilterExp(void* buffer, nuint count, nuint stride);


    /// <summary>
    /// Can be used to encode data in a format that <seealso cref="DecodeFilterOct(void*, nuint, nuint)"/> meshopt_decodeFilter can decode.<para/>
    /// Encodes unit vectors with K-bit (K &lt;= 16) signed X/Y as output.
    /// Each component is stored as an 8-bit or 16-bit normalized integer.
    /// Input data must contain 4 floats for every vector (count * 4 total).
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="count"></param>
    /// <param name="stride">Stride must be equal to 4 or 8. W is preserved as is.</param>
    /// <param name="bits"></param>
    /// <param name="data">Must be divisible by 4.</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeFilterOct")]
    public static partial void EncodeFilterOct(void* destination, nuint count, nuint stride, int bits, in float data);

    /// <summary>
    /// Can be used to encode data in a format that <seealso cref="DecodeFilterQuat(void*, nuint, nuint)"/> meshopt_decodeFilter can decode.<para/>
    /// Encodes unit quaternions with K-bit (4 &lt;= K &lt;= 16) component encoding.
    /// Each component is stored as a 16-bit integer.
    /// Input data must contain 4 floats for every quaternion (count * 4 total).
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="count"></param>
    /// <param name="stride">Must be equal to 8.</param>
    /// <param name="bits"></param>
    /// <param name="data"></param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeFilterQuat")]
    public static partial void EncodeFilterQuat(void* destination, nuint count, nuint stride, int bits, in float data);

    /// <summary>
    /// Can be used to encode data in a format that <seealso cref="DecodeFilterExp(void*, nuint, nuint)"/> meshopt_decodeFilter can decode.<para/>
    /// Encodes arbitrary (finite) floating-point data with an 8-bit exponent and K-bit integer mantissa (1 &lt;= K &lt;= 24).
    /// Exponent can be shared between all components of a given vector as defined by stride or all values of a given component.
    /// Input data must contain stride/4 floats for every vector (count * stride / 4 total).
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="count"></param>
    /// <param name="stride">Must be divisible by 4.</param>
    /// <param name="bits"></param>
    /// <param name="data"></param>
    /// <param name="mode"></param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_encodeFilterExp")]
    public static partial void EncodeFilterExp(void* destination, nuint count, nuint stride, int bits, in float data, MeshoptEncodeExpMode mode);
    #endregion

    #region Simplify
    /// <summary>
    /// Mesh simplifier.<para/>
    /// Reduces the number of triangles in the mesh, attempting to preserve mesh appearance as much as possible.
    /// The algorithm tries to preserve mesh topology and can stop short of the target goal based on topology constraints or target error.
    /// If not all attributes from the input mesh are required, it's recommended to reindex the mesh using <seealso cref="GenerateShadowIndexBuffer(ref uint, in uint, nuint, void*, nuint, nuint, nuint)"/> prior to simplification.
    /// The resulting index buffer references vertices from the original vertex buffer.
    /// If the original vertex data isn't required, creating a compact vertex buffer using meshopt_optimizeVertexFetch is recommended.
    /// </summary>
    /// <param name="destination">Must contain enough space for the target index buffer, worst case is <paramref name="index_count"/> elements (<c>not</c> <paramref name="target_index_count"/>)!</param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="target_index_count"></param>
    /// <param name="target_error">Represents the error relative to mesh extents that can be tolerated, e.g. 0.01 = 1% deformation; value range [0..1].</param>
    /// <param name="options">Must be a bitmask composed of meshopt_SimplifyX options; 0 is a safe default</param>
    /// <param name="result_error">Can be <see cref="IntPtr.Zero"/>, when it's not <see cref="IntPtr.Zero"/>, it will contain the resulting (relative) error after simplification.</param>
    /// <returns>Returns the number of indices after simplification, with destination containing new index data.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplify")]
    public static partial nuint Simplify(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, nuint target_index_count, float target_error, uint options, out float result_error);

    /// <inheritdoc cref="Simplify(ref uint, in uint, nuint, in float, nuint, nuint, nuint, float, uint, out float)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplify")]
    public static partial nuint Simplify(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, nuint target_index_count, float target_error, uint options, IntPtr result_error);

    /// <summary>
    /// Experimental: Mesh simplifier with attribute metric.<para/>
    /// The algorithm ehnahces meshopt_simplify by incorporating attribute values into the error metric used to prioritize simplification order; <see cref="Simplify(ref uint, in uint, nuint, in float, nuint, nuint, nuint, float, uint, out float)"/>  documentation for details.
    /// Note that the number of attributes affects memory requirements and running time; this algorithm requires ~1.5x more memory and time compared to meshopt_simplify when using 4 scalar attributes.
    /// </summary>
    /// <remarks>
    /// TODO <paramref name="target_error"/> and <paramref name="result_error"/> currently use combined distance+attribute error;
    /// This may change in the future
    /// </remarks>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="vertex_attributes">Should have attribute_count floats for each vertex</param>
    /// <param name="vertex_attributes_stride"></param>
    /// <param name="attribute_weights">Should have attribute_count floats in total; the weights determine relative priority of attributes between each other and wrt position. The recommended weight range is [1e-3..1e-1], assuming attribute data is in [0..1] range.</param>
    /// <param name="attribute_count">Attribute_count must be &lt;= 16</param>
    /// <param name="vertex_lock">Can be <see cref="IntPtr.Zero"/>; when it's not <see cref="IntPtr.Zero"/>, it should have a value for each vertex; 1 denotes vertices that can't be moved</param>
    /// <param name="target_index_count"></param>
    /// <param name="target_error"></param>
    /// <param name="options"></param>
    /// <param name="result_error">Can be <see cref="IntPtr.Zero"/>; when it's not <see cref="IntPtr.Zero"/>, it will contain the resulting (relative) error after simplification.</param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifyWithAttributes")]
    public static partial nuint SimplifyWithAttributes(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, in float vertex_attributes, nuint vertex_attributes_stride, in float attribute_weights, nuint attribute_count, in byte vertex_lock, nuint target_index_count, float target_error, uint options, out float result_error);

    /// <inheritdoc cref="SimplifyWithAttributes(ref uint, in uint, nuint, in float, nuint, nuint, in float, nuint, in float, nuint, in byte, nuint, float, uint, out float)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifyWithAttributes")]
    public static partial nuint SimplifyWithAttributes(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, in float vertex_attributes, nuint vertex_attributes_stride, in float attribute_weights, nuint attribute_count, in IntPtr vertex_lock, nuint target_index_count, float target_error, uint options, IntPtr result_error);

    /// <inheritdoc cref="SimplifyWithAttributes(ref uint, in uint, nuint, in float, nuint, nuint, in float, nuint, in float, nuint, in byte, nuint, float, uint, out float)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifyWithAttributes")]
    public static partial nuint SimplifyWithAttributes(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, in float vertex_attributes, nuint vertex_attributes_stride, in float attribute_weights, nuint attribute_count, in byte vertex_lock, nuint target_index_count, float target_error, uint options, IntPtr result_error);

    /// <inheritdoc cref="SimplifyWithAttributes(ref uint, in uint, nuint, in float, nuint, nuint, in float, nuint, in float, nuint, in byte, nuint, float, uint, out float)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifyWithAttributes")]
    public static partial nuint SimplifyWithAttributes(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, in float vertex_attributes, nuint vertex_attributes_stride, in float attribute_weights, nuint attribute_count, in IntPtr vertex_lock, nuint target_index_count, float target_error, uint options, out float result_error);

    /// <summary>
    /// Experimental: Mesh simplifier (sloppy).<para/>
    /// Reduces the number of triangles in the mesh, sacrificing mesh appearance for simplification performance.
    /// The algorithm doesn't preserve mesh topology but can stop short of the target goal based on target error.
    /// The resulting index buffer references vertices from the original vertex buffer.
    /// If the original vertex data isn't required, creating a compact vertex buffer using <seealso cref="OptimizeVertexFetch(void*, ref uint, nuint, void*, nuint, nuint)"/> is recommended.
    /// </summary>
    /// <param name="destination">Must contain enough space for the target index buffer, worst case is <paramref name="index_count"/> elements (<c>NOT</c> <paramref name="target_index_count"/> )!</param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="target_index_count"></param>
    /// <param name="target_error">Represents the error relative to mesh extents that can be tolerated, e.g. 0.01 = 1% deformation; value range [0..1].</param>
    /// <param name="result_error">Can be <see cref="IntPtr.Zero"/>; when it's not <see cref="IntPtr.Zero"/>, it will contain the resulting (relative) error after simplification.</param>
    /// <returns>Returns the number of indices after simplification, with destination containing new index data.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifySloppy")]
    public static partial nuint SimplifySloppy(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, nuint target_index_count, float target_error, out float result_error);

    /// <inheritdoc cref="SimplifySloppy(ref uint, in uint, nuint, in float, nuint, nuint, nuint, float, nint)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifySloppy")]
    public static partial nuint SimplifySloppy(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, nuint target_index_count, float target_error, IntPtr result_error);


    /// <summary>
    /// Experimental: Point cloud simplifier.<para/>
    /// Reduces the number of points in the cloud to reach the given target.
    /// The resulting index buffer references vertices from the original vertex buffer.
    /// If the original vertex data isn't required, creating a compact vertex buffer using <seealso cref="OptimizeVertexFetch(void*, ref uint, nuint, void*, nuint, nuint)"/> is recommended.
    /// </summary>
    /// <param name="destination">Must contain enough space for the target index buffer (<paramref name="target_vertex_count"/> elements)</param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="vertex_colors">Should can be <see cref="IntPtr.Zero"/>, when it's not <see cref="IntPtr.Zero"/>, it should have float3 color in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_colors_stride"></param>
    /// <param name="color_weight"></param>
    /// <param name="target_vertex_count"></param>
    /// <returns>Returns the number of points after simplification, with destination containing new index data.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifyPoints")]
    public static partial nuint SimplifyPoints(ref uint destination, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, in float vertex_colors, nuint vertex_colors_stride, float color_weight, nuint target_vertex_count);

    /// <inheritdoc cref="SimplifyPoints(ref uint, in float, nuint, nuint, in float, nuint, float, nuint)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifyPoints")]
    public static partial nuint SimplifyPoints(ref uint destination, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, in IntPtr vertex_colors, nuint vertex_colors_stride, float color_weight, nuint target_vertex_count);

    /// <summary>
    /// Absolute error must be <c>divided</c> by the scaling factor before passing it to <seealso cref="Simplify(ref uint, in uint, nuint, in float, nuint, nuint, nuint, float, uint, out float)"/> as <c>target_error</c>.
    /// Relative error returned by <seealso cref="Simplify(ref uint, in uint, nuint, in float, nuint, nuint, nuint, float, uint, out float)"/> via result_error must be <c>multiplied</c> by the scaling factor to get absolute error.
    /// </summary>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <returns>Returns the error scaling factor used by the simplifier to convert between absolute and relative extents.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_simplifyScale")]
    public static partial float SimplifyScale(in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);
    #endregion

    #region Stripify
    /// <summary>
    /// Mesh stripifier.<para/>
    /// Converts a previously vertex cache optimized triangle list to triangle strip, stitching strips using restart index or degenerate triangles.
    /// For maximum efficiency the index buffer being converted has to be optimized for vertex cache first.
    /// Using restart indices can result in ~10% smaller index buffers, but on some GPUs restart indices may result in decreased performance.
    /// </summary>
    /// <param name="destination">Must contain enough space for the target index buffer, worst case can be computed with <seealso cref="StripifyBound(nuint)"/></param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    /// <param name="restart_index">Should be 0xffff or 0xffffffff depending on index size, or 0 to use degenerate triangles</param>
    /// <returns>Returns the number of indices in the resulting strip, with destination containing new index data.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_stripify")]
    public static partial nuint Stripify(ref uint destination, in uint indices, nuint index_count, nuint vertex_count, uint restart_index);

    /// <summary>
    /// </summary>
    /// <param name="index_count"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_stripifyBound")]
    public static partial nuint StripifyBound(nuint index_count);

    /// <summary>
    /// Mesh unstripifier.<para/>
    /// Converts a triangle strip to a triangle list.
    /// </summary>
    /// <param name="destination">Must contain enough space for the target index buffer, worst case can be computed with <seealso cref="UnstripifyBound(nuint)"/>.</param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="restart_index"></param>
    /// <returns>Returns the number of indices in the resulting list, with destination containing new index data.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_unstripify")]
    public static partial nuint Unstripify(ref uint destination, in uint indices, nuint index_count, uint restart_index);

    /// <summary>
    /// </summary>
    /// <param name="index_count"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_unstripifyBound")]
    public static partial nuint UnstripifyBound(nuint index_count);
    #endregion

    #region Analize
    /// <summary>
    /// Vertex transform cache analyzer.<para/>
    /// Results may not match actual GPU performance.
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    /// <param name="cache_size"></param>
    /// <param name="warp_size"></param>
    /// <param name="primgroup_size"></param>
    /// <returns>Returns cache hit statistics using a simplified FIFO model.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_analyzeVertexCache")]
    public static partial MeshoptVertexCacheStatistics AnalyzeVertexCache(in uint indices, nuint index_count, nuint vertex_count, uint cache_size, uint warp_size, uint primgroup_size);

    /// <summary>
    /// Overdraw analyzer.<para/>
    /// Results may not match actual GPU performance.
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <returns>Returns overdraw statistics using a software rasterizer.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_analyzeOverdraw")]
    public static partial MeshoptOverdrawStatistics AnalyzeOverdraw(in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);

    /// <summary>
    /// Vertex fetch cache analyzer.<para/>
    /// Results may not match actual GPU performance.
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_size"></param>
    /// <returns>Returns cache hit statistics using a simplified direct mapped model.</returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_analyzeVertexFetch")]
    public static partial MeshoptVertexFetchStatistics AnalyzeVertexFetch(in uint indices, nuint index_count, nuint vertex_count, nuint vertex_size);

    #endregion

    #region Meshlets
    /// <summary>
    /// Meshlet builder.<para/>
    /// Splits the mesh into a set of meshlets where each meshlet has a micro index buffer indexing into meshlet vertices that refer to the original vertex buffer.
    /// The resulting data can be used to render meshes using NVidia programmable mesh shading pipeline, or in other cluster-based renderers.
    /// When using buildMeshlets, vertex positions need to be provided to minimize the size of the resulting clusters.
    /// When using buildMeshletsScan, for maximum efficiency the index buffer being converted has to be optimized for vertex cache first.
    /// </summary>
    /// <param name="meshlets">Must contain enough space for all meshlets, worst case size can be computed with <seealso cref="BuildMeshletsBound(nuint, nuint, nuint)"/></param>
    /// <param name="meshlet_vertices">Must contain enough space for all meshlets, worst case size is equal to max_meshlets * max_vertices</param>
    /// <param name="meshlet_triangles">Must contain enough space for all meshlets, worst case size is equal to max_meshlets * max_triangles * 3</param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="max_vertices">Must not exceed implementation limits of 255</param>
    /// <param name="max_triangles">Must not exceed implementation limits of 512</param>
    /// <param name="cone_weight">Should be set to 0 when cone culling is not used, and a value between 0 and 1 otherwise to balance between cluster size and cone culling efficiency</param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_buildMeshlets")]
    public static partial nuint BuildMeshlets(ref MeshoptMeshlet meshlets, ref uint meshlet_vertices, ref byte meshlet_triangles, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride, nuint max_vertices, nuint max_triangles, float cone_weight);

    /// <inheritdoc cref="BuildMeshlets(ref MeshoptMeshlet, ref uint, ref byte, in uint, nuint, in float, nuint, nuint, nuint, nuint, float)"/>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_buildMeshletsScan")]
    public static partial nuint BuildMeshletsScan(ref MeshoptMeshlet meshlets, ref uint meshlet_vertices, ref byte meshlet_triangles, in uint indices, nuint index_count, nuint vertex_count, nuint max_vertices, nuint max_triangles);

    /// <inheritdoc cref="BuildMeshlets(ref MeshoptMeshlet, ref uint, ref byte, in uint, nuint, in float, nuint, nuint, nuint, nuint, float)"/>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_buildMeshletsBound")]
    public static partial nuint BuildMeshletsBound(nuint index_count, nuint max_vertices, nuint max_triangles);

    /// <summary>
    /// Experimental: Meshlet optimizer<para/>
    /// Reorders meshlet vertices and triangles to maximize locality to improve rasterizer throughput
    /// </summary>
    /// <param name="meshlet_vertices">must refer to meshlet index data; when buildMeshlets* is used, needs to be computed from meshlet's vertex_offset</param>
    /// <param name="meshlet_triangles">must refer to meshlet triangle data; when buildMeshlets* is used, needs to be computed from meshlet's triangle_offset</param>
    /// <param name="triangle_count">must not exceed implementation limits (vertex_count &lt;= 255 - not 256!, triangle_count &lt;= 512)</param>
    /// <param name="vertex_count">must not exceed implementation limits (vertex_count &lt;= 255 - not 256!, triangle_count &lt;= 512)</param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_optimizeMeshlet")]
    public static partial void OptimizeMeshlet(ref uint meshlet_vertices, ref byte meshlet_triangles, nuint triangle_count, nuint vertex_count);
    #endregion

    #region Computes
    /// <summary>
    /// Cluster bounds generator<para/>
    /// Creates bounding volumes that can be used for frustum, backface, and occlusion culling.<para/>
    /// 
    /// For backface culling with orthographic projection, use the following formula to reject backfacing clusters:
    /// <c>dot(view, cone_axis) &gt;= cone_cutoff</c>.<para/>
    /// 
    /// For perspective projection, you can use the formula that needs cone apex in addition to axis &amp; cutoff:
    /// <c>dot(normalize(cone_apex - camera_position), cone_axis) &gt;= cone_cutoff</c>.<para/>
    /// 
    /// Alternatively, you can use the formula that doesn't need cone apex and uses a bounding sphere instead:
    /// <c/>dot(normalize(center - camera_position), cone_axis) &gt;= cone_cutoff + radius / length(center - camera_position)<c/>
    /// or an equivalent formula that doesn't have a singularity at center = camera_position:
    /// <c>dot(center - camera_position, cone_axis) &gt;= cone_cutoff * length(center - camera_position) + radius</c> <para/>
    /// 
    /// The formula that uses the apex is slightly more accurate but needs the apex; if you are already using a bounding sphere
    /// to do frustum/occlusion culling, the formula that doesn't use the apex may be preferable (for derivation, see
    /// Real-Time Rendering 4th Edition, section 19.3).
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_computeClusterBounds")]
    public static partial MeshoptBounds ComputeClusterBounds(in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);

    /// <inheritdoc cref="ComputeClusterBounds(in uint, nuint, in float, nuint, nuint)"/>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_computeMeshletBounds")]
    public static partial MeshoptBounds ComputeMeshletBounds(in uint meshlet_vertices, in byte meshlet_triangles, nuint triangle_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);
    #endregion

    #region Spatial

    /// <summary>
    /// Spatial sorter.<para/>
    /// Generates a remap table that can be used to reorder points for spatial locality.
    /// The resulting remap table maps old vertices to new vertices and can be used in <seealso cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/>.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting remap table (<paramref name="vertex_count"/> elements).</param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_spatialSortRemap")]
    public static partial void SpatialSortRemap(ref uint destination, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);

    /// <summary>
    /// Experimental: Spatial sorter.<para/>
    /// Reorders triangles for spatial locality, and generates a new index buffer. The resulting index buffer can be used with other functions like <seealso cref="OptimizeVertexCache(ref uint, in uint, nuint, nuint)"/>.
    /// </summary>
    /// <param name="destination">Must contain enough space for the resulting index buffer (<paramref name="index_count"/> elements).</param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_positions">Should have float3 position in the first 12 bytes of each vertex.</param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_spatialSortTriangles")]
    public static partial void SpatialSortTriangles(ref uint destination, in uint indices, nuint index_count, in float vertex_positions, nuint vertex_count, nuint vertex_positions_stride);
    #endregion

    #region Quantize
    /// <summary>
    /// Quantize a float into half-precision (as defined by IEEE-754 fp16) floating point value.
    /// Generates +-inf for overflow, preserves NaN, flushes denormals to zero, rounds to nearest.
    /// Representable magnitude range: [6e-5; 65504].
    /// Maximum relative reconstruction error: 5e-4.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_quantizeHalf")]
    public static partial ushort QuantizeHalf(float v);

    /// <summary>
    /// Quantize a float into a floating point value with a limited number of significant mantissa bits, preserving the IEEE-754 fp32 binary representation.
    /// Generates +-inf for overflow, preserves NaN, flushes denormals to zero, rounds to nearest.
    /// Assumes N is in a valid mantissa precision range, which is 1..23.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="N"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_quantizeFloat")]
    public static partial float QuantizeFloat(float v, int N);


    /// <summary>
    /// Reverse quantization of a half-precision (as defined by IEEE-754 fp16) floating point value.
    /// Preserves Inf/NaN, flushes denormals to zero.
    /// </summary>
    /// <param name="h"></param>
    /// <returns></returns>
    [LibraryImport(LIBRARY_NAME, EntryPoint = "meshopt_dequantizeHalf")]
    public static partial float DequantizeHalf(ushort h);

    /// <summary>
    /// Quantize a float in [0..1] range into an N-bit fixed point unorm value.
    /// Assumes reconstruction function (q / (2^N-1)), which is the case for fixed-function normalized fixed point conversion.
    /// Maximum reconstruction error: 1/2^(N+1).
    /// </summary>
    /// <param name="v"></param>
    /// <param name="N"></param>
    /// <returns></returns>
    public static int QuantizeUnorm(float v, int N)
    {
        float scale = (1 << N) - 1;

        v = (v >= 0) ? v : 0;
        v = (v <= 1) ? v : 1;

        return (int)(v * scale + 0.5f);
    }

    /// <summary>
    /// Quantize a float in [-1..1] range into an N-bit fixed point snorm value.
    /// Assumes reconstruction function (q / (2^(N-1)-1)), which is the case for fixed-function normalized fixed point conversion (except early OpenGL versions).
    /// Maximum reconstruction error: 1/2^N.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="N"></param>
    /// <returns></returns>
    public static int QuantizeSnorm(float v, int N)
    {
        float scale = (1 << (N - 1)) - 1;

        float round = (v >= 0 ? 0.5f : -0.5f);

        v = (v >= -1) ? v : -1;
        v = (v <= +1) ? v : +1;

        return (int)(v * scale + round);
    }
    #endregion
}
