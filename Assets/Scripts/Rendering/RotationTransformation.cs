using UnityEngine;

namespace Rendering
{
    public class RotationTransformation : Transformation
    {
        public Vector3 rotation;

        public override Vector3 Apply(Vector3 point)
        {
            ///这里传进来的值是角度，先转换成弧度
            /// 二维坐标系下，任意一个点可以表示为（x,y）
            /// 也可以表示为x*(1,0)+y*(0,1)
            /// 当有旋转的时候可以表示为x*（cosz,sinz）+y(-sinz,cosz)   旋转0度时
            float radZ = rotation.z * Mathf.Deg2Rad;
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);


            return new Vector3(point.x * cosZ - point.y * sinZ, point.x * sinZ + point.y * cosZ, point.z);
        }
    }
}