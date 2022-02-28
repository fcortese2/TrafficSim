using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem
{
    public class TNode : Node
    {
        public Node OutgoingNode = null;

        private void Start()
        {
            if (OutgoingNode == null)
            {
                throw new System.Exception($"TNode {gameObject.name} has to outgoing node!");
            }
        }

        public override Node GetRandomNextNode(VehicleBrain brain, Node origin)
        {
            //if going forward
            if (brain.movementDirection == VehicleBrain.MovementDirection.Forward && OutgoingNode != origin) //if coming from motion direction, normal branch...
            {
                if (Random.Range(0, 2) == 0)
                {
                    return NextNodes[0];
                }
                else return OutgoingNode;
            }
            else if (OutgoingNode == origin)  //if coming from outgoing node
            {
                int LR = Random.Range(0, 2);
                if (LR == 0) //if go left
                {
                    //maintain same 'drection'
                    return PreviousNodes[0];
                }
                else //go right
                {
                    switch (brain.movementDirection)
                    {
                        case VehicleBrain.MovementDirection.Forward:
                            brain.movementDirection = VehicleBrain.MovementDirection.Backward;
                            break;
                        case VehicleBrain.MovementDirection.Backward:
                            brain.movementDirection = VehicleBrain.MovementDirection.Forward;
                            break;
                        default:
                            break;
                    }
                    return NextNodes[0];
                    //flip 'direction'
                }
            }
            else
            {
                throw new System.Exception($"Trying to get node ahead for vehicle going against motion");
            }
        }

        public override Node GetRandomPreviousNode(VehicleBrain brain, Node origin)
        {
            if (origin != OutgoingNode)
            {

                int LB = Random.Range(0, 2);
                if (LB == 0) // if going same direction in loop
                {
                    //mantain same 'direction'
                    return PreviousNodes[0];
                }
                else
                {
                    switch (brain.movementDirection)
                    {
                        case VehicleBrain.MovementDirection.Forward:
                            brain.movementDirection = VehicleBrain.MovementDirection.Backward;
                            break;
                        case VehicleBrain.MovementDirection.Backward:
                            brain.movementDirection = VehicleBrain.MovementDirection.Forward;
                            break;
                        default:
                            break;
                    }

                    if (!brain.isComingFromOuterNode)
                    {
                        return OutgoingNode;
                    }
                    else
                    {
                        brain.isComingFromOuterNode = false;
                        return NextNodes[0];
                    }
                    
                    
                }
            }
            else
            {
                return null;   
            }
            

        }

        public override Vector3 GetDestinationPoint(Node from)
        {
            if (!PreviousNodes.Contains(from) && !NextNodes.Contains(from) && !OutgoingNode == from)
            {
                throw new System.Exception("Coming from a non-registered node at TNODE");
            }
            Vector3 point;
            if (PreviousNodes.Contains(from))
            {
                //return left lane point
                point = transform.position - (transform.right * roadWidth / 4f);
                return point;
            }
            else
            {
                //return right lane point
                point = transform.position + (transform.right * roadWidth / 4f);
                return point;
            }
        }
    }
}