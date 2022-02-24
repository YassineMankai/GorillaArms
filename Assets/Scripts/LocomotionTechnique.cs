using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script. 
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    [Range(0, 10)] public float translationGain = 0.5f;
    public GameObject hmd;
    [SerializeField] private float leftTriggerValue;    
    [SerializeField] private float rightTriggerValue;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isIndexTriggerDown;
    public Transform Centereye;
    public GroundChecker MygroundChecker;


    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public PlaneScript plane;
    public bool isMoving;
    Vector3 currentStartVelocity;
    Vector3 currentStartPosition;
    Vector3 g;
    float movementTimePassed;

    void Start()
    {
        isMoving = false;
        g = new Vector3(0, -9.81f, 0);
    }

    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.
        if (isMoving)
        {
            movementTimePassed += Time.deltaTime;
            if (transform.position.y - MygroundChecker.checkGround() <= MygroundChecker.Offset)
            {
                isMoving = false;
                plane.collided = false;
            }
            else
            {
                transform.position = 0.5f * g * movementTimePassed * movementTimePassed + currentStartVelocity * movementTimePassed + currentStartPosition + (MygroundChecker.checkGround() + MygroundChecker.Offset) * Vector3.up;
            }
        }


        float forwardPower = -3 * Vector3.Dot(plane.CollisionVelocity, Centereye.forward);
        float UpPower = Vector3.Dot(plane.CollisionVelocity, Centereye.up);
        float rightPower = plane.isRightArm ? 0.2f : -0.2f;

        if (plane.collided && !isMoving
            && forwardPower > 0
            && UpPower > 0
            )
        {
            isMoving = true;

            forwardPower = Mathf.Min(forwardPower, 8);
            UpPower = Mathf.Min(forwardPower, 5);

            if (OVRInput.Get(OVRInput.Button.Three))   // X
            {
                forwardPower = 18;
                UpPower = 4f;
            } else if (OVRInput.Get(OVRInput.Button.One)) // A
            {
                forwardPower = 7;
                UpPower = 10;
            }

            currentStartPosition = transform.position;
            currentStartPosition.y = 0;

            transform.position = new Vector3(transform.position.x, MygroundChecker.checkGround() + MygroundChecker.Offset + 0.001f, transform.position.z);

            currentStartVelocity = forwardPower * Centereye.forward + UpPower * Centereye.up + rightPower * Centereye.right;

            movementTimePassed = 0;
        }

        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                this.transform.position = parkourCounter.currentRespawnPos;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        // These are for the game mechanism.
        if (other.CompareTag("banner"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
        // These are for the game mechanism.

    }
}