using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;
using Unity.MLAgents.Policies;
using UnityEngine.UI;
using TMPro;

public class BoomArmMovement : Agent
{
    /// <summary>
    /// articulation body for the boom arm 
    /// </summary>
    [HideInInspector]
    public ArticulationBody ArmAB;

    /// <summary>
    /// articulation body for the fuel hose
    /// </summary> 
    [HideInInspector]
    public ArticulationBody HoseAB;

    /// <summary>
    /// articulation body for the nozzle's tip
    /// </summary>
    [HideInInspector]
    public ArticulationBody nozzleAB;

    /// <summary>
    /// collider comoponent for the nozzle tip
    /// </summary>
    [HideInInspector]
    public Collider nozzleCollider;

    /// <summary>
    /// transform component of the C5's fuel hole
    /// </summary>
    [HideInInspector]
    public Transform fuelHole;

    /// <summary>
    /// gameObject for the rotation point of the Boom Arm
    /// </summary>
    [HideInInspector]
    public GameObject rotPoint;

    
    /// <summary>
    /// roll limit for boom arm (left)
    /// </summary>
    public float rollMax;
    /// <summary>
    /// roll limit for boom arm (right)
    /// </summary>
    public float rollMin;
    /// <summary>
    /// how fast boom arm rolls
    /// </summary>
    public float rollChangeSpeed;

    /// <summary>
    /// pitch limit for boom arm (forward)
    /// </summary>
    public float pitchMax;
    /// <summary>
    /// pitch limit for boom arm (backward)
    /// </summary>
    public float pitchMin;
    /// <summary>
    /// how fast boom arm pitches
    /// </summary>
    public float pitchChangeSpeed;

    /// <summary>
    /// how fast fuel hose extends/retracts from boom arm
    /// </summary>
    public float extendSpeed;
    /// <summary>
    /// extension limit for fuel hose
    /// </summary>
    public float extendMax;
    /// <summary>
    /// retract limit for fuel hose
    /// </summary>
    public float extendMin;

    /// <summary>
    /// whether the nozzle is clamped to the fuel hole or not
    /// </summary>
    [HideInInspector]
    public bool clamped;

    /// <summary>
    /// keyboard key to move the boom arm forward
    /// </summary>
    public KeyCode forward;
    /// <summary>
    /// keyboard key to move the boom arm backward
    /// </summary>
    public KeyCode backward;
    /// <summary>
    /// keyboard key to move the boom arm left
    /// </summary>
    public KeyCode left;
    /// <summary>
    /// keyboard key to move the boom arm right
    /// </summary>
    public KeyCode right;
    /// <summary>
    /// keyboard key to extend the fuel hose
    /// </summary>
    public KeyCode extend;
    /// <summary>
    /// keyboard key to retract the fuel hose
    /// </summary>
    public KeyCode retract;
    /// <summary>
    /// keyboard key to clamp the nozzle to the fuel hole
    /// </summary>
    public KeyCode clampKey;

    /// <summary>
    /// true if boom arm is controlled by keyboard
    /// </summary>
    [HideInInspector]
    public bool keyboardMode;
    /// <summary>
    /// true if boom arm is controlled by joystick
    /// </summary>
    [HideInInspector]
    public bool joystickMode;
    /// <summary>
    /// true if boom arm is controlled by AI
    /// </summary>
    [HideInInspector]
    public bool autoMode;

    /// <summary>
    /// joystick action for pitch/roll arm movement
    /// </summary>
    [HideInInspector]
    public InputAction moveAction;
    /// <summary>
    /// joystick action to extend/retract fuel hose
    /// </summary>
    [HideInInspector]
    public InputAction extendAction;
    /// <summary>
    /// joystick action clamp button
    /// </summary>
    [HideInInspector]
    public InputAction clampAction;
    /// <summary>
    /// player input component for joystick control
    /// </summary>
    [HideInInspector]
    public PlayerInput playerInput;

    /// <summary>
    /// random value to determine starting state while training
    /// </summary>
    float spawnInFrontOfHole;

    /// <summary>
    /// how much fuel has passed from the KC135 to the C5
    /// </summary>
    [Range(0f, 100f)]
    [HideInInspector]
    public float fuelAmt;

