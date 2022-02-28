using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class SpeedLimit : MonoBehaviour
    {
        [SerializeField][Range(20, 120)] int speedLimit;

        float realSpeed;

        private void Reset()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        private void Start()
        {
            realSpeed = SpeedSystemManager.Instance.KmToUnitSpeed(speedLimit);
        }

        private void OnTriggerEnter(Collider other)
        {
            VehicleBrain vehicle = other.GetComponent<VehicleBrain>();
            if (vehicle)
            {
                /*float min = vehicle.speedMinMax.x;*/
                float max = vehicle.speedMinMax.y;
                /*min = vehicle.speedMinMax.x > realSpeed ? realSpeed : vehicle.speedMinMax.x;*/

                max = vehicle.speedMinMax.y > realSpeed ? realSpeed : vehicle.speedMinMax.y;

                float ran = Random.Range(max-((max/100) * 10), max);

                /*vehicle.vehicleSpeed = ran;*/
                vehicle.initialSpeed = ran;
                //Debug.Log("Speed limit recorded");
            }
        }

        private void OnDrawGizmos()
        {
            //Just like the other 2 use cases, this only works if box is at angles that are increments of 90. Since it uses bounds, it would draw not accurately if turned in other directions.
            //Easy fix would be to do some matrix math and turn the coordinates based on obj rotation, but not needed for this example.

            Gizmos.color = Color.red * .35f;

            Gizmos.DrawCube(GetComponent<BoxCollider>().bounds.center, GetComponent<BoxCollider>().bounds.extents * 2);
        }
    }
}