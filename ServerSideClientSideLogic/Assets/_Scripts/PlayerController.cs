using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference _move;
    [SerializeField] Transform _transform;
    [SerializeField] float _speed;

    Vector2 _direction;
    Vector3 _movement;
    
    void OnValidate()
    {
        if (_transform == null) _transform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        _move.action.Enable();

        _move.action.performed += HandleOnMovement;
        _move.action.canceled += HandleOnMovement;
    }

    void OnDisable()
    {
        _move.action.Disable();

        _move.action.performed -= HandleOnMovement;
        _move.action.canceled -= HandleOnMovement;
    }

    void Update()
    {
        _movement = _speed * Time.deltaTime * _direction;
    }

    void FixedUpdate()
    {
        if (_movement.Equals(Vector3.zero)) return;
        
        _transform.position += _movement;
    }

    void HandleOnMovement(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }
}