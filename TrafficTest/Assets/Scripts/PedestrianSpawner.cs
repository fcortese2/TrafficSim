using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TrafficSystem
{

    public class PedestrianSpawner : MonoBehaviour
    {
        [SerializeField] GameObject pedestrianPrefab;

        [HideInInspector] public List<Crosswalk> crosswalks = new List<Crosswalk>();

        public static PedestrianSpawner Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else Destroy(this);
        }

        private void Start()
        {
            StartCoroutine(SpawnAtRandomInterval(2f, 4f));
        }

        IEnumerator SpawnAtRandomInterval(float min, float max)
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(min, max));
                Crosswalk selCrosswalk = crosswalks[Random.Range(0, crosswalks.Count)];
                Pedestrian newPedestrian = Instantiate(pedestrianPrefab).GetComponent<Pedestrian>();
                newPedestrian.walkSpeed = .45f;
                Tuple<Vector3, Vector3> result = selCrosswalk.GetRandomStartingPoint;
                newPedestrian.startPoint = result.Item1;
                newPedestrian.endPoint = result.Item2;
                newPedestrian.transform.position = newPedestrian.startPoint;
                selCrosswalk.EnqueuePedestrian(newPedestrian);
            }
        }

    }

}