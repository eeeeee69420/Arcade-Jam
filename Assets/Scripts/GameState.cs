using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    private ReadyView _readyView;

    public PlayerActions player1;
    public PlayerActions player2;


    private bool _playerOneReady = false;
    private bool _playerTwoReady = false;

    public string horizontalAxis = "Horizontal_";
    public string verticalAxis = "Vertical_";
    public string jumpButton = "Jump_";
    public string actionX = "Action_X_";
    public string actionB = "Action_B_";
    public string actionY = "Action_Y_";
    public string actionRB = "Action_RB_";
    public string actionLB = "Action_LB_";
    private bool ending;

    public enum GameStateEnum
    {
        GetReady,
        InMatch,
        GameOver,
    }

    public GameStateEnum gameState;


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gameState = GameStateEnum.GetReady;

        _readyView = gameObject.GetComponent<ReadyView>();
    }

    private void Update()
    {
        switch (gameState)
        {

            case GameStateEnum.GetReady:
                {
                    if (_playerOneReady && _playerTwoReady)
                    {
                        gameState = GameStateEnum.InMatch;

                        _readyView.SetInMatch();
                    }
                    break;
                }

            case GameStateEnum.InMatch:
                {
                    if (player1.hp <= 0 || player2.hp <= 0 && ending == false)
                    {
                        StartCoroutine(WaitForDeath());
                    }
                    break;
                }
            case GameStateEnum.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(2);
        EndGame();
    }
    public void EndGame()
    {
        ending = false;
        gameState = GameStateEnum.GameOver;

        _readyView.SetInGameOver(player1.hp <= 0 ? "2" : "1");
    }

    public void TakeDamage(string player, float health)
    {
        switch (player)
        {
            case "1":
                {
                    _readyView.UpdatePlayerHealth(player, health);
                    break;
                }
            case "2":
                {
                    _readyView.UpdatePlayerHealth(player, health);
                    break;
                }
        }
    }

    public void SetReady(string player)
    {
        switch (player)
        {
            case "1":
                {
                    _playerOneReady = true;
                    _readyView.SetReady(player);
                    break;
                }
            case "2":
                {
                    _playerTwoReady = true;
                    _readyView.SetReady(player);
                    break;
                }
        }
    }
}