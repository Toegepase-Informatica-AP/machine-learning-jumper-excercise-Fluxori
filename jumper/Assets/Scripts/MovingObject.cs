using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    private float speed = 1f;
    internal Environment environment;

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

    public abstract void OnTriggerExit(Collider other);
}
