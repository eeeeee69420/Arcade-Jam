using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerActions : MonoBehaviour
{
    public string playerCount = "1";

    public Vector3 _start;

    public Rigidbody2D _rigidbody;


    public GameObject xObject;

    public Color bulletColor;

    public float spawnInterval = 1.5f;

    public float currentTime = 0f;

    public bool _canUse = true;

    public Transform spawnPoint;

    public PlayerAnimator _playerAnimator;
    public PlayerJump _playerJump;
    public PlayerMovement _playerMovement;
    public LayerMask enemyLayer;
    public SpriteRenderer _spriteRenderer;

    [SerializeField] private MoveData AttackSideData;
    [SerializeField] private MoveData AttackUpData;
    [SerializeField] private MoveData AttackDownData;
    [SerializeField] private MoveData BlockData;
    public MoveData HurtData;

    public float hp = 100f;
    public float hpMax = 100f;
    public bool invincible = false;

    private void Start()
    {
        _start = gameObject.transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerJump = GetComponent<PlayerJump>();
        _playerMovement = GetComponent<PlayerMovement>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        hp = hpMax;
    }

    public virtual void Update()
    {
        switch (GameState.Instance.gameState)
        {

            case GameState.GameStateEnum.GetReady:
                {
                    if (Input.GetButtonDown(GameState.Instance.jumpButton + playerCount))
                    {
                        GameState.Instance.SetReady(playerCount);
                    }
                    break;
                }

            case GameState.GameStateEnum.InMatch:
                {
                    if (!_canUse)
                        return;
                    if (Input.GetButtonDown(GameState.Instance.actionX + playerCount))
                    {
                        _playerAnimator.PlayAnimation("KnightAttackDown");
                        StartCoroutine(AnimationSequence(AttackDownData));
                    }

                    if (Input.GetButtonDown(GameState.Instance.actionB + playerCount))
                    {
                        _playerAnimator.PlayAnimation("KnightAttackUp");
                        StartCoroutine(AnimationSequence(AttackUpData));
                    }

                    if (Input.GetButtonDown(GameState.Instance.actionY + playerCount))
                    {
                        _playerAnimator.PlayAnimation("KnightAttackStraight");
                        StartCoroutine(AnimationSequence(AttackSideData));
                    }
                    if (Input.GetButtonDown(GameState.Instance.actionLB + playerCount) || Input.GetButtonDown(GameState.Instance.actionRB + playerCount))
                    {
                        _playerAnimator.PlayAnimation("KnightBlock");
                        StartCoroutine(AnimationSequence(BlockData));
                    }

                    break;
                }

            case GameState.GameStateEnum.GameOver:
                {
                    if (Input.GetButtonDown(GameState.Instance.jumpButton + playerCount))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    break;
                }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public virtual IEnumerator AnimationSequence(MoveData moveData)
    {
        _canUse = false;
        if (moveData.freezeMovement)
        {
            _playerJump.canMove = false;
            _playerMovement.canMove = false;
        }
        if (moveData.isInvincible)
        {
            invincible = true;
        }
        foreach (var moveFrame in moveData.moveFrames)
        {
            for (int i = 0; i < 30; i++)
                yield return new WaitForEndOfFrame();
            RunFrame(moveFrame);
        }
        if (moveData.freezeMovement)
        {
            _playerJump.canMove = true;
            _playerMovement.canMove = true;
        }
        if (moveData.isInvincible)
        {
            invincible = false;
        }
        yield return new WaitForSeconds(.2f);
        _canUse = true;

    }
    public virtual void RunFrame(MoveFrame moveFrame)
    {
        if (moveFrame.moveHitbox != null)
        {
            Collider2D enemyHit = Hitbox(moveFrame.moveHitbox);
            if (enemyHit != null)
            {
                PlayerActions enemy = enemyHit.GetComponent<PlayerActions>();
                if (!enemy.invincible)
                {
                    enemy.TakeDamage(moveFrame.moveHitbox.damage);
                }
            }
        }
    }
    public virtual void TakeDamage(float damage)
    {
        StopAllCoroutines();
        _playerAnimator.PlayAnimation("KnightHurt");
        StartCoroutine(AnimationSequence(HurtData));
        hp -= damage;
        GameState.Instance.TakeDamage(playerCount, hp);
        if (hp <= 0)
        {
            _playerAnimator.PlayAnimation("KnightDeath");
            invincible = true;
            _playerJump.canMove = false;
            _playerMovement.canMove = false;
            _canUse = false;
        }
    }
    public Vector2 _gizmoPosition;
    public Vector2 _gizmoSize;
    public bool _drawGizmo;
    public bool _gizmoHit;

    public virtual Collider2D Hitbox(MoveHitbox moveHitbox)
    {
        Vector2 position = (Vector2)transform.position + new Vector2(
            _spriteRenderer.flipX ? -moveHitbox.hitboxOffset.x : moveHitbox.hitboxOffset.x,
            moveHitbox.hitboxOffset.y
        );

        Collider2D hit = Physics2D.OverlapBox(position, moveHitbox.hitboxSize, 0f, enemyLayer);

        _gizmoPosition = position;
        _gizmoSize = moveHitbox.hitboxSize;
        _gizmoHit = hit;
        _drawGizmo = true;
        Debug.Log(hit);
        return hit;
    }

    public virtual void OnDrawGizmos()
    {
        if (!_drawGizmo) return;
        Gizmos.color = _gizmoHit ? Color.red : Color.green;
        Gizmos.DrawWireCube(_gizmoPosition, _gizmoSize);
    }

    [System.Serializable]
    public class MoveData
    {
        public bool freezeMovement;
        public bool isInvincible;
        public List<MoveFrame> moveFrames = new();
    }
    [System.Serializable]
    public class MoveFrame
    {
        public MoveHitbox moveHitbox;
    }
    [System.Serializable]
    public class MoveHitbox
    {
        public Vector2 hitboxOffset;
        public Vector2 hitboxSize;
        public float damage;
    }
}