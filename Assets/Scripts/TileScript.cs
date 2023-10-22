using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [Header("Tile Values")]
    private string type;
    [SerializeField] SpriteRenderer spriteRenderer;

    public void SetType(string t)
    {
        type = t;
    }
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
