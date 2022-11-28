using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DefenceEnemy : MonoBehaviour
{
    private DefenceEnemyFactory _originEnemyFactory;

    /// <summary>
    /// 记录的当前瓦片和目的地瓦片
    /// </summary>
    private DefenceGameTile _tileFrom, _tileTo;

    private Vector3 _positionFrom, _positionTo;

    private float _progress;


    public DefenceEnemyFactory OriginFactory
    {
        get => _originEnemyFactory;
        set { _originEnemyFactory = value; }
    }

    public void SpawnOn(DefenceGameTile tile)
    {
        OnReachTile(tile);
    }

    private void OnReachTile(DefenceGameTile tile)
    {
        transform.localPosition = tile.transform.localPosition;
        _tileFrom = tile;
        _tileTo = tile.NextTileOnPath;
        if (_tileTo == null)
        {
            return;
        }

        _positionFrom = _progress > 0 ? _positionTo : _tileFrom.transform.localPosition;
        _positionTo = _tileTo.ExitPoint;
    }


    public bool GameUpdate()
    {
        _progress += Time.deltaTime;
        while (_progress >= 1f)
        {
            OnReachTile(_tileTo);
            _progress -= 1f;
            if (_tileTo == null)
            {
                _originEnemyFactory.Reclaim(this);
                return false;
            }
        }

        transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);

        return true;
    }
}