    /// <summary>
    /// how quickly the fuel passes from the KC135 to the C5
    /// </summary>
    public float fuelRate;

    /// <summary>
    /// pointer to the script that controlls the C5's movement
    /// </summary>
    [HideInInspector]
    public PlaneMovement c5movscpt;

    /// <summary>
    /// how much reward the Agent has acquired in this current episode of training
    /// </summary>
    [HideInInspector]
    public float episodeRewardNum;

    /// <summary>
    /// how much the boom arm has pitched
    /// </summary>
    [HideInInspector]
    public float xRotDist;
    /// <summary>
    /// how much the boom arm has rolled
    /// </summary>
    [HideInInspector]
    public float zRotDist;
    /// <summary>
    /// distance from nozzle tip to C5 fuel hole
    /// </summary>
    [HideInInspector]
    public float nozzDist;

    /// <summary>
    /// collider that clamps the nozzle to the C5 fuel hole
    /// </summary>
    [HideInInspector]
    public Collider clampCollider;

    /// <summary>
    /// used in respawning the aircraft if they get too far from eachother
    /// </summary>
    [HideInInspector]
    public bool closeEnough;

    /// <summary>
    /// normal boom arm camera 
    /// </summary>
    [HideInInspector]
    public Camera cam1;
    /// <summary>
    /// 'handicap mode' camera for boom arm pilot
    /// </summary>
    [HideInInspector]
    public Camera cam2;

    /// <summary>
    /// flag used in clamping to ensure the clamp fires once per button press
    /// </summary>
    [HideInInspector]
    public int flag;

    /// <summary>
    /// if true, player is using a PS4 controller
    /// </summary>
    [HideInInspector]
    public bool controllerMode;

    /// <summary>
    /// boom arm movement for joystick controller
    /// </summary>
    [HideInInspector]
    public InputAction moveActionController;
    /// <summary>
    /// fuel hose movement for joystick controller
    /// </summary>
    [HideInInspector]
    public InputAction extendActionController;
    /// <summary>
    /// clamp movement for joystick controller
    /// </summary>
    [HideInInspector]
    public InputAction clampActionController;
    /// <summary>
    /// if true, AI pilots boom arm
    /// </summary>
    [HideInInspector]
    public bool BoomArmMode;
    /// <summary>
    /// setting for how the arm is controlled
    /// </summary>
    [HideInInspector]
    public BehaviorParameters behaviorParameters;
    /// <summary>
    /// pointer to EndScreen script
    /// </summary>
    [HideInInspector]
    public EndScreen EndScreen;
    /// <summary>
    /// pointer to Nozzle script
    /// </summary>
    [HideInInspector]
    public Nozzle nozzle;
    /// <summary>
    /// counts how many collisions have occured between the boom arm and C5
    /// </summary>
    [HideInInspector]
    public int counter;

    /// <summary>
    /// clamp indicator on HUD
    /// </summary>
    [HideInInspector]
    public Image clampImage;
    /// <summary>
    /// fuel amount indicator on HUD
    /// </summary>
    [HideInInspector]
    public Image fuelAmount;
    /// <summary>
    /// shape of fuel amount indicator on HUD
    /// </summary>
    [HideInInspector]
    public RectTransform fuelAmountRect;
    /// <summary>
    /// shape of throttle indicator on HUD
    /// </summary>
    [HideInInspector]
    public RectTransform throttleAmount;
    /// <summary>
    /// damage taken to the boom arm indicator on HUD
    /// </summary>
    [HideInInspector]
    public TMP_Text nozzleCondition;


    /// <summary>
    /// converts an Int to a bool
    /// </summary>
    /// <param name="val">int to be converted</param>
    /// <returns></returns>
    bool IntToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// sets indicators on the HUD during simulation
    /// </summary>
    public void SetUI()
    {
        // set clamped status
        if (clamped)
            clampImage.color = Color.green;
        else
            clampImage.color = Color.red;

        fuelAmountRect.sizeDelta = new Vector2(15, fuelAmt * 3.56f);

        throttleAmount.sizeDelta = new Vector2(15, c5movscpt.throttle * 178 + 178);
    }

