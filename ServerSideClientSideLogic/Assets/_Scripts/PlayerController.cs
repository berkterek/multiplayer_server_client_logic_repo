using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] InputActionReference _move;
    [SerializeField] InputActionReference _leftButton;
    [SerializeField] InputActionReference _rightButton;
    [SerializeField] Transform _transform;
    [SerializeField] float _speed;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Color _color;

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
        _leftButton.action.Enable();
        _rightButton.action.Enable();

        _move.action.performed += HandleOnMovement;
        _move.action.canceled += HandleOnMovement;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _move.action.Disable();
        _leftButton.action.Disable();
        _rightButton.action.Disable();

        _move.action.performed -= HandleOnMovement;
        _move.action.canceled -= HandleOnMovement;
    }

    void Update()
    {
        if (!IsOwner) return;
        
        _movement = _speed * Time.deltaTime * _direction;

        if (_leftButton.action.WasPressedThisFrame())
        {
            //RandomColorChange();
            RandomColorChangeInsideEveryClientSideServerRpc();
        }

        if (_rightButton.action.WasPressedThisFrame())
        {
            RandomColorChangeAndColorDataSendToServerSideServerRpc();
        }
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

    private Color GetRandomColor()
    {
        float randomRed = Random.Range(0f, 1f);
        float randomGreen = Random.Range(0f, 1f);
        float randomBlue = Random.Range(0f, 1f);
        
        Color randomColor = new Color(randomRed, randomGreen, randomBlue);
        return randomColor;
    }

    private void RandomColorChange()
    {
        _color = GetRandomColor();
        _renderer.color = _color;
    }

    [ServerRpc]
    private void RandomColorChangeInsideEveryClientSideServerRpc()
    {
        RandomColorChangeInsideEveryClientSideClientRpc();
    }

    [ClientRpc]
    private void RandomColorChangeInsideEveryClientSideClientRpc()
    {
        _color = GetRandomColor();
        _renderer.color = _color;
    }
    
    [ServerRpc]
    private void RandomColorChangeAndColorDataSendToServerSideServerRpc()
    {
        _color = GetRandomColor();
        RandomColorChangeColorDataSendToClientsSideClientRpc(_color);
    }

    [ClientRpc]
    private void RandomColorChangeColorDataSendToClientsSideClientRpc(Color color)
    {
        _renderer.color = color;
    }
}