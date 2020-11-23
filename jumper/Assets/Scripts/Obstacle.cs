using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    // TODO: uitzoeken of we daar variabelen van kunnen maken
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
        speed = Random.Range(1f, 20f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            Destroy(this);
        }
    }
}
