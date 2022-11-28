using UnityEngine;

public enum Direction
{
    North,
    East,
    South,
    West
}

public static class DirectionExtension
{
    private static Quaternion[] rotations =
    {
        Quaternion.identity,
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 180, 1),
        Quaternion.Euler(0, 270, 0)
    };

    public static Quaternion GetRotation(this Direction direction)
    {
        return rotations[(int) direction];
    }
}