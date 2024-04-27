namespace Meshoptimizer;

public static unsafe partial class Meshopt
{
    #region Generate

    /// <summary>
    /// <inheritdoc cref="GenerateVertexRemap(ref uint, in uint, nuint, void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="remap"></param>
    /// <param name="indices"></param>
    /// <param name="vertices"></param>
    /// <returns><inheritdoc cref="GenerateVertexRemap(ref uint, in nint, nuint, void*, nuint, nuint)"/></returns>
    public static int GenerateVertexRemap<TVertex>(out Span<uint> remap, ReadOnlySpan<uint> indices, ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        remap = new uint[indices.Length];
        return GenerateVertexRemap(remap, indices, vertices);
    }

    /// <summary>
    /// <inheritdoc cref="GenerateVertexRemap(ref uint, in nint, nuint, void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertices"></param>
    /// <returns><inheritdoc cref="GenerateVertexRemap(ref uint, in nint, nuint, void*, nuint, nuint)"/></returns>
    public static int GenerateVertexRemap<TVertex>(Span<uint> destination, ReadOnlySpan<uint> indices, ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        fixed (void* ptr = vertices)
        {
            return (int)GenerateVertexRemap(ref destination[0], indices[0], (nuint)indices.Length, ptr, (nuint)vertices.Length, (nuint)sizeof(TVertex));
        }
    }

    /// <summary>
    /// <inheritdoc cref="GenerateVertexRemap(ref uint, in uint, nuint, void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="remappedResult"></param>
    /// <param name="vertices"></param>
    /// <returns><inheritdoc cref="GenerateVertexRemap(ref uint, in uint, nuint, void*, nuint, nuint)"/></returns>
    public static int GenerateVertexRemap<TVertex>(out Span<uint> remappedResult, ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        remappedResult = new uint[vertices.Length * 3];

        fixed (void* ptr = vertices)
        {
            return (int)GenerateVertexRemap(ref remappedResult[0], IntPtr.Zero, (nuint)vertices.Length * 3, ptr, (nuint)vertices.Length, (nuint)sizeof(TVertex));
        }
    }

    /// <summary>
    /// <inheritdoc cref="GenerateVertexRemap(ref uint, in uint, nuint, void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="destination"></param>
    /// <param name="vertices"></param>
    /// <returns><inheritdoc cref="GenerateVertexRemap(ref uint, in uint, nuint, void*, nuint, nuint)"/></returns>
    public static int GenerateVertexRemap<TVertex>(Span<uint> destination, ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        destination = new uint[vertices.Length * 3];

        fixed (void* ptr = vertices)
        {
            return (int)GenerateVertexRemap(ref destination[0], IntPtr.Zero, (nuint)vertices.Length * 3, ptr, (nuint)vertices.Length, (nuint)sizeof(TVertex));
        }
    }

    /// <summary>
    /// <inheritdoc cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="streams"></param>
    /// <param name="stream_count"></param>
    /// <returns><inheritdoc cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/></returns>
    public static int GenerateVertexRemapMulti(Span<uint> destination, ReadOnlySpan<uint> indices, int vertex_count, MeshoptStream streams, int stream_count)
    {
        return (int)GenerateVertexRemapMulti(ref destination[0], indices[0], (nuint)indices.Length, (nuint)vertex_count, streams, (nuint)stream_count);
    }

    /// <summary>
    /// <inheritdoc cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices_count"></param>
    /// <param name="vertex_count"></param>
    /// <param name="streams"></param>
    /// <param name="stream_count"></param>
    /// <returns><inheritdoc cref="GenerateVertexRemapMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/></returns>
    public static int GenerateVertexRemapMulti(Span<uint> destination, int indices_count, int vertex_count, MeshoptStream streams, int stream_count)
    {
        return (int)GenerateVertexRemapMulti(ref destination[0], IntPtr.Zero, (nuint)indices_count, (nuint)vertex_count, streams, (nuint)stream_count);
    }

    /// <summary>
    /// <inheritdoc cref="GenerateShadowIndexBuffer(ref uint, in uint, nuint, void*, nuint, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertices"></param>
    /// <param name="vertex_stride"></param>
    public static void GenerateShadowIndexBuffer<TVertex>(Span<uint> destination, ReadOnlySpan<uint> indices, ReadOnlySpan<TVertex> vertices, int vertex_stride)
        where TVertex : unmanaged
    {
        fixed(void* ptr = vertices)
        {
            GenerateShadowIndexBuffer(ref destination[0], indices[0], (nuint)indices.Length, ptr, (nuint)vertices.Length, (nuint)sizeof(TVertex), (nuint)vertex_stride);
        }
    }

