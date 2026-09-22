using System.Buffers.Binary;
using PZPack.Exceptions;
using PZPack.Interface;
using static PZPack.Interface.IPZPack;

namespace PZPack;

/// <summary>
/// A basic project zomboid pack, v1
/// </summary>
public class PZPackV1 : PZPack, IPZPackV1
{
    private static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];

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
            PZPage page = new(br);
            page.Png = ReadPagePng(br);
            pages.Add(page);
            uint marker = br.ReadUInt32();
            if (marker != IPZPackV1.PZ_PACKV1_END_MARKER)
                throw new PZPackFormatException("Page is not terminated by the PZPackV1 end marker.");
        }
        Pages = [.. pages];
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
    /// Reads one raw PNG stream (no length prefix) starting at the current position.
    /// The stream ends at the IEND chunk, which is part of the returned data.
    /// </summary>
    private static byte[] ReadPagePng(BinaryReader br)
    {
        Stream stream = br.BaseStream;
        long start = stream.Position;
        Span<byte> header = stackalloc byte[8];
        if (stream.Read(header) != 8 || !header.SequenceEqual(PngSignature))
            throw new PZPackFormatException("Page image is not a PNG stream.");
        long position = start + 8;
        while (true)
        {
            stream.Seek(position, SeekOrigin.Begin);
            if (stream.Read(header) != 8)
                throw new PZPackFormatException("Page image ends inside a PNG chunk.");
            uint length = BinaryPrimitives.ReadUInt32BigEndian(header);
            bool isIend = header[4..].SequenceEqual("IEND"u8);
            position += 8 + length + 4;
            if (isIend)
                break;
        }
        stream.Seek(start, SeekOrigin.Begin);
        return br.ReadBytes((int)(position - start));
    }

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
            bw.Write(page.Png);
            bw.Write(_end_marker);
        }
    }
}
