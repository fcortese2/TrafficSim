using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficSystem;
using UnityEditor;

public class NodeWindow : EditorWindow
{
    [MenuItem("Tools/Node Editor")]
    public static void Open()
    {
        GetWindow<NodeWindow>();
    }

    public Transform nodeRootObject;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("nodeRootObject"));

        if (nodeRootObject)
        {
            DrawBtns();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawBtns()
    {
        if (GUILayout.Button("Create New Node"))
        {
            CreateNewNode();
        }
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Node>() && Selection.activeGameObject.transform.parent == nodeRootObject)
        {
            if (GUILayout.Button("T-Node"))
            {
                CreateNewTNode();
            }
            if (GUILayout.Button("New Branch"))
            {
                CreateNewBranch();
            }
        }
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Node>())
        {
            if (GUILayout.Button("Delete Node"))
            {
                DeleteNode();
            }
        }
    }

    private void CreateNewNode()
    {
        GameObject nodeObj = new GameObject($"Node {nodeRootObject.childCount}", typeof(Node));
        nodeObj.transform.SetParent(nodeRootObject);

        Node newNode = nodeObj.GetComponent<Node>();
        if (nodeRootObject.childCount > 1)
        {
            newNode.AddPreviousNode(nodeRootObject.GetChild(nodeRootObject.childCount - 2).GetComponent<Node>());
            nodeRootObject.GetChild(nodeRootObject.childCount - 2).GetComponent<Node>().AddFollowingNode(newNode);

            nodeObj.transform.localPosition = newNode.PreviousNodes[0].transform.localPosition + newNode.PreviousNodes[0].transform.forward;
            nodeObj.transform.rotation = newNode.PreviousNodes[0].transform.rotation;
        }

        Selection.activeGameObject = nodeObj;
    }

    private void CreateNewTNode()
    {
        GameObject nodeObj = new GameObject($"Node {nodeRootObject.childCount}", typeof(TNode));
        nodeObj.transform.SetParent(nodeRootObject);

        TNode newNode = nodeObj.GetComponent<TNode>();

        newNode.AddPreviousNode(Selection.activeGameObject.GetComponent<Node>());

        GameObject branchObj = new GameObject($"Node {nodeRootObject.childCount}", typeof(Node));
        branchObj.transform.SetParent(nodeRootObject);
        Node branchNode = branchObj.GetComponent<Node>();
        branchNode.AddPreviousNode(newNode);
        branchNode.isTjunctionOuterNode = true;

        newNode.OutgoingNode = branchNode;

        if (Selection.activeGameObject.GetComponent<Node>().NextNodes.Count > 0)
        {
            newNode.AddFollowingNode(Selection.activeGameObject.GetComponent<Node>().NextNodes[0]);
            Selection.activeGameObject.GetComponent<Node>().NextNodes[0].PreviousNodes.Remove(Selection.activeGameObject.GetComponent<Node>());
            Selection.activeGameObject.GetComponent<Node>().NextNodes[0].AddPreviousNode(newNode);
            Selection.activeGameObject.GetComponent<Node>().NextNodes.RemoveAt(0);
            Selection.activeGameObject.GetComponent<Node>().AddFollowingNode(newNode);
        }

        newNode.transform.localPosition = newNode.PreviousNodes[0].transform.localPosition + newNode.PreviousNodes[0].transform.forward/2f;
        newNode.transform.rotation = newNode.PreviousNodes[0].transform.rotation;

        branchNode.transform.position = newNode.transform.localPosition + newNode.transform.forward / 4f;
        branchNode.transform.rotation = newNode.transform.rotation;
    }

    private void CreateNewBranch()
    {
        GameObject nodeObj = new GameObject($"Node {nodeRootObject.childCount}", typeof(Node));
        nodeObj.transform.SetParent(nodeRootObject);

        Node newNode = nodeObj.GetComponent<Node>();
        Node originNode = Selection.activeGameObject.GetComponent<Node>();

        newNode.transform.localPosition = originNode.transform.localPosition + originNode.transform.forward;
        newNode.transform.rotation = originNode.transform.rotation;

        originNode.AddFollowingNode(newNode);
        newNode.AddPreviousNode(originNode);

        Selection.activeGameObject = nodeObj;
    }

    private void DeleteNode()
    {
        Node selectedNode = Selection.activeGameObject.GetComponent<Node>();

        if (selectedNode.NextNodes.Count > 0)
        {
            foreach (Node _node in selectedNode.NextNodes)
            {
                _node.PreviousNodes.Remove(selectedNode);
                foreach (Node pN in selectedNode.PreviousNodes)
                {
                    if (!_node.PreviousNodes.Contains(pN))
                    {
                        _node.PreviousNodes.Add(pN);
                    }
                }
            }
        }

        if (selectedNode.PreviousNodes.Count > 0)
        {
            foreach (Node _node in selectedNode.PreviousNodes)
            {
                _node.NextNodes.Remove(selectedNode);
                foreach (Node nN in selectedNode.NextNodes)
                {
                    if (!_node.NextNodes.Contains(nN))
                    {
                        _node.NextNodes.Add(nN);
                    }
                }
            }
        }
    }

}
