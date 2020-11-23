using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Jumper : Agent
{
    public float jumpForce = 5f;

    private Rigidbody body;
    private Environment environment;
    private bool isOnGround = true;

    public override void Initialize()
    {
        base.Initialize();
        body = GetComponent<Rigidbody>();
        environment = GetComponentInParent<Environment>();
        InvokeRepeating(nameof(AddOnGroundReward), 0, 1.0f);
    }

    public override void OnEpisodeBegin()
    {
        environment.ClearEnvironment();
        environment.SpawnObstacle();
        
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
    }

    private void AddOnGroundReward()
    {
        if (isOnGround)
        {
            AddReward(0.01f); 
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(isOnGround);

        // TODO: zien of we hier een reward moeten zetten (0.1 om te blijven "leven")
        // if (transform.localPosition.y < 0)
        // {
        //     AddReward(-1f);
        //     EndEpisode();
        // }
    }
    
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f;
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1f;
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (vectorAction[0] == 1 && isOnGround)
        {
            Jump();
        }
    }

    public void Jump()
    {
        body.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        isOnGround = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        if (other.transform.CompareTag("Obstacle"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