    // called when game is started
    void Start()
    {
        // setting up lots of variables when scene starts
        ArmAB = GetComponent<ArticulationBody>();
        HoseAB = transform.GetChild(1).GetComponent<ArticulationBody>();
        nozzleAB = transform.GetChild(1).GetChild(1).GetComponent<ArticulationBody>();
        nozzleCollider = nozzleAB.GetComponent<Collider>();
        fuelHole = transform.parent.parent.GetChild(0).GetChild(2).transform;
        rotPoint = transform.parent.GetChild(4).gameObject;

        playerInput = GetComponent<PlayerInput>();
        // reading inputs from input device
        moveAction = playerInput.actions["Move2"];
        extendAction = playerInput.actions["Extend2"];
        clampAction = playerInput.actions["Clamp2"];

        moveActionController = playerInput.actions["Move1"];
        extendActionController = playerInput.actions["Extend1"];
        clampActionController = playerInput.actions["Clamp1"];
        
        keyboardMode = IntToBool(PlayerPrefs.GetInt("inputKeyboardBoomArm"));
        joystickMode = IntToBool(PlayerPrefs.GetInt("inputJoystickBoomArm"));
        controllerMode = IntToBool(PlayerPrefs.GetInt("inputControllerBoomArm"));
        BoomArmMode = IntToBool(PlayerPrefs.GetInt("playerBoomArm"));


        // setting up control scheme
        if (BoomArmMode && !keyboardMode && !joystickMode && !controllerMode)
        {
            keyboardMode = true;
            behaviorParameters = transform.GetComponent<BehaviorParameters>();
            behaviorParameters.BehaviorType = BehaviorType.HeuristicOnly;
        }
        else if (BoomArmMode)
        {
            behaviorParameters = transform.GetComponent<BehaviorParameters>();
            behaviorParameters.BehaviorType = BehaviorType.HeuristicOnly;
        }
        else if (!BoomArmMode)
        {
            autoMode = true;
            behaviorParameters = transform.GetComponent<BehaviorParameters>();
            behaviorParameters.BehaviorType = BehaviorType.HeuristicOnly;
        }


        c5movscpt = transform.parent.parent.GetChild(0).GetComponent<PlaneMovement>();
        c5movscpt.Start();

        clampCollider = transform.parent.parent.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Collider>();
    }

    // called every frame
    void Update()
    {
        // getting collision count
        counter = nozzle.CollisionCounter;

        // setting 'closeEnough' based on distance between two aircraft
        if (Mathf.Abs(c5movscpt.distanceZ) < 34)
        {
            closeEnough = true;
        }
        else
        {
            closeEnough = false;
        }

        // end sim if damage taken is too much 
        if (counter <= 0 || Input.GetKeyDown(KeyCode.Escape))
        {
            EndScreen.SetUp();
        }
        else if (counter > 0)
        {
            nozzleCondition.text = "NOZZLE CONDITION: " + (counter / 3) + "%";
        }
        else
        {
            nozzleCondition.text = "NOZZLE CONDITION: 100%";
        }
        // set up UI based on condition of simulation
        SetUI();
        if (Input.GetKeyDown(KeyCode.O))
            EndEpisode();

        // if clamp is activated and fuel < 100, add fuel to fuelAmt
        if (clamped && fuelAmt < 100)
        {
            fuelAmt = Mathf.Min(fuelAmt + (fuelRate * 0.01f), 100);
        }

        // setting this variable to the episode's current reward
        episodeRewardNum = GetCumulativeReward();
    }

    /// <summary>
    /// returns true or false if the nozzle is within the fuel hole
    /// </summary>
    /// <returns></returns>
    bool withinClamp()
    {
        // for every collider found at the sphere cast at the nozzle's position
        foreach (Collider c in Physics.OverlapSphere(nozzleAB.transform.position, 0.07f))
        {
            // if that collider is the "hole"
            if (c.tag == "hole")
            {
                // the nozzle is within clamp range
                return true;
            }
        }
        return false;
    }

