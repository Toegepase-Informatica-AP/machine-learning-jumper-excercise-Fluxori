using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Obstacle obstaclePrefab;
    
    internal Jumper jumper;
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

    public void ClearEnvironment()
    {
        foreach (Transform obstacle in obstacles.transform)
        {
            GameObject.Destroy(obstacle.gameObject);
        }
    }
}
