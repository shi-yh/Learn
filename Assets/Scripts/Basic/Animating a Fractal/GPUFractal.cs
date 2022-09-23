using UnityEngine;

public class GPUFractal : MonoBehaviour
{
    struct FractalPart
    {
        public Vector3 direction, worldPosition;

        public Quaternion rotation, worldRotation;
    }


    private static Vector3[] directions = {Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back};

    private static Quaternion[] rotations = {Quaternion.identity, Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, 90), Quaternion.Euler(90, 0, 0), Quaternion.Euler(-90, 0, 0)};

    private FractalPart[][] _parts;


    private void Awake()
    {
        // _parts[0][0] = CreatePart(0);
        //
        // for (int i = 1; i < _parts.Length; i++)
        // {
        //     FractalPart[] levelParts = _parts[i];
        //     for (int j = 0; j < levelParts.Length; j += 5)
        //     {
        //         for (int k = 0; k < 5; k++)
        //         {
        //             levelParts[j + k] = CreatePart(k);
        //         }
        //     }
        // }
    }


    private void Update()
    {
        // Quaternion deltaRotation = Quaternion.Euler(0, 22.5f * Time.deltaTime, 0);
        //
        // FractalPart rootPart = _parts[0][0];
        //
        // rootPart.rotation *= deltaRotation;
        //
        // rootPart.worldRotation = rootPart.rotation;
        //
        // _parts[0][0] = rootPart;
    }

    FractalPart CreatePart(int childIndex)
    {
        return new FractalPart
        {
            direction = directions[childIndex],
            rotation = rotations[childIndex]
        };
    }
}