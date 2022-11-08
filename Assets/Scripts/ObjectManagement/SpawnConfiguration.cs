using UnityEngine;

[System.Serializable]
public class SpawnConfiguration
{
    public enum SpawnMovementDirection
    {
        Forward,
        Upward,
        Outward,
        Random
    }

    public ShapeFactory[] factoryps;

    public SpawnMovementDirection spawnMovementDirection;

    public FloatRange spawnSpeed;

    public FloatRange angularSpeed;

    public FloatRange scale;

    public ColorRangeHSV color;

    public bool uniformColor;
}