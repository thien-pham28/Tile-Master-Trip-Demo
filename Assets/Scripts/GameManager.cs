using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    //Make it a singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadData(GameData data)
    {
        totalStars = data.stars;
        level = data.level;
    }

    public void SaveData(ref GameData data)
    {
        data.stars = totalStars;
        data.level = level;
    }

    [Header("Level Manager")]
    [SerializeField] private LevelData[] levelData;
    [SerializeField] private float comboTime;
    private float currentComboTime = 0;
    private int combo = 0;
    private int levelStars = 0;
    private int totalStars = 0;
    private int tileCount = 0;
    private int level = 0;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject comboBar;
    [SerializeField] private Slider comboSlider;
    [SerializeField] private TMP_Text comboValue;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text starCount;
    [SerializeField] private TMP_Text winStarCount;
    [SerializeField] private TMP_Text mainLevelText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject starCounter;
    [SerializeField] private TMP_Text totalStarsText;
    private float timer;
    private bool timerIsRunning = false;


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

    }
    public void TimerStatus()
    {
        timerIsRunning = !timerIsRunning;
    }
    private void Update()
    {
        if (timerIsRunning)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (currentComboTime > 0)
                {
                    currentComboTime -= Time.deltaTime;
                    comboSlider.value = currentComboTime / comboTime;
                    comboValue.text = "Combo x " + combo;
                }
                else
                {
                    combo = 0;
                    comboBar.SetActive(false);
                }
                DisplayTime(timer);
            }
            else
            {
                timer = 0;
                timerIsRunning = false;
                GetComponent<Menu>().OpenFailMenu("TIME'S UP!");
                SoundManager.instance.PlaySound(SoundManager.Sound.levelFail);
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void SpawnTiles(LevelData levelData)
    {
        //Play sound
        SoundManager.instance.PlaySound(SoundManager.Sound.tileDrop);
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
                    //Use breaker in case infinite loop
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
                SoundManager.instance.PlaySound(SoundManager.Sound.tileSelect);
                selectedTile.Insert(i + 1, newTile);
                StartCoroutine(newTile.Shift(slotLocations[i + 1], Quaternion.identity, moveSpeed));
                //Check if the next tile is also of the same type (3 matching tiles), if so, delete all 3 and add score
                if (i > 0)
                    if (selectedTile[i - 1].GetTileType() == newTile.GetTileType())
                    {
                        for (int k = i - 1; k < i + 2; k++)
                        {
                            selectedTile[k].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                            selectedTile[k].gameObject.GetComponent<BoxCollider>().enabled = true;
                            selectedTile[k].gameObject.SetActive(false);
                        }
                        selectedTile.RemoveRange(i - 1, 3);
                        SoundManager.instance.PlaySound(SoundManager.Sound.tileComplete);
                        //Shift all the other tiles back
                        for (int j = 0; j < selectedTile.Count; j++)
                        {
                            Debug.Log("reset");
                            StartCoroutine(selectedTile[j].Shift(slotLocations[j], Quaternion.identity, moveSpeed));
                        }
                        levelStars += combo + 1;
                        starCount.text = levelStars.ToString();
                        comboBar.SetActive(true);
                        currentComboTime = comboTime;
                        combo++;
                        tileCount--;
                        if (tileCount <= 0)
                        {
                            //Level completed
                            level++;
                            totalStars += levelStars;
                            totalStarsText.text = totalStars.ToString();
                            winStarCount.text = levelStars.ToString();
                            GetComponent<Menu>().OpenWinMenu();
                            SoundManager.instance.PlaySound(SoundManager.Sound.levelComplete);
                        }
                    }
                //break out of loop
                break;
            }
            //If any tile does not match new tile shift it right
            else
            {
                StartCoroutine(selectedTile[i].Shift(slotLocations[i + 1], Quaternion.identity, moveSpeed));
            }
        }
        //If no tile matches after loop add tile to top of list
        if (!tileAdded)
        {
            selectedTile.Insert(0, newTile);
            SoundManager.instance.PlaySound(SoundManager.Sound.tileSelect);
            StartCoroutine(newTile.Shift(slotLocations[0], Quaternion.identity, moveSpeed));
        }
        //After loop if list count is 7, fail the game
        if (selectedTile.Count >= 7)
        {
            GetComponent<Menu>().OpenFailMenu("OUT OF SLOTS");
            SoundManager.instance.PlaySound(SoundManager.Sound.levelFail);
        }
    }
    public void StartLevel()
    {
        SpawnTiles(levelData[level]);
        tileCount = 0;
        foreach (int count in levelData[level].number)
            tileCount += count;
        timer = levelData[level].playTime;
        levelText.text = levelData[level].displayName;
        timerIsRunning = true;
    }
    public void ClearLevel()
    {
        for (int i = 0; i < selectedTile.Count; i++)
        {
            selectedTile[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            selectedTile[i].GetComponent<BoxCollider>().enabled = true;
        }
        selectedTile.Clear();
        tilePool.DeactivatePooledObject();
        combo = 0;
        currentComboTime = 0;
    }
    public void Restart()
    {
        ClearLevel();
        GetComponent<Menu>().CloseMenu();
        StartLevel();
    }
    public void DoneLoading()
    {
        playButton.SetActive(true);
        starCounter.SetActive(true);
        mainLevelText.text = "LEVEL\n" + levelData[level].level;
        totalStarsText.text = totalStars.ToString();
    }
}
