using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Nozzle : MonoBehaviour
{
    BoomArmMovement bam;
    public int CollisionCounter;

    private void Start()
    {
        bam = transform.parent.parent.GetComponent<BoomArmMovement>();
        CollisionCounter = 300;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "hole")
        {
            if (!bam.clamped && bam.fuelAmt < 100)
                bam.AddReward(0.0001f);
            else if (bam.clamped && bam.fuelAmt < 100)
                bam.AddReward(0.0005f);
            else if (bam.fuelAmt >= 100)
                bam.AddReward(-0.0001f);
        }
        else if (other.gameObject.tag == "funnel" || other.gameObject.tag == "fa")
        { }
        else
        {
            CollisionCounter--;
            Debug.Log(CollisionCounter);
        }
        bam.AddReward(-0.000005f);
    }
}
