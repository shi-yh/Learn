using System;
using UnityEngine;

public class Shape : PersistableObject
{
    private int _shapeId = Int32.MinValue;

    private Color[] _colors;

    private MeshRenderer[] _meshRenderers;

    private static readonly int colorPropertyId = Shader.PropertyToID("_Color");

    private static MaterialPropertyBlock _sharedPropertyBlock;

    public int MaterialId { get; private set; }
    public Vector3 AngularVelocity { get; set; }
    public Vector3 Velocity { get; set; }

    public int ColorCount => _colors.Length;

    private ShapeFactory _originFactory;

    public ShapeFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            if (_originFactory==null)
            {
                _originFactory = value;
            }
            else
            {
                Debug.LogError("Not allowed to change origin factory");
            }
        }
    }
    

    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _colors = new Color[_meshRenderers.Length];
    }

    public void SetMaterial(Material material, int materialId)
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material = material;
        }

        MaterialId = materialId;
    }

    private void SetColor(Color color)
    {
        ///对材质属性改变会导致创建一个新的材质，使用MaterialPropertyBlock可以避免这种情况
        if (_sharedPropertyBlock == null)
        {
            _sharedPropertyBlock = new MaterialPropertyBlock();
        }

        _sharedPropertyBlock.SetColor(colorPropertyId, color);

        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _colors[i] = color;
            _meshRenderers[i].SetPropertyBlock(_sharedPropertyBlock);
        }
    }

    public void SetColor(Color color, int index)
    {
        if (_sharedPropertyBlock == null)
        {
            _sharedPropertyBlock = new MaterialPropertyBlock();
        }

        _sharedPropertyBlock.SetColor(colorPropertyId, color);
        _colors[index] = color;
        _meshRenderers[index].SetPropertyBlock(_sharedPropertyBlock);
    }


    public int ShapeId
    {
        get => _shapeId;
        set
        {
            if (_shapeId == int.MinValue && value != int.MinValue)
            {
                _shapeId = value;
            }
            else
            {
                Debug.LogError("Not allowed to change shapeId");
            }
        }
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(_colors.Length);
        for (int i = 0; i < _colors.Length; i++)
        {
            writer.Write(_colors[i]);
        }
        writer.Write(AngularVelocity);
        writer.Write(Velocity);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        if (reader.Version>=5)
        {
            LoadColors(reader);
        }
        else
        {
            SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        }
        AngularVelocity = reader.Version >= 3 ? reader.ReadVector3() : Vector3.zero;
        Velocity = reader.Version >= 3 ? reader.ReadVector3() : Vector3.zero;
    }

    private void LoadColors(GameDataReader reader)
    {
        int count = reader.ReadInt();
        int max = count <= _colors.Length ? count : _colors.Length;
        
        for (int i = 0; i < max; i++)
        {
            SetColor(reader.ReadColor(), i);
        }

        if (count>_colors.Length)
        {
            for (int i = _colors.Length; i < count; i++)
            {
                reader.ReadColor();
            }
        }
        else if (count<_colors.Length)
        {
            for (int i = count; i < _colors.Length; i++)
            {
                SetColor(Color.white,i);
            }
        }
        
    }

    public void GameUpdate()
    {
        transform.Rotate(AngularVelocity * Time.deltaTime);
        transform.localPosition += Velocity * Time.deltaTime;
    }

    public void Recycle()
    {
        _originFactory.Reclaim(this);
    }
    
}