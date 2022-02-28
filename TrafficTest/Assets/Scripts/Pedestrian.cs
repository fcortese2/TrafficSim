using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TrafficSystem
{
    public class Pedestrian : MonoBehaviour
    {
        public Vector3 startPoint;
        public Vector3 endPoint;

        [HideInInspector] public float walkSpeed;

        public UnityEvent<Pedestrian> OnFinishedCrossing = new UnityEvent<Pedestrian>();


        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint, walkSpeed * Time.deltaTime);

            transform.LookAt(endPoint);

            if ((endPoint - transform.position).sqrMagnitude < .1f)
            {
                OnFinishedCrossing.Invoke(this);
            }
        }
    }
}