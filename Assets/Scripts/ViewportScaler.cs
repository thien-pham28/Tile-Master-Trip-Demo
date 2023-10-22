using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class ViewportScaler : MonoBehaviour
{
    private Camera cam;

    [Tooltip("Set the target aspect ratio.")]
    [SerializeField] private float targetAspectRatio;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        if (Application.isPlaying)
            ScaleViewport();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (cam)
            ScaleViewport();
#endif
    }

    private void ScaleViewport()
    {
        // determine the game window's current aspect ratio
        var windowaspect = Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        var scaleheight = windowaspect / targetAspectRatio;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1)
        {
            var rect = cam.rect;

            rect.width = 1;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1 - scaleheight) / 2;

            cam.rect = rect;
        }
        else // add pillarbox
        {
            var scalewidth = 1 / scaleheight;

            var rect = cam.rect;

            rect.width = scalewidth;
            rect.height = 1;
            rect.x = (1 - scalewidth) / 2;
            rect.y = 0;

            cam.rect = rect;
        }
    }
}

/*Todo
 * Need script to spawn tiles randomly accross the board but dont let them collide
 * Script should read the list from scriptable object and spawn each entry
 * The tile prefabs are the same, the list only changes the type + image of the tile
 * Spawned tile can be flipped on its back but avoid this when spawning
 * Need script to hold collected tile, scoring and failure
 * (Upon mouse enter highlight, upon mouse leave unhighlight, upon mouse up select)
 * Use scriptable object to spawn a level includes: Name + Level + Playtime + A list of tiles to spawn (tiles spawn in group of 3s so each entry in the list represents 3 tiles)
 * To cut down on loading use only 1 scene. Upon launching the game spawn the menu + pooled tiles upon each level enable the tiles and move them.
 */