using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    [SerializeField] Camera cam1;
    [SerializeField] Camera cam2;
    [SerializeField] Camera cam3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Q))
        {
            cam1.enabled = true;
            cam2.enabled = false;
            cam3.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.W))
        {
            cam1.enabled = false;
            cam2.enabled = true;
            cam3.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.E))
        {
            cam1.enabled = false;
            cam2.enabled = false;
            cam3.enabled = true;
        }
    }
}
