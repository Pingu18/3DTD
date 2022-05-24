using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    private PlayerDataController dataCon;

    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button quitBtn;

    [Header("Settings")]
    [SerializeField] private Slider sensSlider;
    [SerializeField] private TMP_Text sensValueText; 

    private bool menuOpen;
    private bool settingsOpen;

    private void Start()
    {
        dataCon = FindObjectOfType<PlayerDataController>();
        menuOpen = false;
        menuCanvas.gameObject.SetActive(false);
        settingsOpen = false;
        setButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleMenu();
        }

        if (settingsOpen)
        {
            sensValueText.text = sensSlider.value.ToString("F2");
        }
    }

    private void toggleMenu()
    {
        if (!menuOpen)
        {
            menuOpen = true;
            menuCanvas.gameObject.SetActive(true);
            optionsCanvas.gameObject.SetActive(true);
            settingsCanvas.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (menuOpen && !settingsOpen)
        {
            menuOpen = false;
            menuCanvas.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else if (menuOpen && settingsOpen)
        {
            settingsOpen = false;
            optionsCanvas.gameObject.SetActive(true);
            settingsCanvas.gameObject.SetActive(false);
            dataCon.saveSens(sensSlider.value);
        }
        
    }

    private void setButtons()
    {
        settingsBtn.onClick.AddListener(() => openSettingsScreen());
        quitBtn.onClick.AddListener(() => returnToMenu());
    }

    private void openSettingsScreen()
    {
        optionsCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        sensSlider.value = dataCon.getSens();
        settingsOpen = true;
    }

    private void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool getInMenu()
    {
        return menuOpen;
    }
}
