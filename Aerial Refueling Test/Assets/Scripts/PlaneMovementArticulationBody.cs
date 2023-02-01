using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneMovementArticulationBody : MonoBehaviour
{
    /// <summary>
    /// determines how fast the plane will roll
    /// </summary>
    [Range(0f, 1000f)]
    public float rollSpeed;

    /// <summary>
    /// determines how fast the plane will pitch
    /// </summary>
    [Range(0f, 1000f)]
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
    [HideInInspector]
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
    [Range(0f, 100f)]
    public float refuellerSpeed;

    /// <summary>
    /// determines how quickly the plane rises and falls
    /// </summary>
    [Range(0f, 1000f)]
    public float lift;

    /// <summary>
    /// pointer to the articulationbody this script is affecting
    /// </summary>
    [HideInInspector]
    public ArticulationBody plane;

    /// <summary>
    /// controls how quickly the plane will yaw
    /// </summary>
    [Range(0f, 1000f)]
    public float yawSpeed;

    /// <summary>
    /// how much the roll of the plane will move the plane left/right
    /// </summary>
    [Range(0f, 1000f)]
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

    /// <summary>
    /// key to pitch the plane forward
    /// </summary>
    [HideInInspector]
    public KeyCode forward;
    /// <summary>
    /// key to pitch the plane back
    /// </summary>
    [HideInInspector]
    public KeyCode backward;
    /// <summary>
    /// key to roll the plane left
    /// </summary>
    [HideInInspector]
    public KeyCode left;
    /// <summary>
    /// key to roll the plane right
    /// </summary>
    [HideInInspector]
    public KeyCode right;
    /// <summary>
    /// key to yaw the plane left
    /// </summary>
    [HideInInspector]
    public KeyCode yawL;
    /// <summary>
    /// key to yaw the plane right
    /// </summary>
    [HideInInspector]
    public KeyCode yawR;
    /// <summary>
    /// key to increase the throttle
    /// </summary>
    [HideInInspector]
    public KeyCode throttleUp;
    /// <summary>
    /// key to increase the throttle
    /// </summary>
    [HideInInspector]
    public KeyCode throttleDown;

    /// <summary>
    /// target position of KC135 for auto-pilot
    /// </summary>
    [HideInInspector]
    public Vector3 targetPos;
    /// <summary>
    /// maximum roll amount for KC135
    /// </summary>
    public float rollMax;
    /// <summary>
    /// maximum pitch amount for KC135
    /// </summary>
    public float pitchMax;

    /// <summary>
    /// if true, control C5 with keyboard
    /// </summary>
    [HideInInspector]
    public bool keyboardMode;
    /// <summary>
    /// if true, control C5 with flight stick
    /// </summary>
    [HideInInspector]
    public bool joystickMode;
    /// <summary>
    /// if true, control C5 with PS4 controller
    /// </summary>
    [HideInInspector]
    public bool controllerMode;
    /// <summary>
    /// if KC135 is player-controlled
    /// </summary>
    [HideInInspector]
    public bool playerKC135;

    /// <summary>
    /// playerInput object for KC135 control
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// X/Y movement from joystick
    /// </summary>
    [HideInInspector]
    private InputAction moveAction;
    /// <summary>
    /// throttle action from joystick
    /// </summary>
    private InputAction thrustAction;
    /// <summary>
    /// yaw action from joystick
    /// </summary>
    private InputAction yawAction;

    /// <summary>
    /// X/Y movement from PS4 controller
    /// </summary>
    private InputAction moveActionController;
    /// <summary>
    /// X/Y movement from PS4 controller
    /// </summary>
    private InputAction thrustActionController;
    /// <summary>
    /// X/Y movement from PS4 controller
    /// </summary>
    private InputAction yawActionController;

    /// <summary>
    /// if true, auto-target C5 movement
    /// </summary>
    [HideInInspector]
    public bool targetMode;

    /// <summary>
    /// converts an int into a bool (used in player preference handling)
    /// </summary>
    /// <param name="val">value to convert</param>
    /// <returns></returns>
    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

    // called upon script startup
    void Start()
    {
        // setup pointers
        plane = GetComponent<ArticulationBody>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move2"];
        thrustAction = playerInput.actions["Thrust2"];
        yawAction = playerInput.actions["Yaw2"];

        moveActionController = playerInput.actions["Move1"];
        thrustActionController = playerInput.actions["Thrust1"];
        yawActionController = playerInput.actions["Yaw1"];


        keyboardMode = intToBool(PlayerPrefs.GetInt("inputKeyboardKC135"));
        joystickMode = intToBool(PlayerPrefs.GetInt("inputJoystickKC135"));
        controllerMode = intToBool(PlayerPrefs.GetInt("inputControllerKC135"));

        playerKC135 = intToBool(PlayerPrefs.GetInt("playerKC135"));

        if (!playerKC135)
        {
            targetMode = true;
        }
        // if playerKC135 and no input mode is set, set keyboard mode
        else if (playerKC135 && !keyboardMode && !joystickMode && !controllerMode)
        {
            keyboardMode = true;
        }
    }

    void Update()
    {
        // creating blank torque force
        Vector3 torq = new(0, 0, 0);

        // autopilot
        if (targetMode)
        {
            // calculate change in position and rotation needed
            Vector3 deltaPos = targetPos - transform.localPosition;
            Vector3 adjustedRot = transform.localEulerAngles;

            // convert from 0->360 to -180->180
            if (adjustedRot.x > 180)
                adjustedRot.x -= 360;
            if (adjustedRot.y > 180)
                adjustedRot.y -= 360;
            if (adjustedRot.z > 180)
                adjustedRot.z -= 360;

            // autoflight for roll
            float xdiff = Vector3.Dot(deltaPos, new Vector3(transform.right.x, 0, transform.right.z));

            // make bigger adjustments if roll is very difference
            if (Mathf.Abs(xdiff) > 2)
            {
                // left
                if (xdiff < 0 && adjustedRot.z < rollMax)
                    torq += 0.01f * rollSpeed * Time.deltaTime * transform.forward;
                // right
                else if (xdiff > 0 && adjustedRot.z > -rollMax)
                    torq -= 0.01f * rollSpeed * Time.deltaTime * transform.forward;
            }
            // make smaller adjustments with smaller difference
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }

            // autoflight for pitch
            float ydiff = deltaPos.y;
            if (Mathf.Abs(ydiff) > 2)
            {
                // down
                if (ydiff < 0 && adjustedRot.x < pitchMax - AngOfAttack)
                    torq += 0.01f * pitchSpeed * Time.deltaTime * transform.right;
                // up
                else if (ydiff > 0 && adjustedRot.x > -pitchMax - AngOfAttack)
                    torq -= 0.01f * pitchSpeed * Time.deltaTime * transform.right;
            }
            else
            {
                transform.eulerAngles = new Vector3(-AngOfAttack, transform.eulerAngles.y, transform.eulerAngles.z);
            }

            // autoflight for throttle
            // calculate z diff between two aircraft
            float zdiff = Vector3.Dot(deltaPos, new Vector3(transform.forward.x, 0, transform.forward.z));
            // make large adjustments at large difference
            if (Mathf.Abs(zdiff) > 5)
            {
                // increase throttle
                if (zdiff > 0)
                    throttle = Mathf.Clamp(throttle + (throttleChange * Time.deltaTime), -1, 1);

                // decrease throttle
                else if (zdiff < 0)
                    throttle = Mathf.Clamp(throttle - (throttleChange * Time.deltaTime), -1, 1);
            }
            // make smaller adjustments at small difference
            else
            {
                throttle = 0;
            }
        }
        
        // player-controlled
        else
        {
            // control with keyboard
            if (keyboardMode)
            {
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
            }
            else if (joystickMode)
            {
                // adding roll to torque vector
                if (moveAction.ReadValue<Vector2>().x < 0)
                    torq += 0.01f * rollSpeed * Time.deltaTime * transform.forward;
                if (moveAction.ReadValue<Vector2>().x > 0)
                    torq -= 0.01f * rollSpeed * Time.deltaTime * transform.forward;

                // adding pitch to torque vector
                if (moveAction.ReadValue<Vector2>().y > 0)
                    torq += 0.01f * pitchSpeed * Time.deltaTime * transform.right;
                if (moveAction.ReadValue<Vector2>().y < 0)
                    torq -= 0.01f * pitchSpeed * Time.deltaTime * transform.right;

                // adding yaw to torque vector
                if (yawAction.ReadValue<float>() < 0)
                    torq -= 0.01f * Time.deltaTime * yawSpeed * transform.up;
                if (yawAction.ReadValue<float>() > 0)
                    torq += 0.01f * Time.deltaTime * yawSpeed * transform.up;

                // setting throttle amount
                if (thrustAction.ReadValue<float>() > 0)
                    throttle = Mathf.Clamp(throttle + (thrustAction.ReadValue<float>() * throttleChange * Time.deltaTime), -1, 1);
                if (thrustAction.ReadValue<float>() < 0)
                    throttle = Mathf.Clamp(throttle + (thrustAction.ReadValue<float>() * throttleChange * Time.deltaTime), -1, 1);
            }
            else if (controllerMode)
            {
                // adding roll to torque vector
                if (moveActionController.ReadValue<Vector2>().x < 0)
                    torq += 0.01f * rollSpeed * Time.deltaTime * transform.forward;
                if (moveActionController.ReadValue<Vector2>().x > 0)
                    torq -= 0.01f * rollSpeed * Time.deltaTime * transform.forward;

                // adding pitch to torque vector
                if (moveActionController.ReadValue<Vector2>().y > 0)
                    torq += 0.01f * pitchSpeed * Time.deltaTime * transform.right;
                if (moveActionController.ReadValue<Vector2>().y < 0)
                    torq -= 0.01f * pitchSpeed * Time.deltaTime * transform.right;

                // adding yaw to torque vector
                if (yawActionController.ReadValue<float>() < 0)
                    torq -= 0.01f * Time.deltaTime * yawSpeed * transform.up;
                if (yawActionController.ReadValue<float>() > 0)
                    torq += 0.01f * Time.deltaTime * yawSpeed * transform.up;

                // setting throttle amount
                if (thrustActionController.ReadValue<float>() > 0)
                    throttle = Mathf.Clamp(throttle + (thrustActionController.ReadValue<float>() * throttleChange * Time.deltaTime), -1, 1);
                if (thrustActionController.ReadValue<float>() < 0)
                    throttle = Mathf.Clamp(throttle + (thrustActionController.ReadValue<float>() * throttleChange * Time.deltaTime), -1, 1);
            }
        }

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
}
