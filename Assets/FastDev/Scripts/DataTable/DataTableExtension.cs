﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

public static class DataTableExtension
{
    private const string DataRowClassPrefixName = "GameMain.DR";
    internal static readonly char[] DataSplitSeparators = new char[] { '\t' };
    internal static readonly char[] DataTrimSeparators = new char[] { '\"' };
    
    public static Color32 ParseColor32(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Color32(byte.Parse(splitedValue[0]), byte.Parse(splitedValue[1]), byte.Parse(splitedValue[2]),
            byte.Parse(splitedValue[3]));
    }

    public static Color ParseColor(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Color(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }

    public static Quaternion ParseQuaternion(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Quaternion(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }

    public static Rect ParseRect(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Rect(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }

    public static Vector2 ParseVector2(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector2(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]));
    }

    public static Vector3 ParseVector3(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
    }

    public static Vector4 ParseVector4(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector4(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }
}