    // called at the beginning of each episode, used in training.
    public override void OnEpisodeBegin()
    {
        // set fuel to 0 at start of episode
        fuelAmt = 0;

        // unclamp if clamped at start of episode
        if (clamped)
            Clamp();

        // getting data about the arm's position
        ArticulationDrive driveZ;
        driveZ = ArmAB.zDrive;
        ArticulationDrive driveY;
        driveY = ArmAB.yDrive;
        ArticulationDrive hoseZ;
        hoseZ = HoseAB.zDrive;

        // determining random value used in spawning the c5 and arm positions
        spawnInFrontOfHole = Random.value;

        // 10% of the time, arm spawns away from the hole with 100 fuel
        if (spawnInFrontOfHole > 0.9f)
        {
            // setting arm position
            List<float> jointPositionBackup = new List<float> { 0, 0, 0, 0 };
            fuelAmt = 100f;
            AddReward(4f);
        }

        // 30% of the time, arm spawns inside fuel hole
        else if (spawnInFrontOfHole > 0.6f)
        {
            // setting arm position
            List<float> jointPositionBackup = new List<float> { -0.01972235f, 0.2663373f, 0, 2.478287f };
            ArmAB.SetJointPositions(jointPositionBackup);

            // spawning plane
            c5movscpt.SpawnPlane(false);
            driveZ.target = 15.26f;
            driveY.target = -1.13f;
            hoseZ.target = 2.48f;
            // 10% of the time, arm spawns inside fuel hole already clamped
            if (spawnInFrontOfHole > 0.8f)
                Clamp();
        }

        // 60% of the time, C5 spawns randomly, arm starts at 0 position.
        else
        {
            List<float> jointPositionBackup = new List<float> { 0, 0, 0, 0 };
            ArmAB.SetJointPositions(jointPositionBackup);
            hoseZ.target = 0f;
            driveZ.target = 0f;
            driveY.target = 0f;
            c5movscpt.SpawnPlane(true);

        }

        // setting the target positions for the arm based on previous parts of this function.
        ArmAB.zDrive = driveZ;
        ArmAB.yDrive = driveY;
        HoseAB.zDrive = hoseZ;
    }

    // used to add observations from the environment to the agent
    public override void CollectObservations(VectorSensor sensor)
    {
        // model input:
        // [ 0] : distance from nozzle to fuel hole
        // [ 1] : arm pitch angular difference
        // [ 2] : arm roll angular difference
        // [ 3] : fuel amount delivered
        // [ 4] : clamped status
        // [ 5] : X pos of nozzle tip in space 
        // [ 6] : Y pos of nozzle tip in space
        // [ 7] : Z pos of nozzle tip in space
        // [ 8] : X pos of fuel hole in space 
        // [ 9] : Y pos of fuel hole in space
        // [10] : Z pos of fuel hole in space

        // straight distance between nozzle and fuel hole
        float straightDist = Vector3.Distance(nozzleCollider.transform.position, fuelHole.transform.position);

        // if distance between nozzle and hole is negligable
        if (straightDist < 0.05)
        {
            // set it to be 0
            sensor.AddObservation(0);
            nozzDist = 0;
        }
        else
        {
            sensor.AddObservation(straightDist);
            nozzDist = straightDist;
        }

        // add reward for being close when needing to fuel, and being far away when done fueling
        // 0.0001 -> 0.000001 ish per frame
        if (fuelAmt < 100)
            AddReward((-straightDist / 20000) + .0000025f);
        else
            AddReward(straightDist / 2000);

        // used in calculating the angle between the arm's current position and ideal position, pointed at the fuel hole
        Vector3 idealAngle = fuelHole.position - rotPoint.transform.position;
        Vector3 currentAngle = nozzleCollider.transform.position - rotPoint.transform.position;
        float zdiff = Vector3.SignedAngle(new Vector3(idealAngle.x, idealAngle.y, 0), new Vector3(currentAngle.x, currentAngle.y, 0), nozzleCollider.transform.forward);
        float xdiff = Vector3.SignedAngle(new Vector3(0, idealAngle.y, idealAngle.z), new Vector3(0, currentAngle.y, currentAngle.z), nozzleCollider.transform.right);

        // if angle is negligable, set it to 0
        if (Mathf.Abs(zdiff) < 0.05)
            zdiff = 0;
        if (Mathf.Abs(xdiff) < 0.05)
            xdiff = 0;
        sensor.AddObservation(xdiff);
        xRotDist = xdiff;
        sensor.AddObservation(zdiff);
        zRotDist = zdiff;
        
        sensor.AddObservation(fuelAmt);

        sensor.AddObservation(clamped);

        // position of nozzle in space
        Vector3 nctp = nozzleCollider.transform.position;
        // position of fuel hole in space
        Vector3 fhp = fuelHole.position;
        sensor.AddObservation(nctp.x);
        sensor.AddObservation(nctp.y);
        sensor.AddObservation(nctp.z);
        sensor.AddObservation(fhp.x);
        sensor.AddObservation(fhp.y);
        sensor.AddObservation(fhp.z);
    }

