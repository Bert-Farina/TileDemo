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
    private CapsuleCollider2D _playerBodyCollider;
    private BoxCollider2D _playerFeetCollider;
    private bool _playerHasHorizontalSpeed;
    private bool _playerHasVerticalSpeed;
    private float _startingGravity;

    [SerializeField] private float runSpeed = 2.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float climbSpeed = 4.0f;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _playerBodyCollider = GetComponent<CapsuleCollider2D>();
        _playerFeetCollider = GetComponent<BoxCollider2D>();

        _startingGravity = _rb2d.gravityScale;
    }

    // Update is called once per frame
    private void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }
    
    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * runSpeed, _rb2d.velocity.y);
        _rb2d.velocity = playerVelocity;
        
        _playerHasHorizontalSpeed = Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
        _playerAnimator.SetBool(IsRunning, _playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        if (!_playerHasHorizontalSpeed) return;
        _playerSprite.flipX = Math.Sign(_rb2d.velocity.x) == -1;
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        //Debug.Log(_moveInput);
    }

    private void OnJump(InputValue value)
    {
        if (!_playerFeetCollider.IsTouchingLayers(LayerMask.GetMask($"Ground"))) return;
        if (value.isPressed)
        {
            _rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void ClimbLadder()
    {
        if (!_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask($"Ladder")))
        {
            _rb2d.gravityScale = _startingGravity;
            _playerAnimator.SetBool(IsClimbing, false);
            return;
        }
        
        Vector2 climbVelocity = new Vector2(_rb2d.velocity.x, _moveInput.y * climbSpeed);
        _rb2d.velocity = climbVelocity;
        _rb2d.gravityScale = 0f;
        
        _playerHasVerticalSpeed = Mathf.Abs(_rb2d.velocity.y) > Mathf.Epsilon;
        _playerAnimator.SetBool(IsClimbing, _playerHasVerticalSpeed);
    }

}
