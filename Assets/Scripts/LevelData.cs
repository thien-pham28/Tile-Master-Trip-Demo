using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int level;
    public string displayName;
    public float playTime;
    public int tileTypeCount;
    public string[] type;
    public Sprite[] tileSprite;
    public int[] number; //How many of that tile is spawned
}