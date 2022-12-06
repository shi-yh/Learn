using UnityEngine;

namespace ObjectManagement
{
    public sealed class MovementShapeBehavior : ShapeBehavior
    {
        public Vector3 velocity { get; set; }

        public override void GameUpdate(Shape shape)
        {
            shape.transform.localPosition += velocity * Time.deltaTime;
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(velocity);
        }

        public override void Load(GameDataReader reader)
        {
            velocity = reader.ReadVector3();
        }

        public override void Recycle()
        {
            ShapeBehaviorPool<MovementShapeBehavior>.Reclaim(this);
        }

        public override ShapeBehaviorType BehaviorType { get=> ShapeBehaviorType.Movement; }
    }
}