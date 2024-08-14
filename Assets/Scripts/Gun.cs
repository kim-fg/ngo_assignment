using Unity.Netcode;
using UnityEngine;

public class Gun : NetworkBehaviour {
    [SerializeField] private NetworkObject projectilePrefab;
    [SerializeField] private float shotDelay = 0.1f;
    [Header("Lazy ref")] 
    [SerializeField] private Transform muzzlePoint;
    
    private float _timeSinceShot;
    private bool _isFiring;

    #region Client

    public void ToggleFire() {
        ToggleFire_ServerRPC();
    }

    #endregion

    #region Server

    [Rpc(SendTo.Server)]
    private void ToggleFire_ServerRPC() {
        _isFiring = !_isFiring;
    }

    private void SpawnProjectile() {
        if (!projectilePrefab) {
            return;
        }

        var projectileInstance = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
        projectileInstance.SpawnWithOwnership(OwnerClientId);
    }
    
    #endregion // Server

    #region GameLoop

    private void Start() {
        // let player shoot immediately
        _timeSinceShot = shotDelay;
    }
    
    private void Update() {
        if (!IsServer) {
            return;
        }
        
        _timeSinceShot += Time.deltaTime;

        if (!_isFiring) {
            return;
        }
        
        if (_timeSinceShot > shotDelay) {
            SpawnProjectile();
            _timeSinceShot = 0;
        }
    }

    #endregion // GameLoop
}
