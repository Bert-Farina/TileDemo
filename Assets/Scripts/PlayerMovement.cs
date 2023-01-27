using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveInput;
    private Rigidbody2D _rb2d;
    private SpriteRenderer _playerSprite;
    
    [SerializeField] private float runSpeed = 2.0f; 
    
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        Run();
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (_rb2d.velocity.x < 0)
        {
            _playerSprite.flipX = true;
        }
        else if(_rb2d.velocity.x > 0)
        {
            _playerSprite.flipX = false;
        }
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
    }
}
