using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TrafficSystem
{
    
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class T_Junction : MonoBehaviour
    {
        Queue<VehicleBrain> queue = new Queue<VehicleBrain>();

        [SerializeField] private float stopWaitTime = 2.75f;
        float tElapsed = 0f;

        private void Reset()
        {
            if (GetComponent<Rigidbody>())
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            if (GetComponent<BoxCollider>())
            {
                GetComponent<BoxCollider>().isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<VehicleBrain>())
            {
                //Debug.Log("Vehicle entered");
                other.GetComponent<VehicleBrain>().isStopped = true;
                other.GetComponent<VehicleBrain>().vehicleSpeed = 0f;
                queue.Enqueue(other.GetComponent<VehicleBrain>());
            }
        }

        private void Update()
        {
            if (tElapsed > stopWaitTime)
            {
                if (queue.Count > 0)
                {
                    queue.Peek().vehicleSpeed = Random.Range(queue.Peek().speedMinMax.x, queue.Peek().speedMinMax.y);
                    queue.Peek().isStopped = false;
                    queue.Dequeue();
                }
                tElapsed = 0;
            }
            else tElapsed += Time.deltaTime;
            
        }


        private void OnDrawGizmos()
        {
            //This only works if box is at angles that are increments of 90. Since it uses bounds, it would draw not accurately if turned in other directions.
            //Easy fix would be to do some matrix math and turn the coordinates based on obj rotation, but not needed for this example.

            Gizmos.color = Color.yellow * .35f;

            Gizmos.DrawCube(GetComponent<BoxCollider>().bounds.center, GetComponent<BoxCollider>().bounds.extents * 2);
        }
    }
}