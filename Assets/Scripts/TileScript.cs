using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            outline.enabled = true;
            transform.localScale = new Vector3(0.85f, 0.3f, 1.05f);
        }
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
        transform.localScale = new Vector3(0.8f, 0.25f, 1f);
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && outline.enabled && !EventSystem.current.IsPointerOverGameObject())
            {
                GameManager.instance.SelectTile(this);
                outline.enabled = false;
            }
        }
    }
    public string GetTileType()
    {
        return type;
    }
    public IEnumerator Shift(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<BoxCollider>().enabled = false;
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        Debug.Log("Finished Lerping");
    }
}
