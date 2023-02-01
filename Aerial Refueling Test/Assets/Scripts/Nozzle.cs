using UnityEngine;
public class Nozzle : MonoBehaviour
{
    /// <summary>
    /// The boom arm movement script.
    /// </summary>
    BoomArmMovement bam;
    /// <summary>
    /// The collision counter.
    /// </summary>
    public int CollisionCounter;

    private void Start()
    {
        // get the boom arm movement script
        bam = transform.parent.parent.GetComponent<BoomArmMovement>();
        // set the collision counter to 300
        CollisionCounter = 300;
    }

    /// <summary>
    /// If nozzle is colliding with hole, add good reward. If nozzle is colliding with funnel, do nothing, else add bad reward.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        // if colliding with hole
        if (other.gameObject.tag == "hole")
        {
            // if not clamped and fuel is less than 100, add good reward
            if (!bam.clamped && bam.fuelAmt < 100)
                bam.AddReward(0.0001f);
            // if clamped and fuel is less than 100, add good reward
            else if (bam.clamped && bam.fuelAmt < 100)
                bam.AddReward(0.0005f);
            // if fuel is greater than or equal to 100, add bad reward to incentivize unclamping once fueled
            else if (bam.fuelAmt >= 100)
                bam.AddReward(-0.0001f);
        }
        // if colliding with funnel, just ignore, no bad or good reward
        else if (other.gameObject.tag == "funnel" || other.gameObject.tag == "fa")
        { }
        // else you are colliding with something else that is incorrect, reduce nozzle counter to "act like you are damaging the nozzle"
        else
        {
            CollisionCounter--;
        }
        // add bad reward if not doing anything close to the hole
        bam.AddReward(-0.000005f);
    }
}
