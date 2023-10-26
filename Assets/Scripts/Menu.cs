using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Canvas menu;
    [SerializeField] Canvas gameUI;
    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas failMenu;
    [SerializeField] Canvas winMenu;
    [SerializeField] TMP_Text failText;
    [SerializeField] GameObject pauseBackground;
    [SerializeField] GameObject selector;

    public void OpenMenu()
    {
        if (!menu.enabled)
        {
            menu.enabled = true;
            pauseBackground.SetActive(true);
            GameManager.instance.TimerStatus();
            SoundManager.instance.PlaySound(SoundManager.Sound.buttonPress);
        }
    }
    public void CloseMenu()
    {
        menu.enabled = false;
        failMenu.enabled = false;
        winMenu.enabled = false;
        pauseBackground.SetActive(false);
        GameManager.instance.TimerStatus();
        SoundManager.instance.PlaySound(SoundManager.Sound.buttonPress);
    }
    public void OpenFailMenu(string failMessage)
    {
        failMenu.enabled = true;
        failText.text = failMessage;
        GameManager.instance.TimerStatus();
        pauseBackground.SetActive(true);
    }
    public void LaunchLevel()
    {
        gameUI.enabled = true;
        mainMenu.enabled = false;
        selector.SetActive(true);
        GameManager.instance.StartLevel();
    }
    public void Home()
    {
        GameManager.instance.ClearLevel();
        CloseMenu();
        gameUI.enabled = false;
        mainMenu.enabled = true;
        selector.SetActive(false);
        SoundManager.instance.PlaySound(SoundManager.Sound.buttonPress);
    }
    public void OpenWinMenu()
    {
        winMenu.enabled = true;
        GameManager.instance.TimerStatus();
    }
}
