using UnityEngine;

namespace ObjectManagement
{
    public sealed class RotationShapeBehavior : ShapeBehavior
    {
        public Vector3 angularVelocity { get; set; }

        public override void GameUpdate(Shape shape)
        {
            shape.transform.Rotate(angularVelocity * Time.deltaTime);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(angularVelocity);
        }

        public override void Load(GameDataReader reader)
        {
            angularVelocity = reader.ReadVector3();
        }

        public override void Recycle()
        {
            ShapeBehaviorPool<RotationShapeBehavior>.Reclaim(this);
        }

        public override ShapeBehaviorType BehaviorType
        {
            get => ShapeBehaviorType.Rotation;
        }
    }
}