    /// <summary>
    /// <inheritdoc cref="GenerateShadowIndexBufferMulti(ref uint, in uint, nuint, nuint, in MeshoptStream, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="streams"></param>
    /// <param name="stream_count"></param>
    public static void GenerateShadowIndexBufferMulti<TVertex>(Span<uint> destination, ReadOnlySpan<uint> indices, int vertex_count, in MeshoptStream streams, int stream_count)
        where TVertex : unmanaged
    {
        GenerateShadowIndexBufferMulti(ref destination[0], indices[0], (nuint)indices.Length, (nuint)vertex_count, streams, (nuint)stream_count);
    }

    /// <summary>
    /// <inheritdoc cref="GenerateAdjacencyIndexBuffer(ref uint, in uint, nuint, in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    public static void GenerateAdjacencyIndexBuffer(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride)
    {
        GenerateAdjacencyIndexBuffer(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride);
    }

    /// <summary>
    /// <inheritdoc cref="GenerateTessellationIndexBuffer(ref uint, in uint, nuint, in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    public static void GenerateTessellationIndexBuffer(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride)
    {
        GenerateTessellationIndexBuffer(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride);
    }
    #endregion

    #region Remap
    /// <summary>
    /// <inheritdoc cref="RemapVertexBuffer(void*, void*, nuint, nuint, in uint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="destination"></param>
    /// <param name="vertices"></param>
    /// <param name="remap"></param>
    public static void RemapVertexBuffer<TVertex>(Span<TVertex> destination, ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> remap)
        where TVertex : unmanaged
    {
        fixed (void* ptrDst = destination)
        fixed (void* ptrVert = vertices)
        {
            RemapVertexBuffer(ptrDst, ptrVert, (nuint)vertices.Length, (nuint)sizeof(TVertex), remap[0]);
        }
    }

    /// <inheritdoc cref="RemapIndexBuffer(ref uint, in uint, nuint, in uint)"/>
    public static void RemapIndexBuffer(Span<uint> destination, ReadOnlySpan<uint> indices, ReadOnlySpan<uint> remap)
    {
        RemapIndexBuffer(ref destination[0], indices[0], (nuint)indices.Length, remap[0]);
    }

    /// <inheritdoc cref="RemapIndexBuffer(ref uint, in uint, nuint, in uint)"/>
    public static void RemapIndexBuffer(Span<uint> destination, int index_count, ReadOnlySpan<uint> remap)
    {
        RemapIndexBuffer(ref destination[0], IntPtr.Zero, (nuint)index_count, remap[0]);
    }
    #endregion

    #region Optimize
    /// <summary>
    /// <inheritdoc cref="OptimizeVertexCache(ref uint, in uint, nuint, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    public static void OptimizeVertexCache(Span<uint> destination, ReadOnlySpan<uint> indices, int vertex_count)
    {
        OptimizeVertexCache(ref destination[0], indices[0], (nuint)indices.Length, (nuint)vertex_count);
    }

    /// <summary>
    /// <inheritdoc cref="OptimizeVertexCacheStrip(ref uint, in uint, nuint, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    public static void OptimizeVertexCacheStrip(Span<uint> destination, ReadOnlySpan<uint> indices, int vertex_count)
    {
        OptimizeVertexCacheStrip(ref destination[0], indices[0], (uint)indices.Length, (nuint)vertex_count);
    }

    /// <summary>
    /// <inheritdoc cref="OptimizeVertexCacheFifo(ref uint, in uint, nuint, nuint, uint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="cache_size"></param>
    public static void OptimizeVertexCacheFifo(Span<uint> destination, ReadOnlySpan<uint> indices, int vertex_count, int cache_size)
    {
        OptimizeVertexCacheFifo(ref destination[0], indices[0], (nuint)indices.Length, (nuint)vertex_count, (uint)cache_size);
    }

    /// <summary>
    /// <inheritdoc cref="OptimizeOverdraw(ref uint, in uint, nuint, in float, nuint, nuint, float)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="threshold"></param>
    public static void OptimizeOverdraw(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, float threshold)
    {
        OptimizeOverdraw(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, threshold);
    }

