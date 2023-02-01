using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlaneMovement : MonoBehaviour
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
    /// the current throttle level
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
    [Range(0f, 2000f)]
    public float lift;

    /// <summary>
    /// pointer to the rigidbody this script is affecting
    /// </summary>
    [HideInInspector]
    public Rigidbody plane;

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
    /// pointer to the C5 gameObject
    /// </summary>
    [HideInInspector]
    public GameObject C5go;
    /// <summary>
    /// pointer to the KC135 gameObject
    /// </summary>
    [HideInInspector]
    public GameObject KC135go;
    /// <summary>
    /// position vector of the C5
    /// </summary>
    Vector3 C5;
    /// <summary>
    /// position vector of the KC135
    /// </summary>
    Vector3 KC135;

    /// <summary>
    /// if true, auto-target C5 movement
    /// </summary>
    [HideInInspector]
    public bool targetMode;
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
    /// if C5 is player-controlled
    /// </summary>
    [HideInInspector]
    public bool playerC5;

    /// <summary>
    /// playerInput object for C5 control
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
    /// target position of C5 for auto-pilot
    /// </summary>
    [HideInInspector]
    public Vector3 targetPos;
    /// <summary>
    /// maximum roll amount for C5
    /// </summary>
    public float rollMax;
    /// <summary>
    /// maximum pitch amount for C5
    /// </summary>
    public float pitchMax;

    /// <summary>
    /// C5 position at start of sim
    /// </summary>
    private Vector3 startPos;
    /// <summary>
    /// C5 rotation at start of sim
    /// </summary>
    private Quaternion startRot;

    /// <summary>
    /// toggle for volumetric clouds
    /// </summary>
    private Boolean volumetricClouds;
    /// <summary>
    /// toggle for fog effects
    /// </summary>
    private Boolean fogToggle;
    /// <summary>
    /// time of day setting
    /// </summary>
    private int timeOfDay;

    /// <summary>
    /// pointer to volumetric clouds gameobject
    /// </summary>
    [HideInInspector]
    public GameObject volumetricCloudsObject;
    /// <summary>
    /// pointer to fog controller gameObject
    /// </summary>
    [HideInInspector]
    public GameObject fogObject;

    /// <summary>
    /// volumetric clouds volume object
    /// </summary>
    [HideInInspector]
    public Volume volume;
    /// <summary>
    /// used in cloud settings
    /// </summary>
    VolumetricClouds cloud;
    /// <summary>
    /// used in cloud settings
    /// </summary>
    VolumetricClouds clouds;
    /// <summary>
    /// used in fog settings
    /// </summary>
    Fog fogVolume;
    /// <summary>
    /// used in fog settings
    /// </summary>
    Fog fog;

    /// <summary>
    /// game object pointer to sunlight 
    /// </summary>
    [HideInInspector]
    public GameObject sun;

    /// <summary>
    /// pointer to EndScreen script
    /// </summary>
    [HideInInspector]
    public EndScreen EndScreen;
    /// <summary>
    /// pointer to BoomArmMovement Script
    /// </summary>
    [HideInInspector]
    public BoomArmMovement BoomArmMovement;
    /// <summary>
    /// how much fuel has been delivered to the c5
    /// </summary>
    [HideInInspector]
    public float fuel;
    /// <summary>
    /// used to track collisions with the boom arm
    /// </summary>
    [HideInInspector]
    public int C5Counter;

    /// <summary>
    /// tracks X distance between aircraft
    /// </summary>
    [HideInInspector]
    public float distanceX;
    /// <summary>
    /// tracks Y distance between aircraft
    /// </summary>
    [HideInInspector]
    public float distanceY;
    /// <summary>
    /// tracks Z distance between aircraft
    /// </summary>
    [HideInInspector]
    public float distanceZ;

    /// <summary>
    /// converts an int into a bool (used in player preference handling)
    /// </summary>
    /// <param name="val">value to convert</param>
    /// <returns></returns>
    bool IntToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

    // called upon script startup
    public void Start()
    {
        // setup pointers
        plane = GetComponent<Rigidbody>();
        startPos = transform.localPosition;
        startRot = transform.rotation;
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move2"];
        thrustAction = playerInput.actions["Thrust2"];
        yawAction = playerInput.actions["Yaw2"];

        moveActionController = playerInput.actions["Move1"];
        thrustActionController = playerInput.actions["Thrust1"];
        yawActionController = playerInput.actions["Yaw1"];

        keyboardMode = IntToBool(PlayerPrefs.GetInt("inputKeyboardC5"));
        joystickMode = IntToBool(PlayerPrefs.GetInt("inputJoystickC5"));
        controllerMode = IntToBool(PlayerPrefs.GetInt("inputControllerC5"));

        playerC5 = IntToBool(PlayerPrefs.GetInt("playerC5"));

        volumetricClouds = IntToBool(PlayerPrefs.GetInt("volumetricCloudsBool"));
        fogToggle = IntToBool(PlayerPrefs.GetInt("fogBool"));
        timeOfDay = PlayerPrefs.GetInt("TimeOfDay");

        volume = volumetricCloudsObject.GetComponent<Volume>();
        
        if (volume.profile.TryGet<VolumetricClouds>(out cloud))
        {
            clouds = cloud;
        }
        if (volume.profile.TryGet<Fog>(out fogVolume))
        {
            fog = fogVolume;
        }
        clouds.active = volumetricClouds;
        fog.active = fogToggle;

        sun.transform.rotation = Quaternion.Euler((timeOfDay * 15 - 90), 0, 0);

        if (!playerC5)
        {
            targetMode = true;
        }
        else if (playerC5 && !keyboardMode && !joystickMode && !controllerMode)
        {
            keyboardMode = true;
        }
    }

    void Update()
    {
        // check if plane needs to be reset each frame
        ResetPlane();

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
            // controll with keyboard
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

    /// <summary>
    /// calculate if planes need to be reset due to distance between
    /// </summary>
    void ResetPlane()
    {
        // get positions of C5, KC135, and fuel amount
        C5 = C5go.transform.position;
        KC135 = KC135go.transform.position;
        fuel = BoomArmMovement.fuelAmt;

        // calculate XYZ difference btween C5 and KC135
        Vector3 difference = new Vector3(C5.x - KC135.x, C5.y - KC135.y, C5.z - KC135.z);

        distanceX = difference.x;
        distanceY = difference.y;
        distanceZ = difference.z;

        // if enough fuel has been delivered
        if (fuel >= 100)
        {
            // and autoflight and arm is not clamped
            if (!playerC5 && !BoomArmMovement.clamped)
            {
                // end sim
                EndScreen.SetUp();
            }
            // player-controlled or arm is clamped
            else
            {
                // if distance is too large between aircraft
                if (distanceX > 60 || distanceX < -60 || distanceY > 60 || distanceY < -60 || distanceZ > 30 || distanceZ < -50)
                {
                    // end sim
                    EndScreen.SetUp();
                }
            }
        }
        // not enough fuel has been delivered
        else
        {
            // if distance is too large between aircraft
            if (distanceX > 60 || distanceX < -60 || distanceY > 60 || distanceY < -60 || distanceZ > 60 || distanceZ < -100)
            {
                // end sim
                EndScreen.SetUp();
            }
        }
        
    }

    // used to detect colisions between C5 and KC135
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "KC135")
        {
            // if too much damage taken
            if (C5Counter >= 100)
            {
                // end sim
                EndScreen.SetUp();
            }
            // increase damage
            else
                C5Counter++;
        }
    }

    /// <summary>
    /// used to spawn the C5 in-world
    /// </summary>
    /// <param name="randPos">if true, C5 has random spawn position, otherwise, just spawn at start position</param>
    public void SpawnPlane(bool randPos)
    {
        transform.localPosition = startPos;
        transform.rotation = startRot;

        if (randPos)
        {
        // get random values for position and rotation
        float rangeRotationX = UnityEngine.Random.Range(-2.5f, 2.5f);
        float rangeRotationY = UnityEngine.Random.Range(-2.5f, 2.5f);
        float rangeRotationZ = UnityEngine.Random.Range(-5, 5);
        float rangeDistanceZ = UnityEngine.Random.Range(-1.25f, 1.25f);
        float rangeDistanceX = UnityEngine.Random.Range(-0.75f, 0.75f);
        float rangeDistanceY = UnityEngine.Random.Range(-0.2f, 0.2f);

        // apply random values to position and rotation
        transform.rotation = Quaternion.Euler(rangeRotationX, rangeRotationY, rangeRotationZ);
        transform.position += new Vector3(rangeDistanceX, rangeDistanceY, rangeDistanceZ);
        }
    }
}
