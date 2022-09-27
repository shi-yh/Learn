using System.IO;
using UnityEngine;

public class GameDataReader
{
    private BinaryReader _reader;

    public int Version { get; }

    public GameDataReader(BinaryReader reder, int version)
    {
        _reader = reder;
        Version = version;
    }

    public Random.State ReadRandomState()
    {
        return JsonUtility.FromJson<Random.State>(_reader.ReadString());
    }

    public float ReadFloat()
    {
        return _reader.ReadSingle();
    }

    public int ReadInt()
    {
        return _reader.ReadInt32();
    }

    public Quaternion ReadQuaternion()
    {
        Quaternion value;

        value.x = _reader.ReadSingle();
        value.y = _reader.ReadSingle();
        value.z = _reader.ReadSingle();
        value.w = _reader.ReadSingle();

        return value;
    }

    public Vector3 ReadVector3()
    {
        Vector3 value;

        value.x = _reader.ReadSingle();
        value.y = _reader.ReadSingle();
        value.z = _reader.ReadSingle();

        return value;
    }

    public Color ReadColor()
    {
        Color c;

        c.r = _reader.ReadSingle();
        c.g = _reader.ReadSingle();
        c.b = _reader.ReadSingle();
        c.a = _reader.ReadSingle();


        return c;
    }
}