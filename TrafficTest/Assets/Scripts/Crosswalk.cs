using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TrafficSystem
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class Crosswalk : MonoBehaviour
    {

        Queue<VehicleBrain> vehicleQueue = new Queue<VehicleBrain>();
        Queue<Pedestrian> pedestrianQueue = new Queue<Pedestrian>();

        Transform pointA;
        Transform pointB;
        public Tuple<Vector3, Vector3> GetRandomStartingPoint
        {
            get
            {
                if (Random.Range(0,101) < 50)
                {
                    return new Tuple<Vector3, Vector3>(pointA.position, pointB.position);
                }
                else
                {
                    return new Tuple<Vector3, Vector3>(pointB.position, pointA.position);
                }
            }
        }

        private void Reset()
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().useGravity = false;
            }

            if (GetComponent<BoxCollider>())
            {
                GetComponent<BoxCollider>().isTrigger = true;
            }

            if (!transform.Find("PointA"))
            {
                pointA = new GameObject("PointA").transform;
                pointA.transform.SetParent(this.transform);
                pointA.transform.localPosition = Vector3.zero;
            }
            else pointA = transform.Find("PointA");

            if (!transform.Find("PointB"))
            {
                pointB = new GameObject("PointB").transform;
                pointB.transform.SetParent(this.transform);
                pointB.transform.localPosition = Vector3.zero;
            }
            else pointB = transform.Find("PointB");

        }

        private void Awake()
        {
            try
            {
            pointA = transform.Find("PointA");
            pointB = transform.Find("PointB");

            }
            catch
            {
                Debug.LogError($"No PointA or PointB in {gameObject.name}");
            }
        }

        private void Start()
        {
            PedestrianSpawner.Instance.crosswalks.Add(this);
        }

        private void Update()
        {
            if (pedestrianQueue.Count == 0 && vehicleQueue.Count > 0)
            {
                int vehiclesWaiting = vehicleQueue.Count;
                for (int i = 0; i < vehiclesWaiting; i++)
                {
                    vehicleQueue.Peek().isStopped = false;
                    vehicleQueue.Peek().vehicleSpeed = Random.Range(vehicleQueue.Peek().speedMinMax.x, vehicleQueue.Peek().speedMinMax.y);
                    vehicleQueue.Dequeue();
                }
            }
        }


        public void EnqueuePedestrian(Pedestrian pedestrian)
        {
            pedestrianQueue.Enqueue(pedestrian);
            //I'm gonna use a listener to listen for when a pedestrian is done crossing, but we could also do it with OnTriggerExit.
            //I just think this is a more reliable way of doing it, especially for scalability reasons.
            pedestrian.OnFinishedCrossing.AddListener(DequeuePedestrian);
        }

        //this is technically quite slow because it scales linearly with number of pedestrians crossing, but in th is case is more than fine.
        //We are not trying to recreate the Shibuya crossing, so this should run fast without the need of using search-find-remove methods.
        private void DequeuePedestrian(Pedestrian pedestrian)
        {
            Debug.Log("Dequeue pedestrian");
            pedestrianQueue = new Queue<Pedestrian>(pedestrianQueue.Where(x => x != pedestrian));
            Destroy(pedestrian.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            VehicleBrain vehicle = other.GetComponent<VehicleBrain>();
            if (vehicle)
            {
                vehicleQueue.Enqueue(vehicle);
                vehicle.isStopped = true;
                vehicle.vehicleSpeed = 0;
            }
        }


        private void OnDrawGizmos()
        {
            //Just like in T_Junction.cs, this only works if box is at angles that are increments of 90. Since it uses bounds, it would draw not accurately if turned in other directions.
            //Easy fix would be to do some matrix math and turn the coordinates of the gizmo based on obj rotation, but not needed for this example.

            Gizmos.color = Color.magenta * .35f;

            Gizmos.DrawCube(GetComponent<BoxCollider>().bounds.center, GetComponent<BoxCollider>().bounds.extents * 2);
        }
    }
}