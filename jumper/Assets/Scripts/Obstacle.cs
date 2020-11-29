using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MovingObject
{
    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            environment.jumper.AddReward(1f);
            environment.SpawnObject();
            Destroy(this.gameObject);
        }
    }
}
