using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovementArticulationBody : MonoBehaviour
{

    /// <summary>
    /// determines how fast the plane will roll
    /// </summary>
    [Range(0f, 100f)]
    public float rollSpeed;

    /// <summary>
    /// determines how fast the plane will pitch
    /// </summary>
    [Range(0f, 100f)]
    public float pitchSpeed;

    /// <summary>
    /// determines at what angle the plane will stay at constant height
    /// </summary>
    [Range(-90f, 90f)]
    public float AngOfAttack;

    /// <summary>
    /// how much throttle
    /// </summary>
    [Range(-1, 1)]
    public float throttle;

    /// <summary>
    /// how quickly the throttle will change
    /// </summary>
    [Range(0f, 1f)]
    public float throttleChange;

    /// <summary>
    /// how fast the throttle makes the plane move
    /// </summary>
    [Range(0f, 350f)]
    public float throttleSpeed;

    /// <summary>
    /// at what speed the refueller is moving (makes the C5 move forward or backward relative to it)
    /// </summary>
    [Range(0f, 10f)]
    public float refuellerSpeed;

    /// <summary>
    /// determines how quickly the plane rises and falls
    /// </summary>
    [Range(0f, 100f)]
    public float lift;

    public ArticulationBody plane;

    /// <summary>
    /// controls how quickly the plane will yaw
    /// </summary>
    [Range(0f, 100f)]
    public float yawSpeed;

    /// <summary>
    /// how much the roll of the plane will move the plane left/right
    /// </summary>
    [Range(0f, 100f)]
    public float LRSpeed;

    /// <summary>
    /// how much flaps rotate in flight
    /// </summary>
    public float flapAmount;

    /// <summary>
    /// list of all left flaps
    /// </summary>
    public List<GameObject> flapsLeft;

    /// <summary>
    /// list of all right flaps
    /// </summary>
    public List<GameObject> flapsRight;

    // move params
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode yawL;
    public KeyCode yawR;
    public KeyCode throttleUp;
    public KeyCode throttleDown;

    void Start()
    {
        plane = GetComponent<ArticulationBody>();
    }

    void Update()
    {
        // creating blank torque force
        Vector3 torq = new(0, 0, 0);

        // adding roll to torque vector
        if (Input.GetKey(left))
            torq += 0.01f * rollSpeed * Time.deltaTime * transform.forward;
        if (Input.GetKey(right))
            torq -= 0.01f * rollSpeed * Time.deltaTime * transform.forward;

        // adding pitch to torque vector
        if (Input.GetKey(forward))
            torq += 0.01f * pitchSpeed * Time.deltaTime * transform.right;
        if (Input.GetKey(backward))
            torq -= 0.01f * pitchSpeed * Time.deltaTime * transform.right;

        // adding yaw to torque vector
        if (Input.GetKey(yawL))
            torq -= 0.01f * Time.deltaTime * yawSpeed * transform.up;
        if (Input.GetKey(yawR))
            torq += 0.01f * Time.deltaTime * yawSpeed * transform.up;

        // setting throttle amount
        if (Input.GetKey(throttleUp))
            throttle = Mathf.Clamp(throttle + (throttleChange * Time.deltaTime), -1, 1);
        if (Input.GetKey(throttleDown))
            throttle = Mathf.Clamp(throttle - (throttleChange * Time.deltaTime), -1, 1);


        // getting angle of the plane
        Vector3 ang = transform.rotation.eulerAngles;
        // how much the plane will rise or fall
        float deltaHeight;
        // how much plane will move left/right based on roll
        float deltaLR;
        // converting from 0->360 to -180->180
        // determining how much the aircraft should move up/down left/right based on rotation
        if (ang.x > 180)
            deltaHeight = (-(ang.x - 360) - AngOfAttack) * lift * Time.deltaTime * 0.01f;
        else
            deltaHeight = (-ang.x - AngOfAttack) * lift * Time.deltaTime * 0.01f;
        if (ang.z > 180)
            deltaLR = (ang.z - 360) * LRSpeed * Time.deltaTime * 0.01f;
        else
            deltaLR = ang.z * LRSpeed * Time.deltaTime * 0.01f;

        // set velocity of plane based on change in up/down/left/right calculated above
        plane.velocity = (new Vector3(transform.forward.x, 0, transform.forward.z) * throttle) + new Vector3(0, deltaHeight, 0) + (new Vector3(-transform.right.x, 0, -transform.right.z) * deltaLR);
        // set angular velocity of plane based on torque
        plane.angularVelocity = torq;
    }

    // fun script to make flaps move on C5
    void AdjustFlaps(string s)
    {
        if (s == "left")
        {
            foreach (GameObject flap in flapsLeft)
                flap.transform.eulerAngles = -Vector3.right * flapAmount;
            foreach (GameObject flap in flapsRight)
                flap.transform.eulerAngles = Vector3.right * flapAmount;
        }
        else if (s == "right")
        {
            foreach (GameObject flap in flapsLeft)
                flap.transform.eulerAngles = Vector3.right * flapAmount;
            foreach (GameObject flap in flapsRight)
                flap.transform.eulerAngles = -Vector3.right * flapAmount;
        }
        else if (s == "up")
        {
            foreach (GameObject flap in flapsLeft)
                flap.transform.eulerAngles = Vector3.right * flapAmount;
            foreach (GameObject flap in flapsRight)
                flap.transform.eulerAngles = Vector3.right * flapAmount;
        }
        else if (s == "down")
        {
            foreach (GameObject flap in flapsLeft)
                flap.transform.eulerAngles = -Vector3.right * flapAmount;
            foreach (GameObject flap in flapsRight)
                flap.transform.eulerAngles = -Vector3.right * flapAmount;
        }
        else if (s == "flat")
        {
            foreach (GameObject flap in flapsLeft)
                flap.transform.eulerAngles = Vector3.zero;
            foreach (GameObject flap in flapsRight)
                flap.transform.eulerAngles = Vector3.zero;
        }
    }

}
