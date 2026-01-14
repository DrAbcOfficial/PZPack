using PZPack.Interface;
using PZPack.Exceptions;
using static PZPack.Interface.IPZPack;

namespace PZPack;

/// <summary>
/// A basic project zomboid pack, v1
/// </summary>
public class PZPackV1 : PZPack, IPZPackV1
{
    private readonly uint _end_marker;

    /// <summary>
    /// Default initializer
    /// </summary>
    public PZPackV1() : base()
    {
        _end_marker = IPZPackV1.PZ_PACKV1_END_MARKER;
    }

    internal PZPackV1(Stream stream)
    {
        if (!stream.CanSeek)
            throw new PZPackArgumentException(nameof(stream), "Stream must support seeking.");
        if (stream.Length < 56)
            throw new PZPackFormatException("Stream is too short to be a valid PZPackV1.");
        stream.Seek(-4, SeekOrigin.End);
        Span<byte> buffer = stackalloc byte[4];
        int bytesRead = stream.Read(buffer);
        if (bytesRead != 4)
            throw new PZPackIOException("Failed to read PZPackV1 end marker.");
        uint endmarker = BitConverter.ToUInt32(buffer);
        if (endmarker != IPZPackV1.PZ_PACKV1_END_MARKER)
            throw new PZPackFormatException("Stream is not a PZPackV1 file.");
        _end_marker = endmarker;
        stream.Seek(0, SeekOrigin.Begin);
        using BinaryReader br = new(stream);
        uint page_count = br.ReadUInt32();
        List<PZPage> pages = [];
        for (uint i = 0; i < page_count; i++)
        {
            pages.Add(new PZPage(br));
        }
        Pages = [.. pages];
        byte[] png = new byte[stream.Length - stream.Position - 4];
        br.Read(png);
        Png = png;
    }
    /// <summary>
    /// end marker
    /// </summary>
    public uint EndMarker => _end_marker;

    /// <summary>
    /// Pack type
    /// </summary>
    public override PZPackType Type  => PZPackType.V1;

    /// <summary>
    /// Encodes the PZPackV1 into a stream
    /// </summary>
    /// <param name="stream">The stream to encode into</param>
    public override void Encode(Stream stream)
    {
        using BinaryWriter bw = new(stream);
        bw.Write((uint)Pages.Length);
        foreach(var page in Pages)
        {
            page.Encode(bw);
        }
        bw.Write(Png);
        bw.Write(_end_marker);
    }
}
