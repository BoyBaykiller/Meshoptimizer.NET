using System.Runtime.InteropServices;

namespace Meshoptimizer;

/// <summary>
/// Vertex attribute stream<para/>
/// Each element takes size bytes, beginning at data, with stride controlling the spacing between successive elements (stride >= size).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MeshoptStream
{
    public void* Data;
    public nuint Size;
    public nuint Stride;

    public MeshoptStream(void* data, nuint size, nuint stride)
    {
        Data = data;
        Size = size;
        Stride = stride;
    }

    public MeshoptStream(IntPtr data, nuint size, nuint stride) : this(data.ToPointer(), size, stride) { }

};
