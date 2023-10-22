using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Manager")]
    [SerializeField] private LevelData[] levelData;

    [Header("Tile Spawner")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private ObjectPooling tilePool;
    [SerializeField] private Vector3 spawnAreaMax; //Max values of the spawn area
    [SerializeField] private Vector3 spawnAreaMin; //Min values of the spawn area
    [SerializeField] private Vector3 spawnRotationMax; //Max values of the spawn rotation
    [SerializeField] private Vector3 spawnRotationMin; //Min values of the spawn rotation

    // Start is called before the first frame update
    void Start()
    {
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
                //Spawn 3 for every number
                for (int count = 0; count < 3;)
                {
                    //Generate spawn point and rotation
                    Vector3 spawnPoint = new Vector3(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y), Random.Range(spawnAreaMin.z, spawnAreaMax.z));
                    Quaternion spawnRotation = Quaternion.Euler(new Vector3(Random.Range(spawnRotationMin.x, spawnRotationMax.x), Random.Range(spawnRotationMin.y, spawnRotationMax.y), Random.Range(spawnRotationMin.z, spawnRotationMax.z)));
                    //Check if spawn location collides with anything
                    if (!Physics.CheckBox(spawnPoint, tilePrefab.transform.localScale, spawnRotation))
                    {
                        //If no collision, spawn and increment loop
                        GameObject tile = tilePool.GetPooledObject(spawnPoint, spawnRotation);
                        //Set sprite and type
                        tile.GetComponent<TileScript>().SetSprite(levelData.tileSprite[type]);
                        tile.GetComponent<TileScript>().SetType(levelData.type[type]);
                        count++;
                    }
                }
            }
        }
    }
}
