using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(NetworkRigidbody), typeof(PlayerInput))]
public class Player : NetworkBehaviour {
    [SerializeField] private float speed = 5f;
    
    private Gun _gun;
    private Rigidbody _body;
    
    private readonly NetworkVariable<Vector2> _moveInput = new(writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Vector2> _lookInput = new(writePerm: NetworkVariableWritePermission.Owner);

    #region Client

    private void OnMove(InputValue moveInput) {
        _moveInput.Value = moveInput.Get<Vector2>();
    }

    private void OnLook(InputValue lookInput) {
        _lookInput.Value = lookInput.Get<Vector2>();
    }

    private void OnShoot() {
        _gun.ToggleFire();
    }

    #endregion // Client

    #region Server

    private void Move() {
        if (_moveInput.Value.magnitude < float.Epsilon) {
            if (_body.velocity.magnitude > float.Epsilon) {
                _body.velocity *= 0.5f;
            } 
            return;
        }

        var direction = _moveInput.Value.normalized;
        var directionXZ = new Vector3(direction.x, 0, direction.y);
        
        _body.velocity = directionXZ * speed;
    }
    
    private void Look() {
        if (_lookInput.Value.magnitude < float.Epsilon) {
            return;
        }

        var direction = _lookInput.Value.normalized;
        var directionXZ = new Vector3(direction.x, 0, direction.y);

        transform.forward = directionXZ;
    }

    #endregion //Server

    #region GameLoop

    private void Awake() {
        _body = GetComponent<Rigidbody>();
        _gun = GetComponentInChildren<Gun>();
    }

    private void Start() {
        gameObject.name = $"Player {OwnerClientId + 1}";
        
        if (!IsLocalPlayer) {
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    private void FixedUpdate() {
        if (!IsServer) {
            return;
        }
        
        Look();
        Move();
    }
    #endregion //GameLoop
}