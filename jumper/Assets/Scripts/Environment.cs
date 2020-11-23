using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Obstacle obstaclePrefab;
    
    private Jumper jumper;
    private TextMeshPro scoreBoard;
    private GameObject obstacles;

    public void OnEnable()
    {
        obstacles = transform.Find("Obstacles").gameObject;
        scoreBoard = transform.GetComponentInChildren<TextMeshPro>();
        jumper = transform.GetComponentInChildren<Jumper>();        
    }

    private void FixedUpdate()
    {
        scoreBoard.text = jumper.GetCumulativeReward().ToString("f2");
    }

    public void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(obstaclePrefab.gameObject, new Vector3(obstacles.transform.position.x, 0, obstacles.transform.position.z), Quaternion.identity);
        obstacle.transform.SetParent(obstacles.transform);
        Debug.Log(obstacles.transform.localPosition);
    }

    // public Vector3 RandomPosition(float up)
    // {
    //     float x = Random.Range(-9.75f, 9.75f);
    //     float z = Random.Range(-9.75f, 9.75f);
    //
    //     return new Vector3(x, up, z);
    // }

    public void ClearEnvironment()
    {
        foreach (Transform obstacle in obstacles.transform)
        {
            GameObject.Destroy(obstacle.gameObject);
        }
    }
}
