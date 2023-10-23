using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [Header("Tile Values")]
    private string type;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Outline outline;

    public void SetType(string t)
    {
        type = t;
    }
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
    private void OnMouseEnter()
    {
        outline.enabled = true;
        transform.localScale = new Vector3(1.05f, 0.55f, 1.55f);
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
        transform.localScale = new Vector3(1f, 0.5f, 1.5f);
    }
}
