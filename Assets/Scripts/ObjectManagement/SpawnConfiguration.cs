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


     public SpawnMovementDirection spawnMovementDirection;

     public FloatRange spawnSpeed;

     public FloatRange angularSpeed;

     public FloatRange scale;

     public ColorRangeHSV color;


}

