using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField]
    private SpawnZone _spawnZone;
    
    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.spawnZone = _spawnZone;
    }

  
}
