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
using UnityEditor.Timeline.Actions;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class BoomArmMovement : Agent
{
    public ArticulationBody ArmAB;
    public ArticulationBody HoseAB;
    public ArticulationBody nozzleAB;
    public Collider nozzleCollider;
    public Transform fuelHole;
    public GameObject rotPoint;

    // ROLL params
    public float rollSpeed;
    public float rollMax;
    public float rollMin;
    public float rollChangeSpeed;
    // PITCH params
    public float pitchSpeed;
    public float pitchMax;
    public float pitchMin;
    public float pitchChangeSpeed;
    // BOOM EXTENSION params
    public float extendSpeed;
    public float extendMax;
    public float extendMin;
    // CLAMP params
    public bool clamped;

    // MOVE params
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode extend;
    public KeyCode retract;
    public KeyCode clampKey;
    public bool keyboardMode;
    public bool joystickMode;
    public bool autoMode;
    private InputAction moveAction;
    private InputAction extendAction;
    private InputAction clampButton;
    private PlayerInput playerInput;

    float spawnInFrontOfHole;

    [Range(0f, 100f)]
    public float fuelAmt;

    public float fuelRate;

    // ACTION LAYOUT:
    // continuous:
    //  [0] - arm pitch (-1 -> 1)
    //  [1] - arm roll (-1 -> 1)
    // discrete:
    //  [0] arm extend (0: retract, 1: remain still, 2: extend)
    //  [1] clamp (0:nothing, 1: activate/deactivate clamp)

    public PlaneMovement c5movscpt;

    public float episodeRewardNum;

    public float xRotDist;
    public float zRotDist;
    public float nozzDist;

    public Collider clampCollider;

    public bool closeEnough;

    public Camera cam1;
    public Camera cam2;

    private InputAction clampAction;
    private int flag;

    public bool controllerMode;

    private InputAction moveActionController;
    private InputAction extendActionController;
    private InputAction clampActionController;

    public bool BoomArmMode;
    private BehaviorParameters behaviorParameters;
    public EndScreen EndScreen;
    public Nozzle nozzle;
    public int counter;

    public Image clampImage;
    public Image fuelAmount;
    public RectTransform fuelAmountRect;
    public RectTransform throttleAmount;
    public TMP_Text nozzleCondition;



    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

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
        ArmAB = GetComponent<ArticulationBody>();
        HoseAB = transform.GetChild(1).GetComponent<ArticulationBody>();
        nozzleAB = transform.GetChild(1).GetChild(1).GetComponent<ArticulationBody>();
        nozzleCollider = nozzleAB.GetComponent<Collider>();
        fuelHole = transform.parent.parent.GetChild(0).GetChild(2).transform;
        rotPoint = transform.parent.GetChild(4).gameObject;

        playerInput = GetComponent<PlayerInput>();
        // reading inputs from input device
        moveAction = playerInput.actions["Move2"];
        extendAction = playerInput.actions["Extend"];
        clampAction = playerInput.actions["Clamp"];

        moveActionController = playerInput.actions["Move1"];
        extendActionController = playerInput.actions["Extend1"];
        clampActionController = playerInput.actions["Clamp1"];
        
        keyboardMode = intToBool(PlayerPrefs.GetInt("inputKeyboardBoomArm"));
        joystickMode = intToBool(PlayerPrefs.GetInt("inputJoystickBoomArm"));
        controllerMode = intToBool(PlayerPrefs.GetInt("inputControllerBoomArm"));
        BoomArmMode = intToBool(PlayerPrefs.GetInt("playerBoomArm"));



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
        counter = nozzle.CollisionCounter;
        Debug.Log(counter);

        if (Mathf.Abs(c5movscpt.distanceZ) < 36)
        {
            closeEnough = true;
        }

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
        SetUI();
        if (Input.GetKeyDown(KeyCode.O))
            EndEpisode();
        if (clamped && fuelAmt < 100)
        {
            fuelAmt = Mathf.Min(fuelAmt + (fuelRate * 0.01f), 100);
        }
        episodeRewardNum = GetCumulativeReward();
        // Debug.Log(nozzleCollider.transform.position);
    }

    // returns true or false if the nozzle is within the fuel hole
    bool withinClamp()
    {
        foreach (Collider c in Physics.OverlapSphere(nozzleAB.transform.position, 0.07f))
        {
            if (c.tag == "hole")
            {
                Debug.Log(transform.parent.parent.name + " " + c.tag);
                return true;
            }
        }
        return false;
    }

    // called at the beginning of each episode
    public override void OnEpisodeBegin()
    {
        fuelAmt = 0;
        if (clamped)
            Clamp();

        //List<float> jointPositionBackup = new List<float> { 0, 0, 0, 0, 0, 0, -0.01849806f, 0.2184863f, 0, 3.388485f };
        //ArmAB.GetJointPositions(jointPositionBackup);
        //foreach (float f in jointPositionBackup)
        //    Debug.Log(f);

        ArticulationDrive driveZ;
        driveZ = ArmAB.zDrive;
        ArticulationDrive driveY;
        driveY = ArmAB.yDrive;
        ArticulationDrive hoseZ;
        hoseZ = HoseAB.zDrive;

        spawnInFrontOfHole = Random.value;
        // spawn in hole
        if (spawnInFrontOfHole > 0.9f)
        {
            List<float> jointPositionBackup = new List<float> { 0, 0, 0, 0 };
            fuelAmt = 100f;
            AddReward(4f);
        }
        else if (spawnInFrontOfHole > 0.6f)
        {
            List<float> jointPositionBackup = new List<float> { -0.01972235f, 0.2663373f, 0, 2.478287f };
            ArmAB.SetJointPositions(jointPositionBackup);
            c5movscpt.SpawnPlane(false);
            driveZ.target = 15.26f;
            driveY.target = -1.13f;
            hoseZ.target = 2.48f;
            if (spawnInFrontOfHole > 0.8f)
                Clamp();
        }

        // spawn random
        else
        {
            List<float> jointPositionBackup = new List<float> { 0, 0, 0, 0 };
            ArmAB.SetJointPositions(jointPositionBackup);
            hoseZ.target = 0f;
            driveZ.target = 0f;
            driveY.target = 0f;

            c5movscpt.SpawnPlane(true);

        }

        ArmAB.zDrive = driveZ;
        ArmAB.yDrive = driveY;
        HoseAB.zDrive = hoseZ;
    }

    // 
    public override void CollectObservations(VectorSensor sensor)
    {
        // model input:
        // [0] : X difference between nozzle and fuel hole
        // [1] : Y difference between nozzle and fuel hole
        // [2] : Z difference between nozzle and fuel hole
        // [3] : distance (magnitude) between nozzle and fuel hole 

        // dot product between nozzle and fuel hole angle
        // Debug.Log(Vector3.Dot(nozzleCollider.transform.forward, fuelHole.transform.forward));

        // X, Y, Z differences between nozzle and fuel hole
        // Vector3 xyzDiff = (nozzleCollider.transform.position - fuelHole.transform.position);
        // sensor.AddObservation(xyzDiff.x);
        // sensor.AddObservation(xyzDiff.y);
        // sensor.AddObservation(xyzDiff.z);

        // straight distance between nozzle and fuel hole
        float straightDist = Vector3.Distance(nozzleCollider.transform.position, fuelHole.transform.position);
        if (straightDist < 0.05)
        {
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

        // Debug.Log((-straightDist / 80000) + .0000025f);
        // Debug.Log(xyzDiff.x  + "|" + xyzDiff.y + "|" + xyzDiff.z + "|" + straightDist + " || " + GetCumulativeReward());

        Vector3 idealAngle = fuelHole.position - rotPoint.transform.position;
        // Debug.DrawLine(fuelHole.position, rotPoint.transform.position, Color.green, 2);
        Vector3 currentAngle = nozzleCollider.transform.position - rotPoint.transform.position;
        // Debug.DrawLine(fuelHole.position, nozzleCollider.transform.position, Color.magenta, 2);

        float zdiff = Vector3.SignedAngle(new Vector3(idealAngle.x, idealAngle.y, 0), new Vector3(currentAngle.x, currentAngle.y, 0), nozzleCollider.transform.forward);
        float xdiff = Vector3.SignedAngle(new Vector3(0, idealAngle.y, idealAngle.z), new Vector3(0, currentAngle.y, currentAngle.z), nozzleCollider.transform.right);

        if (Mathf.Abs(zdiff) < 0.05)
            zdiff = 0;
        if (Mathf.Abs(xdiff) < 0.05)
            xdiff = 0;
        sensor.AddObservation(xdiff);
        xRotDist = xdiff;
        // sensor.AddObservation(ydiff);
        sensor.AddObservation(zdiff);
        zRotDist = zdiff;
        // Debug.Log( zdiff + "|" + xdiff + "|" + straightDist);
        // Debug.Log(GetCumulativeReward());
        // Debug.DrawLine(fuelHole.position, nozzleCollider.transform.position, Color.green);
        // Debug.DrawLine(fuelHole.position, rotPoint.transform.position, Color.blue);

        // Debug.Log(xdiff + " " + zdiff + " " + straightDist + " " + fuelAmt);

        sensor.AddObservation(fuelAmt);

        sensor.AddObservation(clamped);


        Vector3 nctp = nozzleCollider.transform.position;
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
        // getting continuous and discrete actions to pass to environment
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
                discreteActionsOut[1] = 1;
            }
            else if (clampInput == 0 && flag == 1)
            {
                flag = 0;
                discreteActionsOut[1] = 0;
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
                discreteActionsOut[1] = 1;
            else
                discreteActionsOut[1] = 0;
        }
        else if (autoMode && closeEnough)
        {
            // no movement
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
            }
        }
    }

    // takes input from user or neural network and acts in scene
    public override void OnActionReceived(ActionBuffers actions)
    {

        var contActions = actions.ContinuousActions;
        var discActions = actions.DiscreteActions;

        // Debug.Log(contActions[0] + " " + contActions[1] + " | " + discActions[0] + " " + discActions[1]);

        ArticulationDrive ad;

        // adjusting arm y target position
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

    private void OnCollisionStay(Collision collision)
    {
        // if colliding with C5
        if (collision.gameObject.layer == 6)
            AddReward(-0.00005f);
    }

    IEnumerator waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(5);
    }

    public void Clamp()
    {
        // if the arm is already clamped
        if (clamped)
        {
            // set not clamped status
            clamped = false;
            // set nozzle to not collide (trigger)
            if (fuelAmt < 100)
                AddReward(-0.3f);
            else
                AddReward(0.3f);
            clampCollider.enabled = false;
        }
        // if arm is not clamped, but is in proper clamping position
        else if (withinClamp())
        {
            // set nozzle to collide (not trigger)
            // set clamped status
            clamped = true;
            if (fuelAmt < 100)
                AddReward(0.3f);
            else
                AddReward(-0.3f);
            clampCollider.enabled = false;
        }
    }
}

