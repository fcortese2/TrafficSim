using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficSystem;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    [SerializeField] Node[] spawnNodes;

    private void Start()
    {
        StartCoroutine(SpawnAtRandom());
    }

    IEnumerator SpawnAtRandom()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(.8f, 1.2f));
            int idx = Random.Range(0, prefabs.Length);
            GameObject obj = Instantiate(prefabs[idx]);
            prefabs[idx].GetComponent<VehicleBrain>().startingNode = spawnNodes[Random.Range(0, spawnNodes.Length)];
        }

    }
}
