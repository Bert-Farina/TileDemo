using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    private bool _isAlive = true;

    [SerializeField] private float runSpeed = 2.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float climbSpeed = 4.0f;
    [SerializeField] private Vector2 deathKick = new Vector2(10.0f, 10.0f);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
    private static readonly int Dying = Animator.StringToHash("Dying");

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
        if (!_isAlive) return;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
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
        if (!_isAlive) return;
        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!_isAlive) return;
        if (!_playerFeetCollider.IsTouchingLayers(LayerMask.GetMask($"Ground"))) return;
        if (value.isPressed)
        {
            _rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        if (!_isAlive) return;
        Instantiate(bullet, gun.position, transform.rotation);
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

    private void Die()
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) && _playerSprite.flipX)
        {
            _isAlive = false;
            _playerAnimator.SetTrigger(Dying);
            _rb2d.velocity = deathKick;
        }else if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) && !_playerSprite.flipX)
        {
            _isAlive = false;
            _playerAnimator.SetTrigger(Dying);
            _rb2d.velocity = new Vector2(-deathKick.x, deathKick.y);
        }
        else if(_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            _isAlive = false;
            _playerAnimator.SetTrigger(Dying);
        }
    }
}