/*
public class BoomArmMovement : Agent
{
    public ArticulationBody ArmAB;
    public ArticulationBody HoseAB;
    public ArticulationBody nozzleAB;
    public Collider nozzleCollider;
    public Transform fuelHole;
    public GameObject rotPoint;

    // ROLL params
    public float rollSpeed;
    public float rollMax;
    public float rollMin;
    public float rollChangeSpeed;
    // PITCH params
    public float pitchSpeed;
    public float pitchMax;
    public float pitchMin;
    public float pitchChangeSpeed;
    // BOOM EXTENSION params
    public float extendSpeed;
    public float extendMax;
    public float extendMin;
    // CLAMP params
    public bool clamped;

    // MOVE params
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode extend;
    public KeyCode retract;
    public KeyCode clampKey;
    public bool keyboardMode;
    public bool joystickMode;
    public bool controllerMode;
    public bool BoomArmMode;

    private InputAction moveAction;
    private InputAction extendAction;
    private InputAction clampAction;
    private int flag;

    private InputAction moveActionController;
    private InputAction extendActionController;
    private InputAction clampActionController;


    private PlayerInput playerInput;

    private BehaviorParameters behaviorParameters;

    public SceneSetup sceneSetup;

    float spawnInFrontOfHole;

    [Range(0f, 100f)]
    public float fuelAmt;

    public float fuelRate;

    // ACTION LAYOUT:
    // continuous:
    //  [0] - arm pitch (-1 -> 1)
    //  [1] - arm roll (-1 -> 1)
    // discrete:
    //  [0] arm extend (0: retract, 1: remain still, 2: extend)
    //  [1] clamp (0:nothing, 1: activate/deactivate clamp)

    public PlaneMovement c5movscpt;

    Vector3 armStartPos;
    Vector3 armStartRot;
    Vector3 hoseStartPos;

    public EndScreen EndScreen;
    public Nozzle nozzle;
    public int counter;

    public bool autoMode;

    public float episodeRewardNum;

    public float xRotDist;
    public float zRotDist;
    public float nozzDist;

    public Collider clampCollider;

    public Image clampImage;
    public Image fuelAmount;
    public RectTransform fuelAmountRect;
    public RectTransform throttleAmount;
    public TMP_Text nozzleCondition;

    public bool closeEnough;

    public Camera cam1;
    public Camera cam2;

    

    // called when game is started
    void Start()
    {
        ArmAB = GetComponent<ArticulationBody>();
        HoseAB = transform.GetChild(1).GetComponent<ArticulationBody>();
        nozzleAB = transform.GetChild(1).GetChild(1).GetComponent<ArticulationBody>();
        nozzleCollider = nozzleAB.GetComponent<Collider>();
        fuelHole = transform.parent.parent.GetChild(0).GetChild(2).transform;
        rotPoint = transform.parent.GetChild(4).gameObject;

        playerInput = GetComponent<PlayerInput>();
        // reading inputs from input device
        moveAction = playerInput.actions["Move"];
        extendAction = playerInput.actions["Extend"];
        retractAction = playerInput.actions["Retract"];

        c5movscpt = transform.parent.parent.GetChild(0).GetComponent<PlaneMovement>();
        c5movscpt.Start();

        clampCollider = transform.parent.parent.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Collider>();
    }

    // called every frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            EndEpisode();
        if (clamped && fuelAmt < 100)
        {
            fuelAmt = Mathf.Min(fuelAmt + (fuelRate * 0.01f), 100);
        }
        episodeRewardNum = GetCumulativeReward();
        // Debug.Log(nozzleCollider.transform.position);
    }

    // returns true or false if the nozzle is within the fuel hole
    bool withinClamp()
    {
        foreach (Collider c in Physics.OverlapSphere(nozzleAB.transform.position, 0.07f))
        {
        rotPoint = transform.parent.GetChild(4).gameObject;

        playerInput = GetComponent<PlayerInput>();
        // reading inputs from input device
        moveAction = playerInput.actions["Move"];
        extendAction = playerInput.actions["Extend"];
        clampAction = playerInput.actions["Clamp"];

        moveActionController = playerInput.actions["Move1"];
        extendActionController = playerInput.actions["Extend1"];
        clampActionController = playerInput.actions["Clamp1"];

        keyboardMode = intToBool(PlayerPrefs.GetInt("inputKeyboardBoomArm"));
        joystickMode = intToBool(PlayerPrefs.GetInt("inputJoystickBoomArm"));
        controllerMode = intToBool(PlayerPrefs.GetInt("inputControllerBoomArm"));
        BoomArmMode = intToBool(PlayerPrefs.GetInt("playerBoomArm"));

        
        
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

        armStartPos = transform.position;
        armStartRot = transform.rotation.eulerAngles;
        hoseStartPos = HoseAB.transform.position;
    }

    // called every frame
    void Update()
    {
        
    }


    // returns true or false if the nozzle is within the fuel hole
    bool withinClamp()
    {
        foreach (Collider c in Physics.OverlapSphere(nozzleAB.transform.position, 0.07f))
        {
            if (c.tag == "hole")
            {
                Debug.Log(transform.parent.parent.name + " " + c.tag);
                return true;
            if (c.tag == "hole")
            {
                Debug.Log(c.tag);
                return true;
            }
        }
        return false;
    }

    // called at the beginning of each episode
    public override void OnEpisodeBegin()
    {
        fuelAmt = 0;
        if (clamped)
            Clamp();

        //List<float> jointPositionBackup = new List<float> { 0, 0, 0, 0, 0, 0, -0.01849806f, 0.2184863f, 0, 3.388485f };
        //ArmAB.GetJointPositions(jointPositionBackup);
        //foreach (float f in jointPositionBackup)
        //    Debug.Log(f);

        ArticulationDrive driveZ;
        driveZ = ArmAB.zDrive;
        ArticulationDrive driveY;
        driveY = ArmAB.yDrive;
        ArticulationDrive hoseZ;
        hoseZ = HoseAB.zDrive;

            }
        }
        return false;
    }

    // called at the beginning of each episode
    public override void OnEpisodeBegin()
    {

        *//*ArticulationDrive driveZ;
        driveZ = ArmAB.zDrive;
        ArticulationDrive driveY;
        driveY = ArmAB.yDrive;
        ArticulationDrive hoseZ;
        hoseZ = HoseAB.zDrive;

        spawnInFrontOfHole = Random.value;
        // spawn in hole
        if (spawnInFrontOfHole > 0.60f)
        {
            List<float> jointPositionBackup = new List<float> {-0.01972235f, 0.2663373f, 0, 2.478287f};
            ArmAB.SetJointPositions(jointPositionBackup);
            c5movscpt.SpawnPlane(false);
            driveZ.target = 15.26f;
            driveY.target = -1.13f;
            hoseZ.target = 2.48f;
            if (spawnInFrontOfHole > 0.80f)
                Clamp();
        }

        spawnInFrontOfHole = Random.value;

        if (spawnInFrontOfHole > 0.90f)
        {
            hoseZ.target = 2f;
            var tempp = extendSpeed;
            extendSpeed = 5;
            StartCoroutine(waiter());
            c5movscpt.SpawnPlane(false);
            driveZ.target = 12.5f;
            driveY.target = -1.1f;
            hoseZ.target = 3.42f;
            extendSpeed = tempp;

        // spawn random
        else
        {
            List<float> jointPositionBackup = new List<float> {0, 0, 0, 0 };
            ArmAB.SetJointPositions(jointPositionBackup);
            hoseZ.target = 0f;
        {
            hoseZ.target = 0f;
            driveZ.target = 0f;
            driveY.target = 0f;

            c5movscpt.SpawnPlane(true);

        }

        ArmAB.zDrive = driveZ;
        ArmAB.yDrive = driveY;
        HoseAB.zDrive = hoseZ;
    }

    // 
    public override void CollectObservations(VectorSensor sensor)
    {
        // model input:
        // [0] : X difference between nozzle and fuel hole
        // [1] : Y difference between nozzle and fuel hole
        // [2] : Z difference between nozzle and fuel hole
        // [3] : distance (magnitude) between nozzle and fuel hole 

        // dot product between nozzle and fuel hole angle
        // Debug.Log(Vector3.Dot(nozzleCollider.transform.forward, fuelHole.transform.forward));

        // X, Y, Z differences between nozzle and fuel hole
        // Vector3 xyzDiff = (nozzleCollider.transform.position - fuelHole.transform.position);
        // sensor.AddObservation(xyzDiff.x);
        // sensor.AddObservation(xyzDiff.y);
        // sensor.AddObservation(xyzDiff.z);

        // straight distance between nozzle and fuel hole
        float straightDist = Vector3.Distance(nozzleCollider.transform.position, fuelHole.transform.position);
        if (straightDist < 0.05)
        {
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
            AddReward(straightDist / 20000);

        // Debug.Log((-straightDist / 80000) + .0000025f);
        // Debug.Log(xyzDiff.x  + "|" + xyzDiff.y + "|" + xyzDiff.z + "|" + straightDist + " || " + GetCumulativeReward());

        Vector3 idealAngle = fuelHole.position - rotPoint.transform.position;
        // Debug.DrawLine(fuelHole.position, rotPoint.transform.position, Color.green, 2);
        Vector3 currentAngle = nozzleCollider.transform.position - rotPoint.transform.position;
        // Debug.DrawLine(fuelHole.position, nozzleCollider.transform.position, Color.magenta, 2);

        float zdiff = Vector3.SignedAngle(new Vector3(idealAngle.x, idealAngle.y, 0), new Vector3(currentAngle.x, currentAngle.y, 0), nozzleCollider.transform.forward);
        float xdiff = Vector3.SignedAngle(new Vector3(0, idealAngle.y, idealAngle.z), new Vector3(0, currentAngle.y, currentAngle.z), nozzleCollider.transform.right);

        if (Mathf.Abs(zdiff) < 0.05)
            zdiff = 0;
        if (Mathf.Abs(xdiff) < 0.05)
            xdiff = 0;
        sensor.AddObservation(xdiff);
        xRotDist = xdiff;
        // sensor.AddObservation(ydiff);
        sensor.AddObservation(zdiff);
        zRotDist = zdiff;
        // Debug.Log( zdiff + "|" + xdiff + "|" + straightDist);
        // Debug.Log(GetCumulativeReward());
        // Debug.DrawLine(fuelHole.position, nozzleCollider.transform.position, Color.green);
        // Debug.DrawLine(fuelHole.position, rotPoint.transform.position, Color.blue);

        // Debug.Log(xdiff + " " + zdiff + " " + straightDist + " " + fuelAmt);

        sensor.AddObservation(fuelAmt);

        sensor.AddObservation(clamped);

        
        Vector3 nctp = nozzleCollider.transform.position;
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
        // getting continuous and discrete actions to pass to environment
        var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (joystickMode)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            float extendInput = extendAction.ReadValue<float>();

            float moveY = moveInput.y;
            float moveX = moveInput.x;


            continuousActionsOut[0] = -moveY;
            continuousActionsOut[1] = moveX;
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

        else if (autoMode)
        {
            // no movement
            discreteActionsOut[0] = 0;
            continuousActionsOut[0] = 0;
            continuousActionsOut[1] = 0;
            continuousActionsOut[2] = 0;

            if (fuelAmt < 100)
            {
                // moving arm side to side
                if (Mathf.Abs(xRotDist) > 1 || Mathf.Abs(zRotDist) > 1)
                {
                    continuousActionsOut[0] = Mathf.Clamp(xRotDist / 5, -1, 1);
                    continuousActionsOut[1] = Mathf.Clamp(zRotDist / 5, -1, 1);
                }
                // in hole
                else if (nozzDist == 0 && !clamped)
                    Clamp();
                // side to side is correct, but forward/backward needs help
                else
                {
                    continuousActionsOut[0] = Mathf.Clamp(xRotDist / 10, -1, 1);
                    continuousActionsOut[1] = Mathf.Clamp(zRotDist / 10, -1, 1);
                    // extend arm
                    continuousActionsOut[2] = Mathf.Clamp(nozzDist / 10, -1, 1);
                }
            }
            // fuel amt >= 100
            else
            {
                if (clamped)
                    Clamp();
                else if (nozzDist < 7)
                {
                    // retract arm
                    continuousActionsOut[2] = -Mathf.Clamp(1 / (nozzDist + 0.0001f), -1, 1);   
                }
            }
        }

    }

    // takes input from user or neural network and acts in scene
    public override void OnActionReceived(ActionBuffers actions)
    {

        var contActions = actions.ContinuousActions;
        var discActions = actions.DiscreteActions;

        // Debug.Log(contActions[0] + " " + contActions[1] + " | " + discActions[0] + " " + discActions[1]);

        ArticulationDrive ad;

        // adjusting arm y target position
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

    private void OnCollisionStay(Collision collision)
    {
        // if colliding with C5
        if (collision.gameObject.layer == 6)
            AddReward(-0.00005f);
    }

    IEnumerator waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(5);
    }

    public void Clamp()
    {
        // if the arm is already clamped
        if (clamped)
        {
            // set not clamped status
            clamped = false;
            // set nozzle to not collide (trigger)
            if (fuelAmt < 100)
                AddReward(-0.3f);
            else
                AddReward(0.3f);
            clampCollider.enabled = false;
        }
        // if arm is not clamped, but is in proper clamping position
        else if (withinClamp())
        {
            // set nozzle to collide (not trigger)
            // set clamped status
            clamped = true;
            if (fuelAmt < 100)
                AddReward(0.3f);
            else
                AddReward(-0.3f);
            clampCollider.enabled = false;
        }
    }
}
        }

        ArmAB.zDrive = driveZ;
        ArmAB.yDrive = driveY;
        HoseAB.zDrive = hoseZ;*//*
    }

    // 
    public override void CollectObservations(VectorSensor sensor)
    {
        // model input:
        // [0] : X difference between nozzle and fuel hole
        // [1] : Y difference between nozzle and fuel hole
        // [2] : Z difference between nozzle and fuel hole
        // [3] : distance (magnitude) between nozzle and fuel hole 

        // dot product between nozzle and fuel hole angle
        // Debug.Log(Vector3.Dot(nozzleCollider.transform.forward, fuelHole.transform.forward));

        // X, Y, Z differences between nozzle and fuel hole
        // Vector3 xyzDiff = (nozzleCollider.transform.position - fuelHole.transform.position);
        // sensor.AddObservation(xyzDiff.x);
        // sensor.AddObservation(xyzDiff.y);
        // sensor.AddObservation(xyzDiff.z);

        // straight distance between nozzle and fuel hole
        float straightDist = Vector3.Distance(nozzleCollider.transform.position, fuelHole.transform.position);
        if (straightDist < 0.05)
        {
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
            AddReward((-straightDist / 80000) + .0000025f);
        else
            AddReward(straightDist / 80000);

        // Debug.Log((-straightDist / 80000) + .0000025f);
        // Debug.Log(xyzDiff.x  + "|" + xyzDiff.y + "|" + xyzDiff.z + "|" + straightDist + " || " + GetCumulativeReward());

        Vector3 idealAngle = fuelHole.position - rotPoint.transform.position;
        // Debug.DrawLine(fuelHole.position, rotPoint.transform.position, Color.green, 2);
        Vector3 currentAngle = nozzleCollider.transform.position - rotPoint.transform.position;
        // Debug.DrawLine(fuelHole.position, nozzleCollider.transform.position, Color.magenta, 2);

        float zdiff = Vector3.SignedAngle(new Vector3(idealAngle.x, idealAngle.y, 0), new Vector3(currentAngle.x, currentAngle.y, 0), nozzleCollider.transform.forward);
        float xdiff = Vector3.SignedAngle(new Vector3(0, idealAngle.y, idealAngle.z), new Vector3(0, currentAngle.y, currentAngle.z), nozzleCollider.transform.right);

        if (Mathf.Abs(zdiff) < 0.05)
            zdiff = 0;
        if (Mathf.Abs(xdiff) < 0.05)
            xdiff = 0;
        sensor.AddObservation(xdiff);
        xRotDist = xdiff;
        // sensor.AddObservation(ydiff);
        sensor.AddObservation(zdiff);
        zRotDist = zdiff;
        // Debug.Log( zdiff + "|" + xdiff + "|" + straightDist);
        // Debug.Log(GetCumulativeReward());
        // Debug.DrawLine(fuelHole.position, nozzleCollider.transform.position, Color.green);
        // Debug.DrawLine(fuelHole.position, rotPoint.transform.position, Color.blue);

        // Debug.Log(xdiff + " " + zdiff + " " + straightDist + " " + fuelAmt);

        sensor.AddObservation(fuelAmt);

        sensor.AddObservation(clamped);
    }

    // converts user input to actions for the arm to take
    // converts user input to actions for the arm to take


    // takes input from user or neural network and acts in scene
    public override void OnActionReceived(ActionBuffers actions)
    {

        var contActions = actions.ContinuousActions;
        var discActions = actions.DiscreteActions;

        // Debug.Log(contActions[0] + " " + contActions[1] + " | " + discActions[0] + " " + discActions[1]);

        ArticulationDrive ad;

        // adjusting arm y target position
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
        if (discActions[0] == 2 && HoseAB.zDrive.target < extendMax && !clamped)
        {
            // get articulation Z drive
            ad = HoseAB.zDrive;
            // change articulation Z drive target value
            ad.target += extendSpeed * Time.deltaTime;
            // set articulation Z drive 
            HoseAB.zDrive = ad;
        }
        if (discActions[0] == 0 && HoseAB.zDrive.target > extendMin && !clamped)
        {
            // get articulation Z drive
            ad = HoseAB.zDrive;
            // change articulation Z drive target value
            ad.target -= extendSpeed * Time.deltaTime;
            // set articulation Z drive 
            HoseAB.zDrive = ad;
        }


        // clamping the arm into the refeul hole
        if (discActions[1] == 1)
        {
            Clamp();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // if colliding with C5
        if (collision.gameObject.layer == 6)
        {
            AddReward(-0.00005f);
        }
    }

    IEnumerator waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSecondsRealtime(5);
    }

    public void Clamp()
    {
        // if the arm is already clamped
        if (clamped)
        {
            // set not clamped status
            clamped = false;
            // set nozzle to not collide (trigger)
            if (fuelAmt < 100)
                AddReward(-0.3f);
            else
                AddReward(0.3f);
            clampCollider.isTrigger = true;
        }
        // if arm is not clamped, but is in proper clamping position
        else if (withinClamp())
        {
            // set nozzle to collide (not trigger)
            // set clamped status
            clamped = true;
            AddReward(0.3f);
            clampCollider.isTrigger = false;
        }
    }


*/