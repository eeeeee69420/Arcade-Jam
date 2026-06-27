using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.EventSystems.EventTrigger;

public class DragonActions : PlayerActions
{
    public MoveData AttackData;
    public MoveData FlameData;
    public MoveData ChargeData;
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

    public override void Update()
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
                        _playerAnimator.PlayAnimation("DragonClaw");
                        StartCoroutine(AnimationSequence(AttackData));
                    }

                    if (Input.GetButtonDown(GameState.Instance.actionB + playerCount))
                    {
                        _playerAnimator.PlayAnimation("DragonFire");
                        StartCoroutine(AnimationSequence(FlameData));
                    }

                    if (Input.GetButtonDown(GameState.Instance.actionY + playerCount))
                    {
                    }
                    if (Input.GetButtonDown(GameState.Instance.actionLB + playerCount) || Input.GetButtonDown(GameState.Instance.actionRB + playerCount))
                    {
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

    public override IEnumerator AnimationSequence(MoveData moveData)
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
            yield return new WaitForSeconds(1f);
        _canUse = true;

    }
    public override void RunFrame(MoveFrame moveFrame)
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
    public override void TakeDamage(float damage)
    {
        StopAllCoroutines();
        _playerAnimator.PlayAnimation("DragonHurt");
        StartCoroutine(AnimationSequence(HurtData));
        hp -= damage;
        GameState.Instance.TakeDamage(playerCount, hp);
        if (hp <= 0)
        {
            _playerAnimator.PlayAnimation("DragonDeath");
            invincible = true;
            _playerJump.canMove = false;
            _playerMovement.canMove = false;
            _canUse = false;
        }
    }

    public override Collider2D Hitbox(MoveHitbox moveHitbox)
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

    public override void OnDrawGizmos()
    {
        if (!_drawGizmo) return;
        Gizmos.color = _gizmoHit ? Color.red : Color.green;
        Gizmos.DrawWireCube(_gizmoPosition, _gizmoSize);
    }
}