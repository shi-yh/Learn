using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NucleonSpawner : MonoBehaviour
{

    public float timeBetweenSpawns;

    public float spawnDistance;

    public Nucleon[] nucleonPrefabs;

    private float _timeSinceLastSpawn;

    void FixedUpdate()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn>=timeBetweenSpawns)
        {
            _timeSinceLastSpawn-=timeBetweenSpawns;
            SpawnNucleon();
        }
        
    }

    private void SpawnNucleon()
    {
        Nucleon prefab = Instantiate(nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)], transform);

        prefab.transform.localPosition = Random.onUnitSphere * spawnDistance;


    }
}
