namespace PZPack.Interface;

/// <summary>
/// A basic project zomboid pack
/// </summary>
public interface IPZPack
{
    /// <summary>
    /// Enumerates the different types of PZPack
    /// </summary>
    enum PZPackType
    {
        /// <summary>
        /// Not a valid Project Zomboid pack
        /// </summary>
        NotAPZPack = -1,
        /// <summary>
        /// Project Zomboid pack version 1
        /// </summary>
        V1 = 0,
        /// <summary>
        /// Project Zomboid pack version 2
        /// </summary>
        V2,
    }
    /// <summary>
    /// Gets the type of PZPack
    /// </summary>
    public PZPackType Type { get; }
    /// <summary>
    /// Texture Atlas pages, each carrying its own PNG atlas image
    /// </summary>
    public PZPage[] Pages { get; set; }

    /// <summary>
    /// Encodes the PZPack into the specified stream
    /// </summary>
    /// <param name="stream">The stream to encode into</param>
    public void Encode(Stream stream);
}
