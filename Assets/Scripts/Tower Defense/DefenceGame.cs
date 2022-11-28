using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DefenceGame : MonoBehaviour
{
    [SerializeField] private Vector2Int _boradSize = new Vector2Int(11, 11);

    [SerializeField] private DefenceGameBoard _board = default;

    [SerializeField] private DefenceGameTileFactory _tileContectFactory = default;

    [SerializeField] private DefenceEnemyFactory _enemyFactory;

    [SerializeField, Range(0.1f, 10f)] private float _spawnSpeed = 1;

    private DefenceEnemyCollection _enemies = new DefenceEnemyCollection();
    

    private float _spwanProgress;
    
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

        _spwanProgress += _spawnSpeed * Time.deltaTime;
        while (_spwanProgress>=1f)
        {
            _spwanProgress -= 1f;
            SpawnEnemy();
        }
        
        _enemies.GameUpdate();

    }

    private void SpawnEnemy()
    {
        DefenceGameTile tile = _board.GetSpawnPoint(Random.Range(0, _board.SpawnPointCount));

        DefenceEnemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(tile);
        _enemies.Add(enemy);
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleDestination(tile);
            }
            else
            {
                _board.ToggleSpawnPoint(tile);
            }
        }
    }
}