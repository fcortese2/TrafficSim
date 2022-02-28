using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TrafficSystem;

[InitializeOnLoad()]
public class NodeEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Node node, GizmoType gizmoType)
    {

        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.cyan;//left lane color
            if (node.PreviousNodes.Count > 0)
            {
                Vector3 lane = node.GetDestinationPoint(node.PreviousNodes[0]);
                Gizmos.DrawSphere(lane, .08f); ;
                Gizmos.DrawLine(lane, lane + node.transform.forward * .4f);
            }
            Gizmos.color = Color.grey; //right lane color
            if (node.NextNodes.Count > 0)
            {
                Vector3 lane = node.GetDestinationPoint(node.NextNodes[0]);
                Gizmos.DrawSphere(lane, .08f);
                Gizmos.DrawLine(lane, lane - node.transform.forward * .4f);
            }

            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawSphere(node.transform.position, .15f);
        Gizmos.DrawLine(node.transform.position, node.transform.position + node.transform.forward * .6f);

        Gizmos.color = Color.white * .8f;
        Vector3 nodeRightBound = node.transform.position + (node.transform.right * node.roadWidth / 2f);
        Vector3 nodeLeftBound = node.transform.position - (node.transform.right * node.roadWidth / 2f);
        Gizmos.DrawLine(nodeRightBound, nodeLeftBound);

        if (node.PreviousNodes.Count > 0)
        {
            foreach (Node _node in node.PreviousNodes)
            {
                Gizmos.color = Color.yellow;
                Vector3 offsetFrom = node.transform.position + node.transform.right * (node.roadWidth / 2f);
                Vector3 offsetTo = _node.transform.position + _node.transform.right * (_node.roadWidth / 2f);

                Gizmos.DrawLine(offsetFrom, offsetTo);
            }
        }
        if (node.NextNodes.Count > 0)
        {
            foreach (Node _node in node.NextNodes)
            {
                Gizmos.color = Color.blue;
                Vector3 offsetFrom = node.transform.position + node.transform.right * (-node.roadWidth / 2f);
                Vector3 offsetTo = _node.transform.position + _node.transform.right * (-_node.roadWidth / 2f);

                Gizmos.DrawLine(offsetFrom, offsetTo);
            }
        }

        if (node is TNode)
        {
            Gizmos.color = Color.blue;
            Vector3 offsetFrom = node.transform.position + node.transform.right * (-node.roadWidth / 2f);
            Vector3 offsetTo = ((TNode)node).OutgoingNode.transform.position + ((TNode)node).OutgoingNode.transform.right * (-((TNode)node).OutgoingNode.roadWidth / 2f);
            Gizmos.DrawLine(offsetFrom, offsetTo);
        }


    }
}
