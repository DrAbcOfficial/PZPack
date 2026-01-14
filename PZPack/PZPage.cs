using System.Text;

namespace PZPack;

/// <summary>
/// Represents a texture atlas page in a PZPack
/// </summary>
public class PZPage
{
    private string _name;
    private PZEntry[] _entries;
    private int _mask;
    /// <summary>
    /// Initializes a new instance of PZPage with default values
    /// </summary>
    public PZPage()
    {
        _name = string.Empty;
        _entries = [];
        _mask = 0;
    }
    internal PZPage(BinaryReader br)
    {
        uint name_len = br.ReadUInt32();
        _name = Encoding.UTF8.GetString(br.ReadBytes((int)name_len));
        uint entries_count = br.ReadUInt32();
        _mask = br.ReadInt32();
        List<PZEntry> entries = [];
        for (uint i = 0; i < entries_count; i++)
        {
            entries.Add(new PZEntry(br));
        }
        _entries = [.. entries];
    }
    /// <summary>
    /// Texture Name
    /// </summary>
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    /// <summary>
    /// Gets or sets the entries (sprites) in this page
    /// </summary>
    public PZEntry[] Entries
    {
        get { return _entries; }
        set { _entries = value; }
    }
    /// <summary>
    /// Image Mask
    /// </summary>
    public int Mask
    {
        get { return _mask; }
        set { _mask = value; }
    }

    internal void Encode(BinaryWriter bw)
    {
        byte[] name_bytes = Encoding.UTF8.GetBytes(_name);
        bw.Write((uint)name_bytes.Length);
        bw.Write(name_bytes);
        bw.Write((uint)_entries.Length);
        bw.Write(_mask);
        foreach (var entry in _entries)
        {
            entry.Encode(bw);
        }
    }
}
