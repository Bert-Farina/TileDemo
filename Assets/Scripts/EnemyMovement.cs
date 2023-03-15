using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float enemyMoveSpeed = 1f;
    
    private Rigidbody2D _enemyRigidbody;
    private SpriteRenderer _enemySprite;
    private BoxCollider2D _enemyTurnCollider;

    private void Start()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _enemySprite = GetComponent<SpriteRenderer>();
        _enemyTurnCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _enemyRigidbody.velocity = new Vector2(enemyMoveSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_enemyTurnCollider.IsTouchingLayers(LayerMask.GetMask($"Ground"))) return;
        enemyMoveSpeed = -enemyMoveSpeed;
        FlipEnemy();
    }

    private void FlipEnemy()
    {
        _enemySprite.flipX = Math.Sign(_enemyRigidbody.velocity.x) == 1;
    }
}
