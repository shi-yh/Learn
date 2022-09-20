using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate float Function2D(float x, float t);

    public delegate Vector3 Function3D(float x, float z, float t);

    public enum Function2DName
    {
        Wave,
        MultiWave,
        Ripple
    }

    public enum Function3DName
    {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus
    }

    private static Function2D[] _functions2D = {Wave, MultiWave, Ripple};

    private static Function3D[] _function3D = {Wave, MultiWave, Ripple, Sphere, Torus};

    public static Function3DName GetNextFunctionName(Function3DName name)
    {
        return (int) name < _function3D.Length - 1 ? name + 1 : Function3DName.Wave;
    }

    public static Vector3 Morph(float u, float v, float t, Function3D from, Function3D to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), progress);
    }
    
    
    public static Function2D GetFunction2D(Function2DName name)
    {
        return _functions2D[(int) name];
    }

    public static Function3D GetFunction3D(Function3DName name)
    {
        return _function3D[(int) name];
    }


    private static float Wave(float x, float t)
    {
        return Sin(PI * (x + t));
    }

    private static Vector3 Wave(float x, float z, float t)
    {
        Vector3 p;

        p.x = x;
        p.y = Sin(PI * (x + z + t));
        p.z = z;

        return p;
    }


    /// <summary>
    ///                      sin(2π(x+t))
    ///f(x,t) = sin(π(x+t))+ ——————————————
    ///                            2
    /// </summary>
    /// <param name="x"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private static float MultiWave(float x, float t)
    {
        float y = Sin(PI * (x + 0.5f * t));

        //给正弦波增加更多复杂度的最简单方法是添加另一个具有两倍频率的正弦波。
        //这意味着它的改变速度快两倍，这是通过将正弦函数的参数乘以2来完成的。
        //与此同时，我们将把该函数的结果减半。这样可以使新的正弦波的形状与旧的正弦波相同，但尺寸减半。
        y += 0.5f * Sin(2f * PI * (x + t));

        return y;
    }

    private static Vector3 MultiWave(float x, float z, float t)
    {
        Vector3 p;
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2f * PI * (z + t));
        y += Sin(PI * (x + z + 0.25f * t));
        p.y = y * 0.4f;

        p.x = x;

        p.z = z;

        return p;
    }


    /// <summary>
    ///     sin(π(4d-t))
    /// y= ————————————
    ///        1+10d
    /// </summary>
    /// <param name="x"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private static float Ripple(float x, float t)
    {
        float d = Abs(x);
        float y = Sin(PI * (4f * d - t));
        return y / (1 + 10f * d);
    }

    private static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;

        float d = Sqrt(x * x + z * z);

        float y = Sin(PI * (4f * d - t));

        p.y = y / (1f + 10f * d);

        p.x = x;

        p.z = z;

        return p;
    }


    private static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        float r = Cos(0.5f * PI * t);
        float s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    private static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));

        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;

        p.x = s * Sin(PI * u);

        p.y = r2 * Sin(PI * v);

        p.z = s * Cos(PI * u);

        return p;
    }
}