using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public Transform OVRCamera;
    public float Offset;
    // Start is called before the first frame update


    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            Offset = OVRCamera.position.y - hit.point.y;
        }
    }

    public float checkGround()
    {
        float res = 0;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            res = hit.point.y;
        }
        return res;
    }

    private void Update()
    {
        Vector3 newPos = OVRCamera.position;
        newPos.y = 55;
        transform.position = newPos;
    }
}
