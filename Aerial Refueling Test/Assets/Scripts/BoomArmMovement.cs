using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

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
    private InputAction moveAction;
    private InputAction extendAction;
    private InputAction retractAction;
    private InputAction clampButton;
    private PlayerInput playerInput;

    float spawnInFrontOfHole;

    Vector3 startPos;
    Quaternion startRot;
    GameObject c5;


    // ACTION LAYOUT:
    // continuous:
    //  [0] - arm pitch (-1 -> 1)
    //  [1] - arm roll (-1 -> 1)
    // discrete:
    //  [0] arm extend (0: retract, 1: remain still, 2: extend)
    //  [1] clamp (0:nothing, 1: activate/deactivate clamp)

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

        c5 = transform.parent.parent.GetChild(0).gameObject;

        startPos = c5.transform.position;
        startRot = c5.transform.rotation;
    }

    // called every frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            EndEpisode();
    }

    // returns true or false if the nozzle is within the fuel hole
    bool withinClamp()
    {
        foreach (Collider c in Physics.OverlapSphere(nozzleAB.transform.position, 0.07f))
        {
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
        c5.transform.position = startPos;
        c5.transform.rotation = startRot;

        spawnInFrontOfHole = Random.value;

        ArticulationDrive driveZ;
        driveZ = ArmAB.zDrive;
        ArticulationDrive driveY;
        driveY = ArmAB.yDrive;
        ArticulationDrive hoseZ;
        hoseZ = HoseAB.zDrive;

        if (spawnInFrontOfHole > 0.90f)
        // if (spawnInFrontOfHole > 2)
        {
            driveZ.target = 15f;
            driveY.target = -1.4f;
            hoseZ.target = 2f;
        } else
        {
            driveZ.target = 0f;
            driveY.target = 0f;
            hoseZ.target = 0f;

            float rangeRotationX = Random.Range(-2.5f, 2.5f);
            float rangeRotationY = Random.Range(-2.5f, 2.5f);
            float rangeRotationZ = Random.Range(-5, 5);
            float rangeDistanceZ = Random.Range(-3.5f, 0f);

            c5.transform.rotation = Quaternion.Euler(rangeRotationX, rangeRotationY, rangeRotationZ);
            c5.transform.position += new Vector3(0, 0, rangeDistanceZ);
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
        sensor.AddObservation(straightDist);
        AddReward(-straightDist);

        // Debug.Log(xyzDiff.x  + "|" + xyzDiff.y + "|" + xyzDiff.z + "|" + straightDist + " || " + GetCumulativeReward());

        Vector3 idealAngle = fuelHole.position - rotPoint.transform.position;
        Vector3 currentAngle = fuelHole.position - nozzleCollider.transform.position;
        float zdiff = Vector3.SignedAngle(new Vector3(idealAngle.x, idealAngle.y, 0), new Vector3(currentAngle.x, currentAngle.y, 0), transform.forward);
        // float ydiff = Vector3.Angle(new Vector3(idealAngle.x, 0, idealAngle.z), new Vector3(currentAngle.x, 0, currentAngle.z));
        float xdiff = Vector3.SignedAngle(new Vector3(0, idealAngle.y, idealAngle.z), new Vector3(0, currentAngle.y, currentAngle.z), transform.right);
        sensor.AddObservation(xdiff);
        // sensor.AddObservation(ydiff);
        sensor.AddObservation(zdiff);
        // Debug.Log( zdiff + "|" + xdiff + "|" + straightDist);
        // Debug.Log(GetCumulativeReward());
        // Debug.DrawLine(fuelHole.position, nozzleCollider.transform.position, Color.green);
        // Debug.DrawLine(fuelHole.position, rotPoint.transform.position, Color.blue);

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
                discreteActionsOut[0] = 2;
            else if (Input.GetKey(retract))
                discreteActionsOut[0] = 0;
            else
                discreteActionsOut[0] = 1;

            // clamp
            if (Input.GetKey(clampKey))
                discreteActionsOut[1] = 1;
            else
                discreteActionsOut[1] = 0; 
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
            // if the arm is already clamped
            if (clamped)
            {
                // set not clamped status
                clamped = false;
                // set nozzle to not collide (trigger)
                nozzleCollider.isTrigger = true;
                AddReward(-2000);
            }
            // if arm is not clamped, but is in proper clamping position
            else if (withinClamp())
            {
                // set nozzle to collide (not trigger)
                nozzleCollider.isTrigger = false;
                // set clamped status
                clamped = true;
                AddReward(2000);
            }
        }  
    }

    private void OnCollisionStay(Collision collision)
    {
        // if colliding with C5
        if (collision.gameObject.layer == 6)
            AddReward(-1f);
    }
}
