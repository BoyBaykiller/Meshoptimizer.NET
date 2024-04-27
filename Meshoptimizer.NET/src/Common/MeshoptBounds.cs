using System.Numerics;
using System.Runtime.InteropServices;

namespace Meshoptimizer;

[StructLayout(LayoutKind.Sequential)]
public struct MeshoptBounds
{
    /* bounding sphere, useful for frustum and occlusion culling */
    public float CenterX;
    public float CenterY;
    public float CenterZ;
    public float Radius;

    /* normal cone, useful for backface culling */
    public float ConeApexX;
    public float ConeApexY;
    public float ConeApexZ;

    public float ConeAxisX;
    public float ConeAxisY;
    public float ConeAxisZ;

    /* = cos(angle/2) */
    public float ConeCutoff;

    /* normal cone axis and cutoff, stored in 8-bit SNORM format; decode using x/127.0 */
    public byte ConeAxisS8X;
    public byte ConeAxisS8Y;
    public byte ConeAxisS8Z;
    public byte ConeCutoffS8;

    public MeshoptBounds(float centerX, float centerY, float centerZ, float radius, float coneApexX, float coneApexY, float coneApexZ, float coneAxisX, float coneAxisY, float coneAxisZ, float coneCutoff, byte coneAxisS8X, byte coneAxisS8Y, byte coneAxisS8Z, byte coneCutoffS8)
    {
        CenterX = centerX;
        CenterY = centerY;
        CenterZ = centerZ;
        Radius = radius;
        ConeApexX = coneApexX;
        ConeApexY = coneApexY;
        ConeApexZ = coneApexZ;
        ConeAxisX = coneAxisX;
        ConeAxisY = coneAxisY;
        ConeAxisZ = coneAxisZ;
        ConeCutoff = coneCutoff;
        ConeAxisS8X = coneAxisS8X;
        ConeAxisS8Y = coneAxisS8Y;
        ConeAxisS8Z = coneAxisS8Z;
        ConeCutoffS8 = coneCutoffS8;
    }
}