    /// <summary>
    /// <inheritdoc cref="OptimizeVertexFetch(void*, ref uint, nuint, void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static int OptimizeVertexFetch<TVertex>(Span<TVertex> destination, Span<uint> indices, ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        fixed(void* ptrDst = destination)
        fixed(void* ptrVert = vertices)
        {
            return (int)OptimizeVertexFetch(ptrDst, ref indices[0], (nuint)indices.Length, ptrVert, (nuint)vertices.Length, (nuint)sizeof(TVertex));
        }
    }

    /// <summary>
    /// <inheritdoc cref="OptimizeVertexFetchRemap(ref uint, in uint, nuint, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <returns></returns>
    public static int OptimizeVertexFetchRemap(Span<uint> destination, ReadOnlySpan<uint> indices, int vertex_count)
    {
        return (int)OptimizeVertexFetchRemap(ref destination[0], indices[0], (nuint)indices.Length, (nuint)vertex_count);
    }
    #endregion

    #region Encodes | Decodes Vertex and Index

    /// <summary>
    /// <inheritdoc cref="EncodeIndexBuffer(ref byte, nuint, in uint, nuint)"/>
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="indices"></param>
    /// <returns></returns>
    public static int EncodeIndexBuffer(Span<byte> buffer, ReadOnlySpan<uint> indices)
    {
        return (int)EncodeIndexBuffer(ref buffer[0], (nuint)buffer.Length, indices[0], (nuint)indices.Length);
    }

    /// <summary>
    /// <inheritdoc cref="EncodeIndexBufferBound(nuint, nuint)"/>
    /// </summary>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    /// <returns></returns>
    public static int EncodeIndexBufferBound(int index_count, int vertex_count)
    {
        return (int)EncodeIndexBufferBound((nuint)index_count, (nuint)vertex_count);
    }

    /// <summary>
    /// <inheritdoc cref="DecodeIndexBuffer(void*, nuint, nuint, in byte, nuint)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="destination"></param>
    /// <param name="index_count"></param>
    /// <param name="index_size"></param>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static int DecodeIndexBuffer<T>(Span<T> destination, int index_count, int index_size, ReadOnlySpan<byte> buffer)
        where T : unmanaged
    {
        fixed(void* ptr = destination)
        {
            return DecodeIndexBuffer(ptr, (nuint)index_count, (nuint)index_size, buffer[0], (nuint)buffer.Length);
        }
    }

    /// <summary>
    /// <inheritdoc cref="EncodeIndexSequence(ref byte, nuint, in uint, nuint)"/>
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="indices"></param>
    /// <returns></returns>
    public static int EncodeIndexSequence(Span<byte> buffer, ReadOnlySpan<uint> indices)
    {
        return (int)EncodeIndexSequence(ref buffer[0], (nuint)buffer.Length, indices[0], (nuint)indices.Length);
    }

    /// <summary>
    /// <inheritdoc cref="EncodeIndexSequenceBound(nuint, nuint)"/>
    /// </summary>
    /// <param name="index_count"></param>
    /// <param name="vertex_count"></param>
    /// <returns></returns>
    public static int EncodeIndexSequenceBound(int index_count, int vertex_count)
    {
        return (int)EncodeIndexSequenceBound((nuint)index_count, (nuint)vertex_count);
    }

    /// <summary>
    /// <inheritdoc cref="DecodeIndexSequence(void*, nuint, nuint, in byte, nuint)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="destination"></param>
    /// <param name="index_count"></param>
    /// <param name="index_size"></param>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static int DecodeIndexSequence<T>(Span<T> destination, int index_count, int index_size, ReadOnlySpan<byte> buffer)
        where T : unmanaged
    {
        fixed(void* ptr = destination)
        {
            return DecodeIndexSequence(ptr, (nuint)index_count, (nuint)index_size, buffer[0], (nuint)buffer.Length);
        }
    }

    /// <summary>
    /// <inheritdoc cref="EncodeVertexBuffer(ref byte, nuint, void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="buffer"></param>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static int EncodeVertexBuffer<TVertex>(Span<byte> buffer, ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        fixed(void* ptr = vertices)
        {
            return (int)EncodeVertexBuffer(ref buffer[0], (nuint)buffer.Length, ptr, (nuint)vertices.Length, (nuint)sizeof(TVertex));
        }
    }

    /// <summary>
    /// <inheritdoc cref="EncodeVertexBufferBound(nuint, nuint)"/>
    /// </summary>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_size"></param>
    /// <returns></returns>
    public static int EncodeVertexBufferBound(int vertex_count, int vertex_size)
    {
        return (int)EncodeVertexBufferBound((nuint)vertex_count, (nuint)vertex_size);
    }

    /// <summary>
    /// <inheritdoc cref="DecodeVertexBuffer(void*, nuint, nuint, in byte, nuint)"/>
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <param name="destination"></param>
    /// <param name="vertex_count"></param>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static int DecodeVertexBuffer<TVertex>(Span<TVertex> destination, int vertex_count, ReadOnlySpan<byte> buffer)
        where TVertex : unmanaged
    {
        fixed(void* ptr = destination)
        {
            return DecodeVertexBuffer(ptr, (nuint)vertex_count, (nuint)sizeof(TVertex), buffer[0], (nuint)buffer.Length);
        }
    }
    #endregion

    #region Decode | Encode Filter

    /// <summary>
    /// <inheritdoc cref="DecodeFilterOct(void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="buffer"></param>
    /// <param name="stride"></param>
    public static void DecodeFilterOct<T>(Span<T> buffer, int stride)
        where T : unmanaged
    {
        fixed (void* ptr = buffer)
        {
            DecodeFilterOct(ptr, (nuint)buffer.Length, (nuint)stride);
        }
    }

    /// <summary>
    /// <inheritdoc cref="DecodeFilterQuat(void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="buffer"></param>
    /// <param name="stride"></param>
    public static void DecodeFilterQuat<T>(Span<T> buffer, int stride)
        where T : unmanaged
    {
        fixed(void* ptr = buffer)
        {
            DecodeFilterQuat(ptr, (nuint)buffer.Length, (nuint)stride);
        }
    }

    /// <summary>
    /// <inheritdoc cref="DecodeFilterExp(void*, nuint, nuint)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="buffer"></param>
    /// <param name="stride"></param>
    public static void DecodeFilterExp<T>(Span<T> buffer, int stride)
        where T : unmanaged
    {
        fixed(void* ptr = buffer)
        {
            DecodeFilterExp(ptr, (nuint)buffer.Length, (nuint)stride);
        }
    }

    /// <summary>
    /// <inheritdoc cref="EncodeFilterOct(void*, nuint, nuint, int, in float)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="destination"></param>
    /// <param name="count"></param>
    /// <param name="stride"></param>
    /// <param name="bits"></param>
    /// <param name="data"></param>
    public static void EncodeFilterOct<T>(Span<T> destination, int count, int stride, int bits, ReadOnlySpan<float> data)
        where T : unmanaged
    {
        fixed(void* ptr = destination)
        {
            EncodeFilterOct(ptr, (nuint)count, (nuint)stride, bits, data[0]);
        }
    }

    /// <summary>
    /// <inheritdoc cref="EncodeFilterQuat(void*, nuint, nuint, int, in float)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="destination"></param>
    /// <param name="count"></param>
    /// <param name="stride"></param>
    /// <param name="bits"></param>
    /// <param name="data"></param>
    public static void EncodeFilterQuat<T>(Span<T> destination, int count, int stride, int bits, ReadOnlySpan<float> data)
        where T : unmanaged
    {
        fixed (void* ptr = destination)
        {
            EncodeFilterQuat(ptr, (nuint)count, (nuint)stride, bits, data[0]);
        }
    }

    /// <summary>
    /// <inheritdoc cref="EncodeFilterExp(void*, nuint, nuint, int, in float, MeshoptEncodeExpMode)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="destination"></param>
    /// <param name="count"></param>
    /// <param name="stride"></param>
    /// <param name="bits"></param>
    /// <param name="data"></param>
    /// <param name="mode"></param>
    public static void EncodeFilterExp<T>(Span<T> destination, int count, int stride, int bits, ReadOnlySpan<float> data, MeshoptEncodeExpMode mode)
        where T : unmanaged
    {
        fixed (void* ptr = destination)
        {
            EncodeFilterExp(ptr, (nuint)count, (nuint)stride, bits, data[0], mode);
        }
    }
    #endregion

    #region Simplify

    /// <inheritdoc cref="Simplify(ref uint, in uint, nuint, in float, nuint, nuint, nuint, float, uint, out float)"/>
    public static int Simplify(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, int target_index_count, float target_error, uint options, out float result_error)
    {
        return (int)Simplify(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, (nuint)target_index_count, target_error, options, out result_error);
    }

    /// <inheritdoc cref="Simplify(ref uint, in uint, nuint, in float, nuint, nuint, nuint, float, uint, out float)"/>
    public static int Simplify(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, int target_index_count, float target_error, uint options = 0)
    {
        return (int)Simplify(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, (nuint)target_index_count, target_error, options, IntPtr.Zero);
    }


    /// <inheritdoc cref="SimplifyWithAttributes(ref uint, in uint, nuint, in float, nuint, nuint, in float, nuint, in float, nuint, in byte, nuint, float, uint, ref float)"/>
    public static int SimplifyWithAttributes(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, in float vertex_attributes, int vertex_attributes_stride, in float attribute_weights, int attribute_count, ReadOnlySpan<byte> vertex_lock, int target_index_count, float target_error, uint options, out float result_error)
    {
        return (int)SimplifyWithAttributes(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, vertex_attributes, (nuint)vertex_attributes_stride, attribute_weights, (nuint)attribute_count, vertex_lock[0], (nuint)target_index_count, target_error, options, out  result_error);
    }

    /// <inheritdoc cref="SimplifyWithAttributes(ref uint, in uint, nuint, in float, nuint, nuint, in float, nuint, in float, nuint, in byte, nuint, float, uint, ref float)"/>
    public static int SimplifyWithAttributes(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, in float vertex_attributes, int vertex_attributes_stride, in float attribute_weights, int attribute_count, int target_index_count, float target_error, uint options, out float result_error)
    {
        return (int)SimplifyWithAttributes(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, vertex_attributes, (nuint)vertex_attributes_stride, attribute_weights, (nuint)attribute_count, IntPtr.Zero, (nuint)target_index_count, target_error, options, out result_error);
    }

    /// <inheritdoc cref="SimplifyWithAttributes(ref uint, in uint, nuint, in float, nuint, nuint, in float, nuint, in float, nuint, in byte, nuint, float, uint, ref float)"/>
    public static int SimplifyWithAttributes(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, in float vertex_attributes, int vertex_attributes_stride, in float attribute_weights, int attribute_count, ReadOnlySpan<byte> vertex_lock, int target_index_count, float target_error, uint options)
    {
        return (int)SimplifyWithAttributes(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, vertex_attributes, (nuint)vertex_attributes_stride, attribute_weights, (nuint)attribute_count, vertex_lock[0], (nuint)target_index_count, target_error, options, IntPtr.Zero);
    }

    /// <inheritdoc cref="SimplifyWithAttributes(ref uint, in uint, nuint, in float, nuint, nuint, in float, nuint, in float, nuint, in byte, nuint, float, uint, ref float)"/>
    public static int SimplifyWithAttributes(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, in float vertex_attributes, int vertex_attributes_stride, in float attribute_weights, int attribute_count, int target_index_count, float target_error, uint options)
    {
        return (int)SimplifyWithAttributes(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, vertex_attributes, (nuint)vertex_attributes_stride, attribute_weights, (nuint)attribute_count, IntPtr.Zero, (nuint)target_index_count, target_error, options, IntPtr.Zero);
    }

    /// <inheritdoc cref="SimplifySloppy(Span{uint}, ReadOnlySpan{uint}, in float, int, int, int, float, ref float)"/>
    public static int SimplifySloppy(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, int target_index_count, float target_error, out float result_error)
    {
        return (int)SimplifySloppy(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, (nuint)target_index_count, target_error, out result_error);
    }

    /// <inheritdoc cref="SimplifySloppy(Span{uint}, ReadOnlySpan{uint}, in float, int, int, int, float, ref float)"/>
    public static int SimplifySloppy(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, int target_index_count, float target_error)
    {
        return (int)SimplifySloppy(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, (nuint)target_index_count, target_error, IntPtr.Zero);
    }

    /// <inheritdoc cref="SimplifyPoints(ref uint, in float, nuint, nuint, in float, nuint, float, nuint)"/>
    public static int SimplifyPoints(Span<uint> destination, in float vertex_positions, int vertex_count, int vertex_positions_stride, in float vertex_colors, int vertex_colors_stride, float color_weight, int target_vertex_count)
    {
        return (int)SimplifyPoints(ref destination[0], vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, vertex_colors, (nuint)vertex_colors_stride, color_weight, (nuint)target_vertex_count);

    }
    /// <inheritdoc cref="SimplifyPoints(ref uint, in float, nuint, nuint, in float, nuint, float, nuint)"/>
    public static int SimplifyPoints(Span<uint> destination, in float vertex_positions, int vertex_count, int vertex_positions_stride, int vertex_colors_stride, float color_weight, int target_vertex_count)
    {
        return (int)SimplifyPoints(ref destination[0], vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, IntPtr.Zero, (nuint)vertex_colors_stride, color_weight, (nuint)target_vertex_count);
    }

    /// <summary>
    /// <inheritdoc cref="SimplifyScale(in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <returns></returns>
    public static float SimplifyScale(ReadOnlySpan<float> vertex_positions, int vertex_positions_stride)
    {
        return SimplifyScale(vertex_positions[0], (nuint)vertex_positions.Length, (nuint)vertex_positions_stride);
    }
    #endregion

    #region Stripify

    /// <summary>
    /// <inheritdoc cref="Stripify(ref uint, in uint, nuint, nuint, uint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="restart_index"></param>
    /// <returns></returns>
    public static int Stripify(Span<uint> destination, ReadOnlySpan<uint> indices, int vertex_count, int restart_index)
    {
        return (int)Stripify(ref destination[0], indices[0], (nuint)indices.Length, (nuint)vertex_count, (uint)vertex_count);
    }

    /// <inheritdoc cref="StripifyBound(nuint)"/>
    public static int StripifyBound(int index_count)
    {
        return (int)StripifyBound((nuint)index_count);
    }

    /// <summary>
    /// <inheritdoc cref="Unstripify(ref uint, in uint, nuint, uint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="restart_index"></param>
    /// <returns></returns>
    public static int Unstripify(Span<uint> destination, ReadOnlySpan<uint> indices, int restart_index)
    {
        return (int)Unstripify(ref destination[0], indices[0], (nuint)indices.Length, (uint)restart_index);
    }

    /// <inheritdoc cref="UnstripifyBound(nuint)"/>
    public static int UnstripifyBound(int index_count)
    {
        return (int)UnstripifyBound((nuint)index_count);
    }
    #endregion

    #region Analize

    /// <summary>
    /// <inheritdoc cref="AnalyzeVertexCache(in uint, nuint, nuint, uint, uint, uint)"/>
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="cache_size"></param>
    /// <param name="warp_size"></param>
    /// <param name="primgroup_size"></param>
    /// <returns></returns>
    public static MeshoptVertexCacheStatistics AnalyzeVertexCache(ReadOnlySpan<uint> indices, int vertex_count, int cache_size, int warp_size, int primgroup_size)
    {
        return AnalyzeVertexCache(indices[0], (nuint)indices.Length, (nuint)vertex_count, (uint)cache_size, (uint)warp_size, (uint)primgroup_size);
    }

    /// <summary>
    /// <inheritdoc cref="AnalyzeOverdraw(in uint, nuint, in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <returns></returns>
    public static MeshoptOverdrawStatistics AnalyzeOverdraw(ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride)
    {
        return AnalyzeOverdraw(indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride);
    }

    /// <summary>
    /// <inheritdoc cref="AnalyzeVertexFetch(in uint, nuint, nuint, nuint)"/>
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_size"></param>
    /// <returns></returns>
    public static MeshoptVertexFetchStatistics AnalyzeVertexFetch(ReadOnlySpan<uint> indices, int vertex_count, int vertex_size)
    {
        return AnalyzeVertexFetch(indices[0], (nuint)indices.Length, (nuint)vertex_count, (nuint)vertex_size);
    }
    #endregion

    #region Meshlets

    /// <summary>
    /// <inheritdoc cref="BuildMeshlets(ref MeshoptMeshlet, ref uint, ref byte, in uint, nuint, in float, nuint, nuint, nuint, nuint, float)"/>
    /// </summary>
    /// <param name="meshlets"></param>
    /// <param name="meshlet_vertices"></param>
    /// <param name="meshlet_triangles"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <param name="max_vertices"></param>
    /// <param name="max_triangles"></param>
    /// <param name="cone_weight"></param>
    /// <returns></returns>
    public static int BuildMeshlets(ref MeshoptMeshlet meshlets, Span<uint> meshlet_vertices, Span<byte> meshlet_triangles, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride, int max_vertices, int max_triangles, float cone_weight)
    {
        return (int)BuildMeshlets(ref meshlets, ref meshlet_vertices[0], ref meshlet_triangles[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride, (nuint)max_vertices, (nuint)max_triangles, cone_weight);
    }

    /// <summary>
    /// <inheritdoc cref="BuildMeshletsScan(ref MeshoptMeshlet, ref uint, ref byte, in uint, nuint, nuint, nuint, nuint)"/>
    /// </summary>
    /// <param name="meshlets"></param>
    /// <param name="meshlet_vertices"></param>
    /// <param name="meshlet_triangles"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_count"></param>
    /// <param name="max_vertices"></param>
    /// <param name="max_triangles"></param>
    /// <returns></returns>
    public static int BuildMeshletsScan(ref MeshoptMeshlet meshlets, Span<uint> meshlet_vertices, Span<byte> meshlet_triangles, ReadOnlySpan<uint> indices, int vertex_count, int max_vertices, int max_triangles)
    {
        return (int)BuildMeshletsScan(ref meshlets, ref meshlet_vertices[0], ref meshlet_triangles[0], indices[0], (nuint)indices.Length, (nuint)vertex_count, (nuint)max_vertices, (nuint)max_triangles);
    }

    public static int BuildMeshletsBound(int index_count, int max_vertices, int max_triangles)
    {
        return (int)BuildMeshletsBound((nuint)index_count, (nuint)max_vertices, (nuint)max_triangles);
    }

    /// <summary>
    /// <inheritdoc cref="OptimizeMeshlet(ref uint, ref byte, nuint, nuint)"/>
    /// </summary>
    /// <param name="meshlet_vertices"></param>
    /// <param name="meshlet_triangles"></param>
    /// <param name="triangle_count"></param>
    /// <param name="vertex_count"></param>
    public static void OptimizeMeshlet(Span<uint> meshlet_vertices, Span<byte> meshlet_triangles, nuint triangle_count, nuint vertex_count)
    {
        OptimizeMeshlet(ref meshlet_vertices[0], ref meshlet_triangles[0], triangle_count, vertex_count);
    }
    #endregion

    #region Computes

    /// <summary>
    /// <inheritdoc cref="ComputeClusterBounds(in uint, nuint, in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <returns></returns>
    public static MeshoptBounds ComputeClusterBounds(ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride)
    {
        return ComputeClusterBounds(indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride);
    }

    /// <summary>
    /// <inheritdoc cref="ComputeMeshletBounds(in uint, in byte, nuint, in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="meshlet_vertices"></param>
    /// <param name="meshlet_triangles"></param>
    /// <param name="triangle_count"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    /// <returns></returns>
    public static MeshoptBounds ComputeMeshletBounds(ReadOnlySpan<uint> meshlet_vertices, ReadOnlySpan<byte> meshlet_triangles, int triangle_count, in float vertex_positions, int vertex_count, int vertex_positions_stride)
    {
        return ComputeMeshletBounds(meshlet_vertices[0], meshlet_triangles[0], (nuint)triangle_count, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride);
    }
    #endregion

    #region Spatial

    /// <summary>
    /// <inheritdoc cref="SpatialSortRemap(ref uint, in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    public static void SpatialSortRemap(Span<uint> destination, in float vertex_positions, int vertex_count, int vertex_positions_stride)
    {
        SpatialSortRemap(ref destination[0], vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride);
    }

    /// <summary>
    /// <inheritdoc cref="SpatialSortTriangles(ref uint, in uint, nuint, in float, nuint, nuint)"/>
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="indices"></param>
    /// <param name="vertex_positions"></param>
    /// <param name="vertex_count"></param>
    /// <param name="vertex_positions_stride"></param>
    public static void SpatialSortTriangles(Span<uint> destination, ReadOnlySpan<uint> indices, in float vertex_positions, int vertex_count, int vertex_positions_stride)
    {
        SpatialSortTriangles(ref destination[0], indices[0], (nuint)indices.Length, vertex_positions, (nuint)vertex_count, (nuint)vertex_positions_stride);
    }
    #endregion

}
