using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesController : MonoBehaviour
{
    [SerializeField] private TMP_Text PlayerLivesText;
    [SerializeField] private TMP_Text EnemiesLeftText;
    private int livesLeft;
    private bool gameOver = false;

    private void Start()
    {
        livesLeft = 100;
        UpdatePlayerLives(livesLeft);
    }

    public void DecrementPlayerLives()
    {
        livesLeft--;
        if (livesLeft <= 0)
        {
            gameOver = true;
            UpdatePlayerLives(0);
        }
        else
        {
            UpdatePlayerLives(livesLeft);
        }
    }

    private void UpdatePlayerLives(int lives)
    {
        PlayerLivesText.text = lives.ToString() + " ♥";
    }
    public void UpdateEnemiesLeft(int enemies)
    {
        EnemiesLeftText.text = enemies.ToString() + " Enemies";
    }
}
