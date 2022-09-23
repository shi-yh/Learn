using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataReader
{
    private BinaryReader _reader;

    public GameDataReader(BinaryReader reder)
    {
        _reader = reder;
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
}