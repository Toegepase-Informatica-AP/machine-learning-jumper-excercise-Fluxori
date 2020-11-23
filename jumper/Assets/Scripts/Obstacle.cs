using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    private float speed = 1f;
    private Environment environment;

    private void Start()
    {
        SetRandomSpeed();
    }

    private void FixedUpdate()
    {
        Move(speed);
    }

    private void Move(float speed)
    {
        if (environment == null)
        {
            environment = GetComponentInParent<Environment>();
        }

        Vector3 moveVector = speed * -transform.forward * Time.fixedDeltaTime;
        transform.localPosition += moveVector;
    }

    private void SetRandomSpeed()
    {
        speed = Random.Range(4f, 8f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            environment.jumper.AddReward(1f);
            environment.SpawnObstacle();
            Destroy(this.gameObject);
        }
    }
}
