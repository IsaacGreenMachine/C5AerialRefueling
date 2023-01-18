using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozzle : MonoBehaviour
{
    BoomArmMovement bam;
    private void Start()
    {
        bam = transform.parent.parent.GetComponent<BoomArmMovement>();
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
        else if (other.gameObject.tag == "funnel")
        { }
        else
            bam.AddReward(-0.000005f);
    }
}
