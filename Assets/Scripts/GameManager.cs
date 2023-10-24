using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Make it a singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    [Header("Level Manager")]
    [SerializeField] private LevelData[] levelData;
    [SerializeField] private float startUpTime;

    [Header("Selector")]
    [SerializeField] private Vector3[] slotLocations;
    [SerializeField] private float moveSpeed;
    private List<TileScript> selectedTile = new List<TileScript>();

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
        SpawnTiles(levelData[1]);
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
    public void SelectTile(TileScript newTile)
    {
        //Add tile to list
        bool tileAdded = false;
        //Loop list from bottom to top
        for (int i = selectedTile.Count - 1; i >= 0; i--)
        {
            //If a tile matches add new tile right after the matching tile
            if (selectedTile[i].GetTileType() == newTile.GetTileType())
            {
                tileAdded = true;
                Debug.Log("TileAdded");
                selectedTile.Insert(i + 1, newTile);
                StartCoroutine(newTile.Shift(slotLocations[i + 1], Quaternion.identity, moveSpeed));
                //Check if the next tile is also of the same type (3 matching tiles), if so, delete all 3 and add score
                if (i > 0)
                    if (selectedTile[i - 1].GetTileType() == newTile.GetTileType())
                    {
                        selectedTile[i - 1].gameObject.SetActive(false);
                        selectedTile[i].gameObject.SetActive(false);
                        newTile.gameObject.SetActive(false);
                        selectedTile.RemoveRange(i - 1, 3);
                        //Shift all the other tiles back
                        for (int j = 0; j < selectedTile.Count; j++)
                        {
                            Debug.Log("reset");
                            StartCoroutine(selectedTile[j].Shift(slotLocations[j], Quaternion.identity, moveSpeed));
                        }
                    }
                //break out of loop
                break;
            }
            //If any tile does not match new tile shift it right
            else
            {
                Debug.Log("Shifting to right");
                StartCoroutine(selectedTile[i].Shift(slotLocations[i + 1], Quaternion.identity, moveSpeed));
            }
        }
        //If no tile matches after loop add tile to top of list
        if (!tileAdded)
        {
            selectedTile.Insert(0, newTile);
            StartCoroutine(newTile.Shift(slotLocations[0], Quaternion.identity, moveSpeed));
        }
        //After loop if list count is 7, fail the game

    }
}
