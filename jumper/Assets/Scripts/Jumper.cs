using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Jumper : Agent
{
    public float speed = 10;
    public float jumpForce = 2.0f;

    private Rigidbody body;
    private Environment environment;
    private int menhirCount;
    private bool isOnGround = true;

    public override void Initialize()
    {
        base.Initialize();
        body = GetComponent<Rigidbody>();
        environment = GetComponentInParent<Environment>();
    }

    public override void OnEpisodeBegin()
    {
        // transform.localPosition = new Vector3(0f, 1.5f, 0f);
        // transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
        environment.ClearEnvironment();
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
