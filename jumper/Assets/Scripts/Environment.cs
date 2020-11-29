using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private const int MAX_SPAWNED_OBJECTS_BEFORE_END_EPISODE = 5;
    public Obstacle obstaclePrefab;
    public Coin coinPrefab;
    
    internal Jumper jumper;
    private TextMeshPro scoreBoard;
    private GameObject objects;
    private int count = 0;

    public void OnEnable()
    {
        objects = transform.Find("Objects").gameObject;
        scoreBoard = transform.GetComponentInChildren<TextMeshPro>();
        jumper = transform.GetComponentInChildren<Jumper>();        
    }

    private void FixedUpdate()
    {
        scoreBoard.text = jumper.GetCumulativeReward().ToString("f2");
    }

    public void SpawnObject()
    {
        if (count >= MAX_SPAWNED_OBJECTS_BEFORE_END_EPISODE)
        {
            jumper.EndEpisode();
            return;
        }
        int randomNumber = Random.Range(0, 2);
        GameObject prefab;
        float yPos = 0;
        if (randomNumber == 0)
        {
            prefab = obstaclePrefab.gameObject;
        }
        else
        {
            prefab = coinPrefab.gameObject;
            yPos = 0.54f;
        }
        GameObject movingObject = Instantiate(prefab, new Vector3(objects.transform.position.x, objects.transform.position.y + yPos, objects.transform.position.z), Quaternion.identity);
        movingObject.transform.SetParent(objects.transform);
        count++;
    }

    public void ClearEnvironment()
    {
        count = 0;
        foreach (Transform obstacle in objects.transform)
        {
            GameObject.Destroy(obstacle.gameObject);
        }
    }
}
