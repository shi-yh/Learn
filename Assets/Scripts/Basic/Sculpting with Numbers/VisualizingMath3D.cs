using UnityEngine;

public class VisualizingMath3D : MonoBehaviour
{
    [SerializeField] private Transform _point;

    [SerializeField, Range(10, 100)] private int resolution = 10;

    [SerializeField] private FunctionLibrary.Function3DName _functionName = default;

    private Transform[] _points;

    private void Awake()
    {
        float step = 2f / resolution;
        var scale = Vector3.one * step;

        _points = new Transform[resolution * resolution];

        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Transform point = Instantiate(_point, transform, false);
                
                point.localScale = scale;

                _points[i * resolution + j] = point;
            }
        }
    }


    private void Update()
    {
        float time = Time.time;

        float step = 2f / resolution;
        
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                _points[i * resolution + j].position = FunctionLibrary.GetFunction3D(_functionName)((i+0.5f)*step-1, (j+0.5f)*step-1, time);
            }
        }
    }
}