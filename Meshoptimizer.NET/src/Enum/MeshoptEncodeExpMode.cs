namespace Meshoptimizer;

public enum MeshoptEncodeExpMode
{
    /// < Comment doc File ></Comment>
    Separate,

    /// <summary>
    /// When encoding exponents, use shared value for all components of each vector (better compression).
    /// </summary>
    SharedVector,

    /// <summary>
    /// When encoding exponents, use shared value for each component of all vectors (best compression).
    /// </summary>
    SharedComponent,
}
