using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DefenceGameTileContentType
{
    Empty,
    Destination,
    Wall,
    SpawnPoint
}

public class GameTileContent : MonoBehaviour
{
    [SerializeField] private DefenceGameTileContentType _type = default;

    public DefenceGameTileContentType Type => _type;

    private DefenceGameTileFactory _originFactory;

    public DefenceGameTileFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory==null,"Redefined origin Factory");
            _originFactory = value;
        }
    }

    public void Recycle()
    {
        _originFactory.Reclaim(this);
    }

}