using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform positionRight;
    public Transform positionLeft;
    public Transform positionTop;
    public Transform positionBottom;

    private PlayerActions _playerActions;

    public Vector2 direction = Vector2.right;

    private void Start()
    {
        _playerActions = GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if (GameState.Instance.gameState != GameState.GameStateEnum.InMatch) return;

        int horizontal = (int)Input.GetAxisRaw(GameState.Instance.horizontalAxis + _playerActions.playerCount);
        int vertical = (int)Input.GetAxisRaw(GameState.Instance.verticalAxis + _playerActions.playerCount);

        switch (horizontal)
        {
            case 0 when vertical == 0:
                {
                    break;
                }
            case 0 when vertical == 1:
                {
                    direction = Vector2.up;
                    break;
                }
            case 0 when vertical == -1:
                {
                    direction = Vector2.down;
                    break;
                }
            case 1 when vertical == 0:
                {
                    direction = Vector2.right;
                    break;
                }
            case -1 when vertical == 0:
                {
                    direction = Vector2.left;
                    break;
                }
        }
    }
}