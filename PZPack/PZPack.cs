using PZPack.Interface;
using static PZPack.Interface.IPZPack;

namespace PZPack;

/// <summary>
/// Base class for Project Zomboid packs
/// </summary>
public abstract class PZPack : IPZPack
{
    private PZPage[] _pages;

    /// <summary>
    /// Project zomboid pack type
    /// </summary>
    public virtual PZPackType Type => PZPackType.NotAPZPack;
    /// <summary>
    /// Default initializer
    /// </summary>
    public PZPack()
    {
        _pages = [];
    }
    /// <summary>
    /// Texture Atlas
    /// </summary>
    public PZPage[] Pages
    {
        get { return _pages; }
        set { _pages = value; }
    }
    /// <summary>
    /// Encodes the PZPack into the specified stream
    /// </summary>
    /// <param name="stream">The stream to encode into</param>
    public abstract void Encode(Stream stream);

    /// <summary>
    /// Determines if a file is a valid Project Zomboid pack and returns its type
    /// </summary>
    /// <param name="filename">The path to the file to check</param>
    /// <returns>The type of PZPack if valid, otherwise NotAPZPack</returns>
    public static PZPackType IsFileAPZPack(string filename)
    {
        using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
        if (fs.Length < 56)
            return PZPackType.NotAPZPack;
        using BinaryReader br = new(fs);
        uint magic = br.ReadUInt32();
        if (magic == IPZPackV2.PZ_PACKV2_MAGIC)
            return PZPackType.V2;
        else
        {
            fs.Seek(-4, SeekOrigin.End);
            magic = br.ReadUInt32();
            if (magic == IPZPackV1.PZ_PACKV1_END_MARKER)
                return PZPackType.V1;
        }
        return PZPackType.NotAPZPack;
    }

    /// <summary>
    /// Opens a file as a Project Zomboid pack version 1
    /// </summary>
    /// <param name="filename">The path to the PZPackV1 file</param>
    /// <returns>A new instance of PZPackV1 containing the parsed data</returns>
    public static PZPackV1 OpenV1(string filename)
    {
        using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
        return new PZPackV1(fs);
    }

    /// <summary>
    /// Opens a file as a Project Zomboid pack version 2
    /// </summary>
    /// <param name="filename">The path to the PZPackV2 file</param>
    /// <returns>A new instance of PZPackV2 containing the parsed data</returns>
    public static PZPackV2 OpenV2(string filename)
    {
        using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
        return new PZPackV2(fs);
    }
}
