using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelHose : MonoBehaviour
{
    BoomArmMovement bam;
    private void Start()
    {
        bam = transform.parent.GetComponent<BoomArmMovement>();
    }

    private void OnCollisionStay(Collision collision)
    {
        // if colliding with C5
        if (collision.gameObject.layer == 6)
            bam.AddReward(-0.1f);
    }


}
