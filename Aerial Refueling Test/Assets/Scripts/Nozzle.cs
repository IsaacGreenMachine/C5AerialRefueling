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
            bam.AddReward(5f);
        else if (other.gameObject.tag == "funnel")
        { }
        else
            bam.AddReward(-1f);
    }
}
