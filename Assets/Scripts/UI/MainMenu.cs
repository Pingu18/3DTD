using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        playBtn.onClick.AddListener(() => SceneManager.LoadScene(1));
        quitBtn.onClick.AddListener(() => Application.Quit());
    }
}
