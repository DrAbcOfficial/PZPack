namespace PZPack.Interface;

/// <summary>
/// Project Zomboid tiles pack version 1
/// </summary>
public interface IPZPackV1 : IPZPack
{
    /// <summary>
    /// V1 end marker
    /// </summary>
    const uint PZ_PACKV1_END_MARKER = 0xDEADBEEF;
    /// <summary>
    /// The end marker for PZPack version 1
    /// </summary>
    uint EndMarker { get; }
}
