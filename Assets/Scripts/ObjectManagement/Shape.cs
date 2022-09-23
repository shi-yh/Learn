using System;
using UnityEngine;

public class Shape : PersistableObject
{
    private int _shapeId = Int32.MinValue;

    private Color _color;

    private MeshRenderer _meshRenderer;
    
    private static readonly int colorPropertyId = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock sharedPropertyBlock;

    public int MaterialId { get; private set; }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material material, int materialId)
    {
        
        _meshRenderer.material = material;

        MaterialId = materialId;
    }

    public void SetColor(Color color)
    {
        _color = color;

        ///对材质属性改变会导致创建一个新的材质，使用MaterialPropertyBlock可以避免这种情况

        if (sharedPropertyBlock==null)
        {
            sharedPropertyBlock  = new MaterialPropertyBlock();

        }
        sharedPropertyBlock.SetColor(colorPropertyId,color);
        _meshRenderer.SetPropertyBlock(sharedPropertyBlock);
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
        writer.Write(_color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
    }
}