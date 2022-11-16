using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeBehaviorType
{
    Movement,
    Rotation,
    Oscillation
}

public static class ShapeBehaviorTypeMethods
{
    public static ShapeBehavior GetInstance(this ShapeBehaviorType type)
    {
        switch (type)
        {
            case ShapeBehaviorType.Movement:
            {
                return ShapeBehaviorPool<MovementShapeBehavior>.Get();
            }
            case ShapeBehaviorType.Rotation:
            {
                return ShapeBehaviorPool<RotationShapeBehavior>.Get();
            }
            case ShapeBehaviorType.Oscillation:
            {
                return ShapeBehaviorPool<OscillationShapeBehavior>.Get();
            }
        }

        return null;
    }
}


public abstract class ShapeBehavior
{
    public abstract void GameUpdate(Shape shape);

    public abstract void Save(GameDataWriter writer);

    public abstract void Load(GameDataReader reader);

    public abstract void Recycle();

    public abstract ShapeBehaviorType BehaviorType { get; }
}