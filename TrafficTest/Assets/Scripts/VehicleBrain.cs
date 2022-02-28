using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficSystem;

namespace TrafficSystem
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class VehicleBrain : MonoBehaviour
    {
        private const float ARRIVAL_SENS = .16f;

        public Node startingNode;

        private Node currentNode = null;
        private Node targetNode = null;
        Vector3 targetPos = Vector3.zero;
        public bool isComingFromOuterNode = false;

        public float vehicleSpeed = 2f;
        [Header("Start")]
        public bool randomizeSpeedOnStart = true;
        public Vector2 speedMinMax = new Vector2();
        public bool randomizeDirectionOnStart = true;
        public MovementDirection movementDirection;

        [HideInInspector] public bool isStopped = false;


        private Transform raycastPoint;
        [HideInInspector] public float initialSpeed;

        private void Reset()
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().useGravity = false;
            }

            if (transform.Find("raycast_point"))
            {
                raycastPoint = transform.Find("raycast_point");
            }
            else
            {
                GameObject obj = new GameObject("raycast_point");
                raycastPoint = obj.transform;
                raycastPoint.SetParent(this.transform);
                raycastPoint.localPosition = Vector3.zero;
            }
        }


        private void Start()
        {
            try
            {
                raycastPoint = transform.Find("raycast_point");

                transform.position = startingNode.transform.position;
                currentNode = startingNode;
                if (randomizeDirectionOnStart)
                {
                    movementDirection = (MovementDirection)Random.Range(0, 2);
                }
                if (randomizeSpeedOnStart)
                {
                    vehicleSpeed = Random.Range((float)speedMinMax.x, (float)speedMinMax.y);
                }
                initialSpeed = vehicleSpeed; ;
            }
            catch
            {
                //there's a slim chance this trows an error due to startingNode being null, however it is not a problem and we can just destroy this obj on spawn.
                Destroy(gameObject);
            }
            
        }

        private void Update()
        {
            if (targetNode == null)
            {
                RecalculateTarget();
            }

            Scan();

            Motion();

        }

        private void Scan()
        {
            //this can be done different ways, in this case, since we only have cars and large vehicles, raycasting is a good enough approach to get what is in front of us, but if we added 
            //motorcycles and bikes we should rather use overlapbox in front of the vehicle, but that is too expensive and unnecessary for what I am trying to achieve with this demo
            if (!isStopped)
            {
                Ray ray = new Ray(raycastPoint.position, raycastPoint.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1f))
                {
                    VehicleBrain brain = hit.collider.GetComponent<VehicleBrain>();
                    if (brain)
                    {
                        vehicleSpeed = brain.vehicleSpeed < vehicleSpeed ? brain.vehicleSpeed : brain.vehicleSpeed; // limit current vehicle speed to vehicle in front
                    }
                }
                else
                {
                    vehicleSpeed = initialSpeed;
                }
            }
            

        }

        private void Motion()
        {
            if (targetPos != Vector3.zero)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, vehicleSpeed * Time.deltaTime);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position).normalized, 6 * Time.deltaTime);

            if (DistanceSqr(transform.position, targetPos) <= ARRIVAL_SENS * ARRIVAL_SENS)
            {
                
                currentNode = targetNode;
                
                RecalculateTarget();
            }

        }

        private float DistanceSqr(Vector3 posA, Vector3 posB)
        {
            Vector3 heading;
            float distanceSqr;

            heading.x = posA.x - posB.x;
            heading.y = posA.y - posB.y;
            heading.z = posA.z - posB.z;

            distanceSqr = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;

            return distanceSqr;
        }


        private void RecalculateTarget()
        {
            try
            {
                
                
                if (movementDirection == MovementDirection.Forward)
                {
                    targetNode = currentNode.GetRandomNextNode(this, currentNode);
                    targetPos = targetNode.GetDestinationPoint(currentNode);
                }
                else
                {
                    targetNode = currentNode.GetRandomPreviousNode(this, currentNode);
                    targetPos = targetNode.GetDestinationPoint(currentNode);
                }

                if (currentNode.isTjunctionOuterNode)
                {
                    isComingFromOuterNode = true;
                }

                if (currentNode is TNode)
                {
                    isComingFromOuterNode = false;
                }



            }
            catch (System.Exception)
            {
                //Debug.Log($"Could not find a node past current node");
                //Debug.Log($"{movementDirection.ToString()}");
                //Debug.LogError(e.Message);
                Destroy(gameObject);
                return;
            }

        }

        private void OnDrawGizmosSelected()
        {
            if (targetPos != Vector3.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, targetPos);
            }

            if (raycastPoint)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(raycastPoint.position, raycastPoint.position + raycastPoint.forward);
            }
        }


        public enum MovementDirection
        {
            Forward,
            Backward
        }
    }

}