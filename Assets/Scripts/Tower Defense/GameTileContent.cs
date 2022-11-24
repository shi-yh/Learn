using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DefenceGameTileContentType
{
    Empty,
    Destination,
}

public class GameTileContent : MonoBehaviour
{
    [SerializeField] private DefenceGameTileContentType _type = default;

    public DefenceGameTileContentType Type => _type;
}