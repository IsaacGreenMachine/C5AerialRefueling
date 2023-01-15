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
    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.tag == "funnel")
            Debug.Log(collision);
        // bam.AddReward();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "hole")
            bam.AddReward(1f);
        else if (other.gameObject.tag == "funnel")
        { }
        else
            bam.AddReward(-0.1f);
    }
}
