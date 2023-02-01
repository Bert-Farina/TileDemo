using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveInput;
    private Rigidbody2D _rb2d;
    private SpriteRenderer _playerSprite;
    private Animator _playerAnimator;
    private bool _playerHasSpeed;
    
    [SerializeField] private float runSpeed = 2.0f;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Run();
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (!_playerHasSpeed) return;
        _playerSprite.flipX = Math.Sign(_rb2d.velocity.x) == -1;
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log(_moveInput);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * runSpeed, _rb2d.velocity.y);
        _rb2d.velocity = playerVelocity;
        
        _playerHasSpeed = Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
        _playerAnimator.SetBool(IsRunning, _playerHasSpeed);
    }
}
