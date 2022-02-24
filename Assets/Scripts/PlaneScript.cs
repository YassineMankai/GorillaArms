using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform Centereye;
    public bool collided;
    public Vector3 CollisionVelocity;
    public bool isRightArm;

    void Start()
    {
        collided = false;
        CollisionVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Centereye.position.x, Centereye.position.y - 0.6f, Centereye.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GorillaArmsLeft" || other.tag == "GorillaArmsRight")
        {
            collided = true;
            
            CollisionVelocity = other.gameObject.GetComponent<VelocityChecker>().velocity;

            isRightArm = other.tag == "GorillaArmsRight";
        }
    }
}
