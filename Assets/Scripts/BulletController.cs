using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private Vector2 _direction = Vector2.right;

    public float delayBeforeShrink = 2f;

    public float shrinkDuration = 1f;

    public int speed = 5;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        StartCoroutine(ShrinkRoutine());
    }

    private IEnumerator ShrinkRoutine()
    {
        yield return new WaitForSeconds(delayBeforeShrink);

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shrinkDuration);

            yield return null;
        }

        transform.localScale = targetScale;

        Destroy(gameObject);
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    private void Update()
    {
        _rigidbody2D.linearVelocity = _direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}