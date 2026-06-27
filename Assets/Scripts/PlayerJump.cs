using System.Collections;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private PlayerActions _playerActions;

    public float capsuleHeight = 0.25f;
    public float capsuleRadius = 0.08f;

    public Transform feetCollider;

    public LayerMask groundMask;

    public bool groundCheck;

    public float jumpForce = 10;

    public float fallForce = 2;

    private Vector2 _gravityVector;

    private PlayerAnimator _playerAnimator;

    public bool canMove = true;

    private void Start()
    {
        _gravityVector = new Vector2(0, Physics2D.gravity.y);

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerActions = GetComponent<PlayerActions>();
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        if (GameState.Instance.gameState != GameState.GameStateEnum.InMatch) return;

        groundCheck = Physics2D.OverlapCapsule(feetCollider.position,
     new Vector2(capsuleHeight, capsuleRadius), CapsuleDirection2D.Horizontal,
     0, groundMask);

        _playerAnimator.animator.SetBool("TouchingGround", groundCheck && Mathf.Abs(_rigidbody2D.linearVelocityY) < 0.04);
        if (Input.GetButtonDown(GameState.Instance.jumpButton + _playerActions.playerCount) && groundCheck && canMove)
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);
            if (_playerActions.playerCount == "1")
            {
                _playerAnimator.PlayAnimation("KnightJump");
            }
            StartCoroutine(StartJumpSequence());
        }

        if (_rigidbody2D.linearVelocity.y < 0)
        {
            _rigidbody2D.linearVelocity += _gravityVector * (fallForce * Time.deltaTime);
        }
    }
    private IEnumerator StartJumpSequence()
    {
        _playerAnimator.animator.SetBool("IsJumping", true);
        yield return new WaitForSeconds(.2f);
        _playerAnimator.animator.SetBool("IsJumping", false);
    }
}