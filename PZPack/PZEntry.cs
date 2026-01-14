using System.Drawing;
using System.Text;

namespace PZPack;

/// <summary>
/// Represents a sprite entry in a PZPack
/// </summary>
public class PZEntry
{
    private string _name;
    private Point _pos;
    private Size _size;
    private Size _offset;
    private Size _total_size;
    /// <summary>
    /// Initializes a new instance of PZEntry with default values
    /// </summary>
    public PZEntry()
    {
        _name = string.Empty;
        _pos = new();
        _size = new();
        _offset = new();
        _total_size = new();
    }

    internal PZEntry(BinaryReader br)
    {
        uint name_len = br.ReadUInt32();
        _name = Encoding.UTF8.GetString(br.ReadBytes((int)name_len));
        _pos = new()
        {
            X = (int)br.ReadUInt32(),
            Y = (int)br.ReadUInt32()
        };
        _size = new()
        {
            Width = (int)br.ReadUInt32(),
            Height = (int)br.ReadUInt32()
        };
        _offset = new()
        {
            Width = (int)br.ReadUInt32(),
            Height = (int)br.ReadUInt32()
        };
        _total_size = new()
        {
            Width = (int)br.ReadUInt32(),
            Height = (int)br.ReadUInt32()
        };
    }
    /// <summary>
    /// Sprite name 
    /// </summary>
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    /// <summary>
    /// Crop starting point in Page image
    /// </summary>
    public Point Position
    {
        get { return _pos; }
        set { _pos = value; }
    }
    /// <summary>
    /// The width and height of the cropping area
    /// </summary>
    public Size Size
    {
        get { return _size; }
        set { _size = value; }
    }
    /// <summary>
    /// Paste the cropping result to the offset position of the output image
    /// </summary>
    public Size Offset
    {
        get { return _offset; }
        set { _offset = value; }
    }
    /// <summary>
    /// The total dimensions of the output image (typically ≥ the crop size, for padding or alignment)
    /// </summary>
    public Size TotalSize
    {
        get { return _total_size; }
        set { _total_size = value; }
    }

    internal void Encode(BinaryWriter bw)
    {
        byte[] name_bytes = Encoding.UTF8.GetBytes(_name);
        bw.Write((uint)name_bytes.Length);
        bw.Write(name_bytes);
        bw.Write((uint)_pos.X);
        bw.Write((uint)_pos.Y);
        bw.Write((uint)_size.Width);
        bw.Write((uint)_size.Height);
        bw.Write((uint)_offset.Width);
        bw.Write((uint)_offset.Height);
        bw.Write((uint)_total_size.Width);
        bw.Write((uint)_total_size.Height);
    }
}
