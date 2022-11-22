using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceGame : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boradSize = new Vector2Int(11, 11);

    [SerializeField]
    private DefenceGameBoard _board = default;

    private void Awake()
    {
        _board.Initialize(_boradSize);
    }


    private void OnValidate()
    {
        if (_boradSize.x<2)
        {
            _boradSize.x = 2;
        }

        if (_boradSize.y<2)
        {
            _boradSize.y = 2;
        }
        
    }
}
