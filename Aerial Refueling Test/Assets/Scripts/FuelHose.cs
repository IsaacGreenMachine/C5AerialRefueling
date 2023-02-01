using UnityEngine;

public class FuelHose : MonoBehaviour
{
    /// <summary>
    /// The boom arm movement script.
    /// </summary>
    BoomArmMovement bam;
    private void Start()
    {
        // get the boom arm movement script
        bam = transform.parent.GetComponent<BoomArmMovement>();
    }


    // If the fuel hose is colliding with the C5 and not the fuel hole/funnel, add bad reward.
    private void OnCollisionStay(Collision collision)
    {
        // if colliding with C5
        if (collision.gameObject.layer == 6)
            bam.AddReward(-0.00005f);
    }
}
