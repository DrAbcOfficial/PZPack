using PZPack.Interface;
using PZPack.Exceptions;
using static PZPack.Interface.IPZPack;

namespace PZPack;

/// <summary>
/// A basic project zomboid pack, v2
/// </summary>
public class PZPackV2 : PZPack, IPZPackV2
{

    private uint _magic;
    private int _mask;

    /// <summary>
    /// Default initializer
    /// </summary>
    public PZPackV2() : base()
    {
        _magic = IPZPackV2.PZ_PACKV2_MAGIC;
        _mask = 0;
    }

    internal PZPackV2(Stream stream)
    {
        if (stream.Length < 64)
            throw new PZPackFormatException("Stream is too small to fit a PZPackV2");
        using BinaryReader br = new(stream);
        uint magic = br.ReadUInt32();
        if (magic != IPZPackV2.PZ_PACKV2_MAGIC)
            throw new PZPackFormatException("Stream is not a valid PZPackV2 file!");
        _magic = magic;
        _mask = br.ReadInt32();
        uint page_count = br.ReadUInt32();
        List<PZPage> pages = [];
        for (uint i = 0; i < page_count; i++)
        {
            pages.Add(new PZPage(br));
        }
        Pages = [.. pages];
        uint image_data_len = br.ReadUInt32();
        Png = br.ReadBytes((int)image_data_len);
    }

    /// <summary>
    /// Pack type
    /// </summary>
    public override PZPackType Type => PZPackType.V2;

    /// <summary>
    /// Magic code
    /// </summary>
    public uint Magic => _magic;
    /// <summary>
    /// Pack mask
    /// </summary>
    public int Mask
    {
        get { return _mask; }
        set { _mask = value; }
    }
    /// <summary>
    /// Encodes the PZPackV2 into a stream
    /// </summary>
    /// <param name="stream">The stream to encode into</param>
    public override void Encode(Stream stream)
    {
        using BinaryWriter bw = new(stream);
        bw.Write(_magic);
        bw.Write(_mask);
        bw.Write((uint)Pages.Length);
        foreach (var page in Pages)
        {
            page.Encode(bw);
        }
        bw.Write((uint)Png.Length);
        bw.Write(Png);
    }
}
