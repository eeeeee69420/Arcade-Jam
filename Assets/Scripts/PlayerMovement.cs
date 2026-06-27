using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5;

    private Rigidbody2D _rigidbody2D;

    private PlayerActions _playerActions;

    private PlayerAnimator _playerAnimator;
    private SpriteRenderer _spriteRenderer;
    private PlayerJump _playerJump;
    public bool canMove = true;
    private float horizontal;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerActions = GetComponent<PlayerActions>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerJump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        if (GameState.Instance.gameState != GameState.GameStateEnum.InMatch) return;
        if (_playerJump.groundCheck)
        {
            horizontal = 0f;
            if (canMove)
            {
                horizontal = Input.GetAxisRaw(GameState.Instance.horizontalAxis + _playerActions.playerCount);
            }
        }
        else
        {
            if (canMove)
            {
                horizontal = Input.GetAxisRaw(GameState.Instance.horizontalAxis + _playerActions.playerCount) / 1.3f;
            }
        }

            bool isMoving = Mathf.Abs(horizontal) > 0.04f;
        if (isMoving)
        {
            if (_playerActions.playerCount == "1")
            {
                if (horizontal > 0)
                    _spriteRenderer.flipX = false;
                else if (horizontal < 0)
                    _spriteRenderer.flipX = true;
            }
            else
            {
                if (horizontal > 0)
                    _spriteRenderer.flipX = true;
                else if (horizontal < 0)
                    _spriteRenderer.flipX = false;
            }
        }
        _playerAnimator.animator.SetBool("IsMoving", isMoving);
        _rigidbody2D.linearVelocity = new Vector2(horizontal * speed, _rigidbody2D.linearVelocity.y);
    }
}