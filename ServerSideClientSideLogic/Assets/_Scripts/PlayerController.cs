using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] InputActionReference _move;
    [SerializeField] Transform _transform;
    [SerializeField] float _speed;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Color _color;
    [SerializeField] int _score = 0;

    Vector2 _direction;
    Vector3 _movement;
    
    void OnValidate()
    {
        if (_transform == null) _transform = GetComponent<Transform>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            _renderer.color = Color.red;
            return;
        }

        _renderer.color = Color.blue;
        
        _move.action.Enable();

        _move.action.performed += HandleOnMovement;
        _move.action.canceled += HandleOnMovement;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _move.action.Disable();

        _move.action.performed -= HandleOnMovement;
        _move.action.canceled -= HandleOnMovement;
    }

    void Update()
    {
        if (!IsOwner) return;
        
        _movement = _speed * Time.deltaTime * _direction;
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        
        if (_movement.Equals(Vector3.zero)) return;
        
        _transform.position += _movement;
    }

    void HandleOnMovement(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }
}