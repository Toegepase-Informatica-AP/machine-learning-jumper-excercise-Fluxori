using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MovingObject
{
    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            environment.jumper.AddReward(-1f);
            environment.SpawnObject();
            Destroy(this.gameObject);
        }
    }
}
