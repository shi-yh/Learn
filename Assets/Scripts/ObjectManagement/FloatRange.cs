using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    public float min, max;

    public float RandomValueRange
    {
        get => Random.Range(min, max);
    }
}