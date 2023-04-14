using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    
    private Rigidbody2D _bulletRb;
    private PlayerMovement _player;
    private float _xSpeed;
    
    void Start()
    {
        _bulletRb = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();
        
        if (_player.GetComponent<SpriteRenderer>().flipX)
        {
            _xSpeed = -bulletSpeed;
        }
        else
        {
            _xSpeed = bulletSpeed;
        }
    }

    void Update()
    {
        _bulletRb.velocity = new Vector2(_xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
