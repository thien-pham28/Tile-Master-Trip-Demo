using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Manager")]
    [SerializeField] private LevelData[] levelData;
    [SerializeField] private float startUpTime;

    [Header("Tile Spawner")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private ObjectPooling tilePool;
    [SerializeField] private Vector3 spawnAreaMax; //Max values of the spawn area
    [SerializeField] private Vector3 spawnAreaMin; //Min values of the spawn area
    [SerializeField] private int spawnRotationMax; //Max y value of the spawn rotation
    [SerializeField] private int spawnRotationMin; //Min y value of the spawn rotation
    private void Start()
    {
        StartCoroutine(InitialWait(startUpTime));
    }

    IEnumerator InitialWait(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnTiles(levelData[0]);
    }
    void SpawnTiles(LevelData levelData)
    {
        //Iterate through all tile types
        for (int type = 0; type < levelData.tileTypeCount; type++)
        {
            //Loop for the number of each type
            for (int number = 0; number < levelData.number[type]; number++)
            {
                int breaker = 0;
                //Spawn 3 for every number
                for (int count = 0; count < 3;)
                {
                    if (breaker == 50)
                    {
                        Debug.Log("Breaker");
                        break;
                    }             
                    //Generate spawn point and rotation
                    Vector3 spawnPoint = new Vector3(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y), Random.Range(spawnAreaMin.z, spawnAreaMax.z));
                    Quaternion spawnRotation = Quaternion.Euler(new Vector3(0, Random.Range(spawnRotationMin, spawnRotationMax), 0));
                    //Check if spawn location collides with anything
                    if (!Physics.CheckBox(spawnPoint, tilePrefab.transform.localScale / 2, spawnRotation))
                    {
                        //If no collision, spawn and increment loop
                        GameObject tile = tilePool.GetPooledObject(spawnPoint, spawnRotation);
                        //Set sprite and type
                        tile.GetComponent<TileScript>().SetSprite(levelData.tileSprite[type]);
                        tile.GetComponent<TileScript>().SetType(levelData.type[type]);
                        count++;
                        breaker = 0;
                    }
                    breaker++;
                }
            }
        }
    }
}
