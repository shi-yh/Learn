using UnityEngine;

namespace ObjectManagement
{
    public class FloatRangeSlierAttribute: PropertyAttribute
    {
        public float Min { get; private set; }

        public float Max { get; private set; }

        public FloatRangeSlierAttribute(float min, float max)
        {
            if (max < min)
            {
                max = min;
            }

            Max = max;

            Min = min;
        }
    }
}