    // converts user input to actions for the arm to take
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // enables/disables 'handicap mode' for boom arm pilot
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            if (cam1.enabled)
            {
                cam1.enabled = false;
                cam2.enabled = true;
            }
            else
            {
                cam1.enabled = true;
                cam2.enabled = false;
            }
        }

        // getting continuous and discrete actions from user to pass to environment like the agent would
        var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (joystickMode)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            float extendInput = extendAction.ReadValue<float>();
            float clampInput = clampAction.ReadValue<float>();
            float moveY = moveInput.y;
            float moveX = moveInput.x;


            continuousActionsOut[0] = moveY;
            continuousActionsOut[1] = moveX;

            if (extendInput > 0)
            {
                continuousActionsOut[2] = 1;
            }
            else if (extendInput < 0)
            {
                continuousActionsOut[2] = -1;
            }
            else
            {
                continuousActionsOut[2] = 0;
            }

            if (clampInput == 1 && flag == 0)
            {
                flag = 1;
                discreteActionsOut[0] = 1;
            }
            else if (clampInput == 0 && flag == 1)
            {
                flag = 0;
                discreteActionsOut[0] = 0;
            }
        }
        else if (controllerMode)
        {
            Vector2 moveInput = moveActionController.ReadValue<Vector2>();
            float extendInput = extendActionController.ReadValue<float>();
            float clampInput = clampActionController.ReadValue<float>();

            float moveY = moveInput.y;
            float moveX = moveInput.x;

            continuousActionsOut[0] = moveY;
            continuousActionsOut[1] = moveX;

            if (extendInput > 0)
            {
                discreteActionsOut[0] = 2;
            }
            else if (extendInput < 0)
            {
                discreteActionsOut[0] = 0;
            }
            else
            {
                discreteActionsOut[0] = 1;
            }

            if (clampInput == 1 && flag == 0)
            {
                flag = 1;
                discreteActionsOut[1] = 1;
            }
            else if (clampInput == 0 && flag == 1)
            {
                flag = 0;
                discreteActionsOut[1] = 0;
            }
        }
        else if (keyboardMode)
        {
            // pitch
            if (Input.GetKey(backward))
                continuousActionsOut[0] = 1;
            else if (Input.GetKey(forward))
                continuousActionsOut[0] = -1;
            else
                continuousActionsOut[0] = 0;

            // roll
            if (Input.GetKey(right))
                continuousActionsOut[1] = 1;
            else if (Input.GetKey(left))
                continuousActionsOut[1] = -1;
            else
                continuousActionsOut[1] = 0;

            // extension amount
            if (Input.GetKey(extend))
                continuousActionsOut[2] = 1;
            else if (Input.GetKey(retract))
                continuousActionsOut[2] = -1;
            else
                continuousActionsOut[2] = 0;

            // clamp
            if (Input.GetKeyDown(clampKey))
                discreteActionsOut[0] = 1;
            else
                discreteActionsOut[0] = 0;
        }
        else if (autoMode && closeEnough)
        {
            behaviorParameters = transform.GetComponent<BehaviorParameters>();
            behaviorParameters.BehaviorType = BehaviorType.Default;
            /* // no movement
             discreteActionsOut[0] = 1;
             discreteActionsOut[1] = 0;
             continuousActionsOut[0] = 0;
             continuousActionsOut[1] = 0;

             if (fuelAmt < 100)
             {
                 // moving arm side to side
                 if (Mathf.Abs(xRotDist) > 0.5f || Mathf.Abs(zRotDist) > 0.5f)
                 {
                     continuousActionsOut[0] = Mathf.Clamp(xRotDist, -1, 1) / 5;
                     continuousActionsOut[1] = Mathf.Clamp(zRotDist, -1, 1) / 5;
                 }
                 // in hole
                 else if (nozzDist == 0 && !clamped)
                     Clamp();
                 // side to side is correct, but forward/backward needs help
                 else
                 {
                     continuousActionsOut[0] = Mathf.Clamp(xRotDist, -1, 1) / 10;
                     continuousActionsOut[1] = Mathf.Clamp(zRotDist, -1, 1) / 10;
                     // extend arm
                     discreteActionsOut[0] = 2;
                 }
             }
             // fuel amt >= 100
             else
             {
                 if (clamped)
                     Clamp();
                 else
                 {
                     // retract arm
                     discreteActionsOut[0] = 0;
                 }
             }*/

        }
    }

    // takes input from user or neural network and acts in scene
    public override void OnActionReceived(ActionBuffers actions)
    {
        // getting both the continuous actions and discrete actions from user / agent
        var contActions = actions.ContinuousActions;
        var discActions = actions.DiscreteActions;

        // getting information about the Articulation Body of the arm
        ArticulationDrive ad;

        // adjusting arm x target position
        if (contActions[0] < 0 && ArmAB.zDrive.target > pitchMin && !clamped)
        {

            ad = ArmAB.zDrive;
            ad.target += pitchChangeSpeed * Time.deltaTime * contActions[0];
            ArmAB.zDrive = ad;
        }
        if (contActions[0] > 0 && ArmAB.zDrive.target < pitchMax && !clamped)
        {
            ad = ArmAB.zDrive;
            ad.target += pitchChangeSpeed * Time.deltaTime * contActions[0];
            ArmAB.zDrive = ad;
        }

        // adjusting arm x target position
        if (contActions[1] < 0 && ArmAB.yDrive.target > rollMin && !clamped)
        {
            ad = ArmAB.yDrive;
            ad.target += rollChangeSpeed * Time.deltaTime * contActions[1];
            ArmAB.yDrive = ad;
        }
        if (contActions[1] > 0 && ArmAB.yDrive.target < rollMax && !clamped)
        {
            ad = ArmAB.yDrive;
            ad.target += rollChangeSpeed * Time.deltaTime * contActions[1];
            ArmAB.yDrive = ad;
        }

        // extending / retracting arm
        if (contActions[2] > 0 && HoseAB.zDrive.target < extendMax && !clamped)
        {
            // get articulation Z drive
            ad = HoseAB.zDrive;
            // change articulation Z drive target value
            ad.target += extendSpeed * Time.deltaTime * contActions[2];
            // set articulation Z drive 
            HoseAB.zDrive = ad;
        }
        if (contActions[2] < 0 && HoseAB.zDrive.target > extendMin && !clamped)
        {
            // get articulation Z drive
            ad = HoseAB.zDrive;
            // change articulation Z drive target value
            ad.target += extendSpeed * Time.deltaTime * contActions[2];
            // set articulation Z drive 
            HoseAB.zDrive = ad;
        }


        // clamping the arm into the refeul hole
        if (discActions[0] == 1)
        {
            Clamp();
        }
    }

    // used to add negative reward based on if arm is colliding with the C5
    private void OnCollisionStay(Collision collision)
    {
        // if colliding with C5
        if (collision.gameObject.layer == 6)
            AddReward(-0.00005f);
    }

    /// <summary>
    /// Clamps the nozzle to the fuel hole if in range, or unclamps if already clamped
    /// </summary>
    public void Clamp()
    {
        // if the arm is already clamped
        if (clamped)
        {
            // set not clamped status
            clamped = false;
            // add reward
            if (fuelAmt < 100)
                AddReward(-0.3f);
            else
                AddReward(0.3f);
            // disabling physical clamp mechanism
            clampCollider.enabled = false;
        }
        // if arm is not clamped, but is in proper clamping position
        else if (withinClamp())
        {
            // set clamped status
            clamped = true;
            // add reward
            if (fuelAmt < 100)
                AddReward(0.3f);
            else
                AddReward(-0.3f);
            // enabling physical clamp mechanism
            clampCollider.enabled = true;
        }
    }
}