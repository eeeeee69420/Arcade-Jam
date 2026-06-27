using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyView : MonoBehaviour
{

    public GameObject startScreen;
    public GameObject inMatchScreen;
    public GameObject gameOverScreen;

    public Image backgroundPlayerOne;
    public TextMeshProUGUI readyTextMeshProPlayerOne;

    public Image backgroundPlayerTwo;
    public TextMeshProUGUI readyTextMeshProPlayerTwo;

    public Image healthPlayerOne;
    public Image healthPlayerTwo;

    public TextMeshProUGUI playerWins;

    public Color backgroundColor = Color.green;

    private void Start()
    {
        startScreen.SetActive(true);
        inMatchScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void SetReady(string player)
    {
        switch (player)
        {
            case "1":
                {
                    backgroundPlayerOne.color = backgroundColor;
                    readyTextMeshProPlayerOne.text = "Player 1 ready!";
                    break;
                }
            case "2":
                {
                    backgroundPlayerTwo.color = backgroundColor;
                    readyTextMeshProPlayerTwo.text = "Player 2 ready!";
                    break;
                }
        }
    }

    public void SetInMatch()
    {
        startScreen.SetActive(false);
        inMatchScreen.SetActive(true);
    }

    public void SetInGameOver(string player)
    {
        startScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        playerWins.text = "Player " + player + " wins!";
    }

    public void UpdatePlayerHealth(string player, float health)
    {
        switch (player)
        {
            case "1":
                {
                    healthPlayerOne.fillAmount = health / 100;
                    break;
                }
            case "2":
                {
                    healthPlayerTwo.fillAmount = health / 300;
                    break;
                }
        }
    }
}