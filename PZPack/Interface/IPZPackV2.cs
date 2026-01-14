namespace PZPack.Interface;

/// <summary>
/// A basic project zomboid pack, v2
/// </summary>
public interface IPZPackV2 : IPZPack
{
    /// <summary>
    /// Magic Code
    /// </summary>
    const uint PZ_PACKV2_MAGIC = 0x4B505A50;
    /// <summary>
    /// The magic code for PZPack version 2
    /// </summary>
    public uint Magic { get; }
    /// <summary>
    /// Pack mask
    /// </summary>
    public int Mask { get; set; }
}
