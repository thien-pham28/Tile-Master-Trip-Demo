using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] Canvas menu;
    [SerializeField] GameObject pauseBackground;
    public void OpenMenu()
    {
        if (!menu.enabled)
        {
            menu.enabled = true;
            pauseBackground.SetActive(true);
        }
    }
    public void CloseMenu()
    {
        menu.enabled = false;
        pauseBackground.SetActive(false);
    }
}
