using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private List<Node> previousNodes = new List<Node>();
        [SerializeField] private List<Node> nextNodes = new List<Node>();

        public List<Node> PreviousNodes { get { return previousNodes; } }
        public List<Node> NextNodes { get { return nextNodes; } }

        public bool isTjunctionOuterNode = false;

        public float roadWidth = 1f;

        public void SetRoadWidth(float width)
        {
            roadWidth = width;
        }

        public void AddFollowingNode(Node node)
        {
            nextNodes.Add(node);
        }

        public void AddPreviousNode(Node node)
        {
            previousNodes.Add(node);
        }

        public virtual Node GetRandomNextNode(VehicleBrain brain, Node origin)
        {
            return nextNodes[Random.Range(0, nextNodes.Count)];
        }

        public virtual Node GetRandomPreviousNode(VehicleBrain brain, Node origin)
        {
            return previousNodes[Random.Range(0, previousNodes.Count)];
        }

        #region Motion Logic
        ///Long story short, we can say that if traffic is lef-sided, whenever the vehicle comes from a previous node, we always return a point on the left lane,
        ///whilst if the vehicle is coming from a 'following' node, we have to always return the right lane. In right-hand traffic, this is the opposite. 
        
        public virtual Vector3 GetDestinationPoint(Node from)
        {
            if (!previousNodes.Contains(from) && !nextNodes.Contains(from))
            {
                throw new System.Exception("Coming from a non-registered node");
            }
            Vector3 point;
            if (previousNodes.Contains(from))
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
        #endregion

    }


}