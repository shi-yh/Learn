using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceGame : MonoBehaviour
{
    [SerializeField] private Vector2Int _boradSize = new Vector2Int(11, 11);

    [SerializeField] private DefenceGameBoard _board = default;

    [SerializeField] private DefenceGameTileFactory _tileContectFactory = default;

    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    private void Awake()
    {
        _board.Initialize(_boradSize, _tileContectFactory);
    }


    private void OnValidate()
    {
        if (_boradSize.x < 2)
        {
            _boradSize.x = 2;
        }

        if (_boradSize.y < 2)
        {
            _boradSize.y = 2;
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPath = !_board.ShowPath;
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            _board.ShowGrid = !_board.ShowGrid;
        }
    }

    private void HandleTouch()
    {
        DefenceGameTile tile = _board.GetTile(TouchRay);

        if (tile != null)
        {
            _board.ToggleWall(tile);
        }
    }

    private void HandleAlternativeTouch()
    {
        DefenceGameTile tile = _board.GetTile(TouchRay);

        if (tile != null)
        {
            _board.ToggleDestination(tile);
        }
    }
}